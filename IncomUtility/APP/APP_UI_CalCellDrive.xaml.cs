using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IncomUtility.APP
{
    /// <summary>
    /// APP_UI_CalCellDrive.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_CalCellDrive : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;
        Thread feedBack = null;
        bool feedBackRunning = false;

        int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
        public APP_UI_CalCellDrive()
        {
            InitializeComponent();
        }

        private void tBtn_Increase_Click(object sender, RoutedEventArgs e)
        {
            increaseDuty();
        }

        private void tBtn_Decrease_Click(object sender, RoutedEventArgs e)
        {
            decreaseDuty();
        }

        private void tBtn_StopCal_Click(object sender, RoutedEventArgs e)
        {
            calibrationStop();
        }

        private void tBtn_StartCal_Click(object sender, RoutedEventArgs e)
        {
            calibrationStart();
        }

        private void tBtn_AcceptCal_Click(object sender, RoutedEventArgs e)
        {
            calibrationAccept();
        }

        private void increaseDuty()
        {
            byte[] step = new byte[1];
            step[0] = (byte)tUpdown_AdjustPv.Value;
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_INC_DRIVE_DUTY, step, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - INCREASE DUTY");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Increase Duty : " + step[0].ToString());
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void decreaseDuty()
        {
            byte[] step = new byte[1];
            step[0] = (byte)tUpdown_AdjustPv.Value;
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_DEC_DRIVE_DUTY, step, ref err);
            if(err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - DECREASE DUTY");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Decrease Duty : " + step[0].ToString());
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void calibrationStart()
        {
            /*
             * Stop Calibration before Start
             */
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_SEN_DRIVE_CAL, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - START CALIBRATION");
                tTxt_Logs.AppendText(Environment.NewLine);

                return;
            }

            /*
             * Start Cell Drive Calibration
             */
            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_SEN_DRIVE_CAL, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - START CELL DRIVE CALIBRATION");
                tTxt_Logs.AppendText(Environment.NewLine);
                
                return;
            }

            byte cellType = result[offset];
            float targetVlaue = Utility.getF32FromByteA(result, offset + 1);

            tTxt_Logs.AppendText("START CELL DRIVE CAL");
            tTxt_Logs.AppendText(Environment.NewLine);
            
            string str = "";

            if (cellType == 0)
            {
                str = "Voltage (V)";
            }
            else if( cellType == 1)
            {
                str = "Current";
            }
            else if(cellType == 2)
            {
                str = "Resistance";
            }

            str += " / " + (targetVlaue /1000).ToString("N2");
            tTxt_TargetDrive.Text = str;

            /*
             * Start Feedback Thread
             */
            if (!feedBackRunning)
            {
                feedBack = new Thread(getFeedBack);
                feedBack.Start();
                feedBackRunning = true;
            }
        }
        public void calibrationStop()
        {
            if(feedBack != null)
            {
                feedBackRunning = false;
                feedBack.Join();

                SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_SEN_DRIVE_CAL, ref err);
                if(err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if(err != ERROR_LIST.ERROR_NONE)
                {
                    tTxt_Logs.AppendText("ERROR - STOP CELL DRIVE CALIBRATION");
                    tTxt_Logs.AppendText(Environment.NewLine);

                    return;
                }
            }
        }
        private void calibrationAccept()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ACCPET_SEN_DRIVE_CAL, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - ACCEPT CELL DRIVE CALIBRATION");
                tTxt_Logs.AppendText(Environment.NewLine);

                return;
            }

            tTxt_Logs.AppendText("Calibrate Cell Drive Successfully");
            tTxt_Logs.AppendText(Environment.NewLine);

            calibrationStop();
        }

        private void getFeedBack()
        {
            while (feedBackRunning)
            {
                byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CELL_VOLTAGE_CURRENT, ref err);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    continue;
                }
                byte cellDriveType = result[offset];
                if (cellDriveType == 0)
                {
                    float cellVoltage = Utility.getF32FromByteA(result, offset + 1);
                    tTxt_CellDriveFeedback.Text = cellVoltage.ToString("N2");
                }
                else if (cellDriveType == 1)
                {
                    float cellCurrent = Utility.getF32FromByteA(result, offset + 5);
                    tTxt_CellDriveFeedback.Text = cellCurrent.ToString("N2");

                }

                Thread.Sleep(1000);
            }
        }
    }
}
