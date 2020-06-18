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
    /// APP_UI_CalVoltageOutput.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_CalVoltageOutput : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;
        bool voltageOutputRunning = false;
        Thread voltageOutput = null;
       
        int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;

        public APP_UI_CalVoltageOutput()
        {
            InitializeComponent();
        }

        private void tBtn_StopCal_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrateStop();
        }

        private void tBtn_StartCal_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibarteStart();
        }

        private void tBtn_AcceptZero_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrateAccept();
        }

        private void tBtn_StopSpan_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrateStop();
        }

        private void tBtn_StartSpan_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrateStart();
        }

        private void tBtn_AcceptSpan_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrateAccept();
        }

        private void zeroCalibarteStart()
        {
            float targetVoltage = 0;
            byte[] payload = Utility.getBytesFromF32(targetVoltage);

            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_VOLTAGE_ZERO_CAL, payload, ref err);
            if(err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Cannot start Voltage Calibartion");
                return;
            }
            if(err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - START ZERO CALIBRATION");
                return;
            }

            if (!voltageOutputRunning)
            {
                voltageOutput = new Thread(getVoltageOutput);
                voltageOutput.Start();
                voltageOutputRunning = true;
            }
        }

        public void zeroCalibrateStop()
        {
            if (voltageOutput != null)
            {
                voltageOutputRunning = false;
                voltageOutput.Join();

                QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_VOLTAGE_ZERO_CAL, ref err);

                if (err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - STOP VOLTAGE ZERO CALIBRATION");
                    return;
                }
            }
        }

        private void zeroCalibrateAccept()
        {
            float targetCurrent = (float)tUpdown_VoltageOutputZero.Value;
            byte[] payload = Utility.getBytesFromF32(targetCurrent);
            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ACCEPT_VOLTAGE_ZERO_CAL, payload, ref err);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Failed Voltage Zero Accept");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - ACCEPT ZERO CALIBRATION");
                return;
            }
            MessageBox.Show("Success Voltage Zero Accept");

            zeroCalibrateStop();

        }

        private void spanCalibrateStart()
        {
            float targetVoltageConc = (float)tUpdown_TargetConc.Value;
            byte[] payload = Utility.getBytesFromF32(targetVoltageConc);

            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_VOLTAGE_SPAN_CAL, payload, ref err);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Cannot start Voltage SPAN Calibartion");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - START SPAN CALIBRATION");
                return;
            }

            if (!voltageOutputRunning)
            {
                voltageOutput = new Thread(getVoltageOutput);
                voltageOutput.Start();
                voltageOutputRunning = true;
            }
        }

        public void spanCalibrateStop()
        {
            if (voltageOutput != null)
            {
                voltageOutputRunning = false;
                voltageOutput.Join();

                QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_VOLTAGE_SPAN_CAL, ref err);

                if (err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - STOP VOLTAGE SPAN CALIBRATION");
                    return;
                }
            }
        }

        private void spanCalibrateAccept()
        {
            float targetCurrent = (float)tUpdown_VoltageOutputSpan.Value;
            byte[] payload = Utility.getBytesFromF32(targetCurrent);
            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ACCEPT_ANALOGUE_SPAN_CAL, payload, ref err);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Failed Voltage Span Calibartion");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - ACCEPT SPAN CALIBRATION");
                return;
            }
            MessageBox.Show("Success Voltage Span Calibartion");

            spanCalibrateStop();
        }

        private void getVoltageOutput()
        {
            while (voltageOutputRunning)
            {
                byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_ANALOGUE_OUTPUT, ref err);

                if (err != ERROR_LIST.ERROR_NONE)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                float voltageLoopback = Utility.getF32FromByteA(result, offset + 4);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tUpdown_VoltageOutputSpan.Value = voltageLoopback;
                    tUpdown_VoltageOutputZero.Value = voltageLoopback;
                }));


                Thread.Sleep(1000);
            }
        }

        public void threadClose()
        {
            if (voltageOutputRunning)
                voltageOutput.Abort();
        }

    }
}
