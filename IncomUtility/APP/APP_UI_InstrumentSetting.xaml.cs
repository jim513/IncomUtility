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
    /// APP_UI_InstrumentSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_InstrumentSetting : Window
    {
        SenParamTable paramTable;
        CONFIG_PARAM_TABLE_STRUCT param;
        ERROR_LIST err;
        public APP_UI_InstrumentSetting()
        {
            InitializeComponent();

            paramTable = new SenParamTable();
            param = new CONFIG_PARAM_TABLE_STRUCT(0,0,0);
        }

        private byte[] GetParmCfg(INNCOM_CONF com)
        {
            bool found = paramTable.SearchParmCfg((ushort)com, ref param);
            if (!found)
                return null;

            byte[] payload = new byte[2];
            payload = BitConverter.GetBytes(param.u16_parm_index);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(payload);

            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CONFIG, payload, ref err, 350);
            if (err != ERROR_LIST.ERROR_NONE)
                return null;

            byte[] parmaValue = Quattro.getResponseValueData(result);

            return parmaValue;
            
        }
        private void tBtn_ReadAlarms_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;
            }
            /*
             * Read Alarm threshold 1
             */
            byte[] value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_THRESHOLD1);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM THRESHOLD 1");
                return;
            }
            float alarmThresholdvalue = Utility.getF32FromByteA(value, 2);
            tUpdown_AlarmThreshold1.Value = alarmThresholdvalue;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
             * Read Alarm threshold 2
             */
            value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_THRESHOLD2);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM THRESHOLD 2");
                return;
            }
            alarmThresholdvalue = Utility.getF32FromByteA(value, 2);
            tUpdown_AlarmThreshold2.Value = alarmThresholdvalue;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Alarm threshold 3
            */
            value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_THRESHOLD3);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM THRESHOLD 3");
                return;
            }
            alarmThresholdvalue = Utility.getF32FromByteA(value, 2);
            tUpdown_AlarmThreshold3.Value = alarmThresholdvalue;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Alarm Trigger 1
            */
            value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_TRIGGER1);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Trigger 1");
                return;
            }          
            tCmb_AlarmTrigger1.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
            * Read Alarm Trigger 2
            */
            value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_TRIGGER2);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Trigger 2");
                return;
            }
            tCmb_AlarmTrigger2.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
            * Read Alarm Trigger 3
            */
            value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_TRIGGER3);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Trigger 3");
                return;
            }
            tCmb_AlarmTrigger3.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Alarm Latching
            */
            value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_LATCHING);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Latching");
                return;
            }
            tCmb_AlarmLacting.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Gas Alarm Parameters");

        }

        /*
         *  Not Completed
         */
        private void tBtn_ReadCal_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;
            }
            /*
             * Read Cal Interval
             */
            byte[] value = GetParmCfg(INNCOM_CONF.CAL_PARAM_CAL_INTERVAL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Calibration Interval");
                return;
            }
            ushort CalInterval = Utility.getU16FromByteA(value, 2);
            tUpdown_CalInterval.Value = CalInterval;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Cal Con
            */
            value = GetParmCfg(INNCOM_CONF.ALARM_PARAM_THRESHOLD2);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM THRESHOLD 2");
                return;
            }
            float CalCon = Utility.getF32FromByteA(value, 2);
            tUpdown_CalCon.Value = CalCon;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


        }
    }
}
