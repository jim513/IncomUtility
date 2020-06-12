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

namespace IncomUtility
{

    /// <summary>
    /// APP_UI_GasCalibration.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_GasCalibration : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;

        int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;

        bool zeroCalibrationRunning = false;
        bool spanCalibrationRunning = false;
        Thread zeroCal = null;
        Thread spanCal = null;
        public APP_UI_GasCalibration()
        {
            InitializeComponent();
        }


        private void tBtn_StopCal_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrationStop();
        }

        private void tBtn_AcceptZero_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrateAccept();
        }

        private void tBtn_SpanCalAccept_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrateAccept();
        }

        private void tBtn_SpanGasNext_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrationStart();
        }

        private void tBtn_CalTypeStart_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrationStart();
        }


        private void tTab_GasCalibration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tabSelectControl();
        }


        private void tBtn_SpanCalStop_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrationStop();
        }    

        private void zeroCalibrationStart()
        {
            /*
             *  Check Sensor Gas Type
             */
            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_SENSOR_INFO, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Gas Calibration Start");
                return;
            }

            byte gasType = result[offset + 1];
                                
            if(gasType == (byte)GAS_TYPE.O2)
            {
                MessageBox.Show("O2 Sensor Does not need zero calibration");
                setInhitbitOutput();
                return;
            }
            /*
             *  Stop Zero Cal befor Start
             */
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_ZERO_CAL, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Gas Calibration Start");
                return;
            }
            /*
             * Start Zero Cal;
             */
            byte[] payload0 = new byte[1];
            payload0[0] = (byte)tCmb_CalibrationType.SelectedIndex;

            float baseLineConcentrate = 0;
            byte[] payload1 = Utility.getBytesFromF32(baseLineConcentrate);
            payload0 = Utility.mergeByteArray(payload0, payload1);
            
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_ZERO_CAL, payload0, ref err ,200);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Gas Calibration Start");
                return;
            }
            zeroCalibrationRunning = true;
            tTabitem_CalType.IsEnabled = false;
            tTabitem_TargetSpanGas.IsEnabled = false;
            tTabitem_SpanCalibration.IsEnabled = false;

            tTab_GasCalibration.SelectedIndex = 1;
          
        }

        private void spanCalibrationStart()
        {
            setInhitbitOutput();

            /*
           *  Stop Span Cal befor Start
           */
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_SPAN_CAL, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Span Calibration Start");
                return;
            }
            /*
             * Start Span Cal;
             */
            byte[] payload0 = new byte[1];
            payload0[0] = (byte)tCmb_CalibrationType.SelectedIndex;
            float targetSpan = 0;
            try
            {
                targetSpan = float.Parse(tTxt_TargetSpanConc.Text);
            }
            catch
            {
                MessageBox.Show("ERROR - Span Calibration Parse");
                return;
            }
            byte[] payload1 = Utility.getBytesFromF32(targetSpan);
            payload0 = Utility.mergeByteArray(payload0, payload1);
            
            float targetsensitivitiy = 1;
            payload1 = Utility.getBytesFromF32(targetsensitivitiy);
            payload0 = Utility.mergeByteArray(payload0, payload1);

            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_SPAN_CAL, payload0, ref err, 200);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Span Calibration Start");
                return;
            }

            spanCalibrationRunning = true;
            tTabitem_CalType.IsEnabled = false;
            tTabitem_TargetSpanGas.IsEnabled = false;
            tTabItem_Zero.IsEnabled = false;

            tTab_GasCalibration.SelectedIndex = 3;

            tPbar_GasReading.Maximum = 20;
            tPbar_GasReading.Value = 0;

            spanCal = new Thread(spanCalibrate);
            spanCal.Start();
        }
        /*
         * Need Debug
         */
        private void zeroCalibrateAccept()
        {
            if(zeroCalibrationRunning == false)
            {
                return;
            }

            byte[] payload0 = new byte[1];
            payload0[0] = (byte)tCmb_CalibrationType.SelectedIndex;

            float baseLineConcentrate = 0;
            byte[] payload1 = Utility.getBytesFromF32(baseLineConcentrate);
            payload0 = Utility.mergeByteArray(payload0, payload1);

            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ACCEPT_ZERO_CAL,payload0, ref err ,400);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Failed in zeroing");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - ZERO CLIBRATION");
                return;
            }
        
            float zeroCalibrationFactor = Utility.getF32FromByteA(result,offset);
            float gasReading = Utility.getF32FromByteA(result, offset + 4);

            tTxt_ZeroCalibration.Text = gasReading.ToString("N1");
            MessageBox.Show("Calculated Zero Factor: "+ zeroCalibrationFactor.ToString("N2"));  
        }
        
        private void spanCalibrateAccept()
        {

            if (spanCalibrationRunning == false)
            {
                return;
            }
            /*
             *  Get data to accept span
             */
            byte[] payload0 = new byte[1];
            payload0[0] = (byte)tCmb_CalibrationType.SelectedIndex;
            float targetSpan = 0;
            try
            {
                targetSpan = float.Parse(tTxt_TargetSpanConc.Text);
            }
            catch
            {
                MessageBox.Show("ERROR - Span Calibration Parse");
                return;
            }
            byte[] payload1 = Utility.getBytesFromF32(targetSpan);
            payload0 = Utility.mergeByteArray(payload0, payload1);

            byte[] payload2 = Utility.getBytesFromU16((ushort)INNCOM_CONF_LIST.GAS_PARAM_CORRECTION_FACTOR);
            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CONFIG, payload2, ref err, 200);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read Span Gas Data");
                return;
            }
            byte[] sensitivity = new byte[4];
            Array.Copy(result, offset + (int)INNCOM_CONF.SZ_PARAM_INDEX, sensitivity, 0, 4);
            Utility.mergeByteArray(payload0, sensitivity);

            /*
             * Accept Span Cal
             */
            result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ACCEPT_SPAN_CAL, payload0, ref err, 400);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Failed in spaning");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - ZERO CLIBRATION");
                return;
            }

            float spanCalibrationFactor = Utility.getF32FromByteA(result, offset);

            float gasReading = Utility.getF32FromByteA(result, offset + 4);
            tTxt_GasReading.Text = gasReading.ToString("N1");

            MessageBox.Show("Calculated Zero Factor: " + spanCalibrationFactor.ToString("N2"));

        }

        private void tabSelectControl()
        {
            if(tTabItem_Zero == null)
            {
                return;
            }
            if(tTabitem_TargetSpanGas == null)
            {
                return;
            }
            if(tTabItem_Zero.IsSelected)
            {
                if(zeroCalibrationRunning)
                {
                    zeroCal = new Thread(zeroCalibrate);
                    zeroCal.Start();
                }
            }
            if (tTabitem_TargetSpanGas.IsSelected)
            {
                /*
                 * Read Sensor Info
                 */
                byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_SENSOR_INFO, ref err);
                if (err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - Read Span Gas Sensor");
                    return;
                }

                byte gasType = result[offset + 1];

                /*
                 * read target span gas
                 */
                byte[] payload = Utility.getBytesFromU16((ushort)INNCOM_CONF_LIST.CAL_PARAM_CAL_CONC);
                result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CONFIG, payload, ref err, 200);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - Read Span Gas Data");
                    return;
                }
                float CalCon = Utility.getF32FromByteA(result, offset + (int)INNCOM_CONF.SZ_PARAM_INDEX);
                tTxt_TargetSpanConc.Text = CalCon.ToString("N1");

                payload = Utility.getBytesFromU16((ushort)INNCOM_CONF_LIST.GAS_PARAM_MEASURING_RANGE);
                result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CONFIG, payload, ref err, 200);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - Read Span Gas Data");
                    return;
                }
                float measuringRange = Utility.getF32FromByteA(result, offset + (int)INNCOM_CONF.SZ_PARAM_INDEX);
                string str = "";
                if (gasType == (int)GAS_TYPE.O2)
                {
                    str = " Configuralbe Target Calibration Conc. : " + (0.7 * measuringRange).ToString("N1") + " ~ " + (0.9 * measuringRange).ToString("N1");
                }
                else
                {
                    str = " Configuralbe Target Calibration Conc. : " + (0.3 * measuringRange).ToString("N1") + " ~ " + (0.7 * measuringRange).ToString("N1");
                }
                tTxt_TargetSpanRange.Text = str;
            }
        }

        private void zeroCalibrate()
        {
            byte[] result = null;
            byte[] payload = { 0x00 };
            while (zeroCalibrationRunning)
            {

                result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_GAS_READING,payload, ref err ,100);
               
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tTxt_ZeroCalibration.Text = Utility.getF32FromByteA(result, offset + 1).ToString("N1");
                }));
                Thread.Sleep(1000);

            }
        }

        private void spanCalibrate()
        {
            byte[] result = null;
            byte[] payload = { 0x00 };
            while (spanCalibrationRunning)
            {

                result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_GAS_READING, payload, ref err, 100);

                if (err != ERROR_LIST.ERROR_NONE)
                {
                    continue;
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tTxt_GasReading.Text = Utility.getF32FromByteA(result, offset + 1).ToString("N1");
                    if (tPbar_GasReading.Value < 20)
                    {
                        tPbar_GasReading.Value++;
                    }
                }));

                Thread.Sleep(1000);
            }
        }

        public void spanCalibrationStop()
        {
            if (spanCal != null)
            {
                spanCalibrationRunning = false;
                spanCal.Join();

                SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_SPAN_CAL, ref err);
                
                releaseOutput();

                if( err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - STOP SPAN CLIBRATION");
                    return;
                }
            }
            tTabitem_CalType.IsEnabled = true;
            tTabitem_TargetSpanGas.IsEnabled = true;
            tTabItem_Zero.IsEnabled = true;

        }

        public void zeroCalibrationStop()
        {
            if(zeroCal != null)
            {
                zeroCalibrationRunning = false;
                zeroCal.Join();
               
                SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_ZERO_CAL, ref err);
                releaseOutput();
                if (err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - STOP ZERO CLIBRATION");
                    return;
                }
            }
            tTabitem_CalType.IsEnabled = true;
            tTabitem_TargetSpanGas.IsEnabled = true;
            tTabitem_SpanCalibration.IsEnabled = true;
            tTab_GasCalibration.SelectedIndex = 0;
        }

        public bool setInhitbitOutput()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_INHIBIT_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {        
                return false;
            }     

            return true;
        }

        public bool releaseOutput()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_RELEASE_OUTPUT, ref err, 200);
            if (err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
            {
                return false ;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {          
                return false;
            }
            return true;
        }

       
    }
}
