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
    /// APP_UI_CalAnalogueOutput.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_CalAnalogueOutput : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;
        bool mAOutputRunning = false;
        Thread mAOutput = null;

        int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
        public APP_UI_CalAnalogueOutput()
        {
            InitializeComponent();
        }

        private void tBtn_StopCal_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrateStop();
        }

        private void tBtn_StartCal_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrateStart();
        }

        private void tBtn_AcceptZero_Click(object sender, RoutedEventArgs e)
        {
            zeroCalibrateAccept();
        }

        private void tBtn__StopSapn_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrateStop();
        }

        private void tBtn_StartSpan_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrateStart();
        }

        private void tBtn_AcceptSapn_Click(object sender, RoutedEventArgs e)
        {
            spanCalibrateAccept();
        }

        private void zeroCalibrateStart()
        {
            float targetmA = 4;
            byte[] payload = Utility.getBytesFromF32(targetmA);

            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_ANALOGUE_ZERO_CAL, payload, ref err);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Cannot start Analouge Zero Calibartion");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - START ZERO CALIBRATION");
                return;
            }

            if (!mAOutputRunning)
            {
                mAOutput = new Thread(getmAOutput);
                mAOutput.Start();
                mAOutputRunning = true;
            }
        }

        public void zeroCalibrateStop()
        {
            if (mAOutput != null)
            {
                mAOutputRunning = false;
                mAOutput.Join();

                QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_ANALOGUE_ZERO_CAL, ref err);

                if (err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - STOP ANALOGUE ZERO CALIBRATION");
                    return;
                }
            }
        }

        private void zeroCalibrateAccept()
        {
            float targetCurrent = (float)tUpdown_AnalogueOutputZero.Value;
            byte[] payload = Utility.getBytesFromF32(targetCurrent);
            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ACCEPT_ANALOGUE_ZERO_CAL,payload, ref err);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Failed Analouge Zero Calibartion");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - ACCEPT ZERO CALIBRATION");
                return;
            }
            MessageBox.Show("Success Analouge Zero Calibartion");

            zeroCalibrateStop();

        }

        private void spanCalibrateStart()
        {
            float targetmA = 20;
            byte[] payload = Utility.getBytesFromF32(targetmA);

            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_ANALOGUE_SPAN_CAL, payload, ref err);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Cannot start Analouge SPAN Calibartion");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - START SPAN CALIBRATION");
                return;
            }

            if (!mAOutputRunning)
            {
                mAOutput = new Thread(getmAOutput);
                mAOutput.Start();
                mAOutputRunning = true;
            }
        }

        public void spanCalibrateStop()
        {
            if (mAOutput != null)
            {
                mAOutputRunning = false;
                mAOutput.Join();

                QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_ANALOGUE_SPAN_CAL, ref err);

                if (err == ERROR_LIST.ERROR_PORT_NOT_OPEN)
                {
                    return;
                }
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - STOP ANALOGUE SPAN CALIBRATION");
                    return;
                }
            }
        }

        private void spanCalibrateAccept()
        {
            float targetCurrent = (float)tUpdown_analogueOutputSpan.Value;
            byte[] payload = Utility.getBytesFromF32(targetCurrent);
            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ACCEPT_ANALOGUE_SPAN_CAL, payload, ref err);
            if (err == ERROR_LIST.ERROR_NCK)
            {
                MessageBox.Show("Failed Analouge Zero Calibartion");
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - ACCEPT ZERO CALIBRATION");
                return;
            }
            MessageBox.Show("Success Analouge Span Calibartion");

            spanCalibrateStop();
        }

        private void getmAOutput()
        {
            while (mAOutputRunning) {
                byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_ANALOGUE_OUTPUT, ref err);

                if (err != ERROR_LIST.ERROR_NONE)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                float mAloopback = Utility.getF32FromByteA(result, offset + 5);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tTxt_mAOutputSapn.Text = mAloopback.ToString("N2");
                    tTxt_mAOutputZero.Text = mAloopback.ToString("N2");
                }));
             

                Thread.Sleep(1000);
            }
        }

        public void threadClose()
        {
            if (mAOutputRunning)
                mAOutput.Abort();
        }
    }
}
