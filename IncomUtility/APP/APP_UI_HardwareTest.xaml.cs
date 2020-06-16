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
    /// APP_UI_HardwareTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_HardwareTest : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;
        public APP_UI_HardwareTest()
        {
            InitializeComponent();
        }

        private void tBtn_ForceCurrent_Click(object sender, RoutedEventArgs e)
        {
            forceCurrent();
        }

        private void tBtn_ReleaseCurrent_Click(object sender, RoutedEventArgs e)
        {
            releaseCurrent();
        }

        private void tBtn_ForceVoltage_Click(object sender, RoutedEventArgs e)
        {
            forceVoltage();
        }

        private void tBtn_ReleaseVoltage_Click(object sender, RoutedEventArgs e)
        {
            releaseVoltage();
        }

        private void tBtn_CtrlWatchdog_Click(object sender, RoutedEventArgs e)
        {
            ctrlWatchdog();
        }

        private void tBtn_ForceLEDs_Click(object sender, RoutedEventArgs e)
        {
            forceLEDs();
        }

        private void tBtn_ReleaseLEDs_Click(object sender, RoutedEventArgs e)
        {
            releaseLEDs();
        }

        private void tBtn_ForceRelays_Click(object sender, RoutedEventArgs e)
        {
            forceRelays();
        }

        private void tBtn_ReleaseRelays_Click(object sender, RoutedEventArgs e)
        {
            releaseRelays();
        }

        private void tBtn_InhibitOutput_Click(object sender, RoutedEventArgs e)
        {
            setInhitbitOutput();
        }

        private void tBtn_ReleaseOutput_Click(object sender, RoutedEventArgs e)
        {
            releaseOutput();
        }

        private void tBtn_EnableInternalLED_Click(object sender, RoutedEventArgs e)
        {
            enableInternalLED();
        }

        private void tBtn_DisableInternalLED_Click(object sender, RoutedEventArgs e)
        {
            disableInternalLED();
        }

        private void tBtn_StartSensorReplacement_Click(object sender, RoutedEventArgs e)
        {
            startSensorReplacement();
        }

        private void tBtn_ExitSensorReplacement_Click(object sender, RoutedEventArgs e)
        {
            exitSensorReplacement();
        }

        private void tBtn_CancleSensorReplacement_Click(object sender, RoutedEventArgs e)
        {
            cancleSensorReplcement();
        }

        private void tBtn_EnableReflexTest_Click(object sender, RoutedEventArgs e)
        {
            enableReflexTest();
        }

        private void tBtn_DisableReflexTest_Click(object sender, RoutedEventArgs e)
        {
            disableReflexTest();
        }

        public bool setInhitbitOutput()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_INHIBIT_OUTPUT, ref err, 300);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - INHIBITING OUTPUT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return false;
            }

            tTxt_Logs.AppendText("Output has been inhibited");
            tTxt_Logs.AppendText(Environment.NewLine);

            tBtn_InhibitOutput.IsEnabled = false;
            tBtn_ReleaseOutput.IsEnabled = false;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            Thread.Sleep(500);

            tBtn_InhibitOutput.IsEnabled = true;
            tBtn_ReleaseOutput.IsEnabled = true;

            return true;
        }

        public bool releaseOutput()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_RELEASE_OUTPUT, ref err, 300);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - RELEASEING OUTPUT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return false;
            }

            tTxt_Logs.AppendText("Output has been released");
            tTxt_Logs.AppendText(Environment.NewLine);
           
            tBtn_InhibitOutput.IsEnabled = false;
            tBtn_ReleaseOutput.IsEnabled = false;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            Thread.Sleep(500);

            tBtn_InhibitOutput.IsEnabled = true;
            tBtn_ReleaseOutput.IsEnabled = true;
            return true;
        }
      
        private void enableInternalLED()
        {
            byte[] switchOn = { 0x01 };
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_CTRL_INTERNAL_LEDS, switchOn, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - SWITCGING ON INTERNAL LEDs");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Internal LEDs are on");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void disableInternalLED()
        {
            byte[] switchOff = { 0x00 };
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_CTRL_INTERNAL_LEDS, switchOff,ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - SWITCGING OFF INTERNAL LEDs");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Internal LEDs are off");
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void cancleSensorReplcement()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_CANCEL_SENSOR_REPLACEMENT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - CANCLE SENSRO REPLACEMENT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_RELEASE_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - CANCLE SENSRO REPLACEMENT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Sensor has been powered up. It will take 60 seconds to complete the warm-up");
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void startSensorReplacement()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_REPLACE_SENSOR, ref err,200);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - SENSOR NOT POWERED OFF");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Sensor has been powered off. Please replace sensor");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void exitSensorReplacement()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_EXIT_REPLACE_SENSOR, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - EXIT SENSRO REPLACEMENT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Sensor has been powered up. It will take 60 seconds to complete the warm-up");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void forceCurrent()
        {
            float current = (float)tUpdonw_ForceCurrent.Value;
            byte[] payload = Utility.getBytesFromF32(current);

            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_FORCE_ANALOGUE_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - FORCE CURRENT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Force Current : " + current.ToString());
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void releaseCurrent()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_RELEASE_ANALOGUE_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - RELEASE CURRENT");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Force Current OK");
            tTxt_Logs.AppendText(Environment.NewLine);
        }
        
        private void forceLEDs()
        {
            byte[] payload = new byte[3];

            payload[0] = (byte)(tCmb_LEDType.SelectedIndex + 1);
            payload[1] = (byte)tCmb_Switch.SelectedIndex;

            if ((bool)tChb_InternalLEDs.IsChecked)
            {
                payload[2] = 1;
            }
            else
            {
                payload[2] = 0;
            }
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_FORCE_LEDS,payload, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - FORCE LEDs");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Forced LEDs OK");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void releaseLEDs()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_RELEASE_LEDS, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - RELEASE LEDs");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Release LEDs OK");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void forceRelays()
        {
            byte[] payload = new byte[2];

            payload[0] = (byte)(tCmb_RelayType.SelectedIndex + 1);
            payload[1] = (byte)tCmb_Energized.SelectedIndex;

            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_FORCE_RELAYS, payload, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - FORCE RELAYS");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Forced Relays OK");
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void releaseRelays()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_RELEASE_RELAYS, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - RELEASE RELAYS");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Relase Relays OK");
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void forceVoltage()
        {
            float current = (float)tUpdown_ForceVoltage.Value;
            byte[] payload = Utility.getBytesFromF32(current);

            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_FORCE_VOLTAGE_OUTPUT,payload, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - FORCE VOLTAGE");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Force Voltage : " + current.ToString());
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void releaseVoltage()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_RELEASE_VOLTAGE_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - RELEASE VOLTAGE");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Release Voltage OK");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void ctrlWatchdog()
        {
            byte[] payload = new byte[1];

            if ((bool)tChb_WatchdogEnabled.IsChecked)
            {
                payload[0] = 1;
            }
            else
            {
                payload[0] = 0;
            }
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_CTRL_WATCHDOG, payload, ref err ,200);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - CONTROLLING WATCHDOG ");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            if ((bool)tChb_WatchdogEnabled.IsChecked)
            {
                tTxt_Logs.AppendText("Enabled Watchdog");
                tTxt_Logs.AppendText(Environment.NewLine);
            }
            else
            {
                tTxt_Logs.AppendText("Disabled Watchdog");
                tTxt_Logs.AppendText(Environment.NewLine);
            }

        }

        /*
         *  Not Used
         */
         /*
        private void startSimulation()
        {
            byte[] payload = { 0x00 };

            if ((bool)tChb_Alarm.IsChecked)
            {
               payload[0] |= Utility.setBitPos(payload[0], 0);
            }

            if ((bool)tChb_Warning.IsChecked)
            {
                payload[0] |= Utility.setBitPos(payload[0], 1);
            }

            if ((bool)tChb_Fault.IsChecked)
            {
                payload[0] |= Utility.setBitPos(payload[0], 2);
            }

            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_START_SIMULATION,payload, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - START SIMULATION");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Start Simulation");
            tTxt_Logs.AppendText(Environment.NewLine);
        }
        */
        /*
         *  Not Used
         */
         /*
        private void stopSimulation()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_STOP_SIMULATION, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - STOP SIMULATION");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Stop Simulation");
            tTxt_Logs.AppendText(Environment.NewLine);
        }
        */

        private void enableReflexTest()
        {
             SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_ENABLE_REFLEX_TEST, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
                {
                    tTxt_Logs.AppendText("ERROR - ENABLE REFLEX TEST");
                    tTxt_Logs.AppendText(Environment.NewLine);
                    return;
                }
            tTxt_Logs.AppendText("Enabled Reflex Test for ECC sensor");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void disableReflexTest()
        {
            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_DISABLE_REFLEX_TEST, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - DISABLE REFLEX TEST");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Disabled Reflex Test for ECC sensor");
            tTxt_Logs.AppendText(Environment.NewLine);
        }
    }
}
