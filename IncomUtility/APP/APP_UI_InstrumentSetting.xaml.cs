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

        private byte[] GetParmCfg(INNCOM_CONF_LIST com)
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
            byte[] value = GetParmCfg(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD1);
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
            value = GetParmCfg(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD2);
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
            value = GetParmCfg(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD3);
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
            value = GetParmCfg(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER1);
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
            value = GetParmCfg(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER2);
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
            value = GetParmCfg(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER3);
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
            value = GetParmCfg(INNCOM_CONF_LIST.ALARM_PARAM_LATCHING);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Latching");
                return;
            }
            tCmb_AlarmLacting.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Gas Alarm Parameters");

        }

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
            byte[] value = GetParmCfg(INNCOM_CONF_LIST.CAL_PARAM_CAL_INTERVAL);
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
            value = GetParmCfg(INNCOM_CONF_LIST.CAL_PARAM_CAL_CONC);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Calibration Conc.");
                return;
            }
            float CalCon = Utility.getF32FromByteA(value, 2);
            tUpdown_CalCon.Value = CalCon;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
             * Read Last Cal Date
              */
            value = GetParmCfg(INNCOM_CONF_LIST.CAL_PARAM_LAST_CAL_DATE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Last Calibration Date");
                return;
            }
            int year = Utility.getU16FromByteA(value, 2);
            byte month = value[4];
            byte day = value[5];
            tDate_LastCalDate.SelectedDate = new DateTime(year, month, day);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Days Since Last Cal
            */
            value = GetParmCfg(INNCOM_CONF_LIST.CAL_PARAM_DAYS_SINCE_LAST_CAL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Days Since Last Cal");
                return;
            }
            int days = Utility.getU16FromByteA(value, 2);
            tUpdown_DayPassed.Value = days;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Calibration Parameters");
        }

        private void tBtn_ReadCircuitCal_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;
            }
            /*
             * Read 4-20mA Offset sink
             */
            byte[] value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Offset Sink");
                return;
            }
            float _420mAOffsetSink = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit1.Value = _420mAOffsetSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
          
            /*
             * Read 4-20mA Span Sink
             */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Span Sink");
                return;
            }
            float _420mASpanSink = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit2.Value = _420mASpanSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read 4-20mA Offset Source
            */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Offset Source");
                return;
            }
            float _420mAOffsetSource = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit3.Value = _420mAOffsetSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        
            /*
             * Read 4-20mA Span Source
             */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Span Source");
                return;
            }
            float _420mASpanSource = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit4.Value = _420mASpanSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Loop Offset Sink
             */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read  Loop Offset Sink");
                return;
            }
            float loopOffsetSink = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit5.Value = loopOffsetSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

          
            /*
             * Read Loop Span Sink
             */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Loop Span Sink");
                return;
            }
            float loopSpanSink = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit6.Value = loopSpanSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
           * Read Loop Offset Source
           */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read  Loop Offset Source");
                return;
            }
            float loopOffsetSource = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit7.Value = loopOffsetSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
           * Read Loop Span Source
           */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Loop Span Source");
                return;
            }
            float loopSpanSource = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit8.Value = loopSpanSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
          * Read Voltage Out Offset
          */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_OFFSET);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Voltage Out Offset");
                return;
            }
            float voltagegOutOffset = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit9.Value = voltagegOutOffset;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
          * Read Voltage Out Span
          */
            value = GetParmCfg(INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_SPAN);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Voltage Out Span");
                return;
            }
            float voltagegOutSpan = Utility.getF32FromByteA(value, 2);
            tUpdown_Circuit10.Value = voltagegOutSpan;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Circuit Calibration Parameters");

        }

        private void tBtn_ReadGeneral_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;
            }
            /*
             * Read Parameters Location Tag
             */
            byte[] value = GetParmCfg(INNCOM_CONF_LIST.GENERAL_PARAM_LOCATION_TAG);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Parameters Location Tag");
                return;
            }
            string str = Encoding.Default.GetString(value,2,value.Length -2).Trim('\0');
            tTxt_LocationTag.Text = str;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read LED Control
             */
            value = GetParmCfg(INNCOM_CONF_LIST.GENERAL_PARAM_LED_CONTROL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read LED Control");
                return;
            }
            tCmb_LEDControl.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Alarm Operation Mode
             */
            value = GetParmCfg(INNCOM_CONF_LIST.GENERAL_PARAM_ALARM_OP_MODE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Alarm Operation Mode");
                return;
            }
            tCmb_AlarmOperationMode.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            
            /*
             * Read Safety Mode
             */
            value = GetParmCfg(INNCOM_CONF_LIST.GENERAL_PARAM_OP_MODE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Saftey Mode");
                return;
            }
            tCmb_SafetyMode.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Calibration Overdue
             */
            value = GetParmCfg(INNCOM_CONF_LIST.GENERAL_PARAM_CAL_OVERDUE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Calibration Overdue");
                return;
            }
            tCmb_CalOverdue.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
           * Read Passcode
           */
            value = GetParmCfg(INNCOM_CONF_LIST.GENERAL_PARAM_PASSCODE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Passcode");
                return;
            }
            tUpdown_Passcode1.Value = value[2];
            tUpdown_Passcode2.Value = value[3];
            tUpdown_Passcode3.Value = value[4];
            tUpdown_Passcode4.Value = value[5];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("'Read General Parameters");

        }

        private void tBtn_ReadmAOutput_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;
            }
            /*
             * Read mA Fault Current
             */
            byte[] value = GetParmCfg(INNCOM_CONF_LIST.MA_PARAM_FAULT_CURRENT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read mA Fault Current");
                return;
            }
            tUpdown_FaultCurrent.Value = value[2] / 10;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
           * Read mA Warning Current
           */
            value = GetParmCfg(INNCOM_CONF_LIST.MA_PARAM_WARNING_CURRENT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read mA Warning Current");
                return;
            }
            tUpdown_WarningCurrent.Value = value[2] / 10;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
           * Read mA Over Range Curretn
           */
            value = GetParmCfg(INNCOM_CONF_LIST.MA_PARAM_OVERRANGE_CURRENT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read mA Over Range Curretn");
                return;
            }
            tUpdown_OverRangeCurrent.Value = value[2] / 10;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Inhibit Timeout
             */
            value = GetParmCfg(INNCOM_CONF_LIST.MA_PARAM_INHIBIT_TIMEOUT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Inhibit Timeout");
                return;
            }
            tUpdown_InhibitTimeout.Value = Utility.getU16FromByteA(value, 2);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
             * Read Inhibit Timeout
             */
            value = GetParmCfg(INNCOM_CONF_LIST.MA_PARAM_INHIBIT_TIMEOUT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Inhibit Timeout");
                return;
            }
            tUpdown_InhibitTimeout.Value = Utility.getU16FromByteA(value, 2);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Device Output Type
             */
            value = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_OUTPUT_DEVICE_TYPE,ref err);
            if(err != ERROR_LIST.ERROR_NONE)
                tUpdown_InhibitCurrent.Value = 0;
            tUpdown_InhibitCurrent.Value = Utility.getF32FromByteA(value, (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ + 3);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read mA Output Parameters");
        }

        private void tBtn_ReadModbus_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                MessageBox.Show("Incom is not connected");
                return;
            }
            /*
             * Read Slave ID
             */
            byte[] value = GetParmCfg(INNCOM_CONF_LIST.MODBUS_PARAM_SLAVE_ID);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Slave ID");
                return;
            }
            tUpdown_SlaveID.Value = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Baudrate
             */
            value = GetParmCfg(INNCOM_CONF_LIST.MODBUS_PARAM_BAUDRATE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Baudrate");
                return;
            }
            tCmb_Baudrate.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Parity 
             */
            value = GetParmCfg(INNCOM_CONF_LIST.MODBUS_PARAM_PARITY);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Parity");
                return;
            }
            tCmb_Parity.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Flow Control
             */
            value = GetParmCfg(INNCOM_CONF_LIST.MODBUS_PARAM_FLOW_CONTROL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Flow Control");
                return;
            }
            tCmb_FlowControl.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Data bits
             */
            value = GetParmCfg(INNCOM_CONF_LIST.MODBUS_PARAM_DATABITS);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Databits");
                return;
            }
            tCmb_Databits.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Stop bits
             */
            value = GetParmCfg(INNCOM_CONF_LIST.MODBUS_PARAM_STOPBITS);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Stopbits");
                return;
            }
            tCmb_Stopbits.SelectedIndex = value[2];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Modbus Settings");
        }

        private void tBtn_ReadNTC_Click(object sender, RoutedEventArgs e)
        {
            //List<Xceed.Wpf.Toolkit.IntegerUpDown> NTCUpDowns = new List<Xceed.Wpf.Toolkit.IntegerUpDown>();
            //for (int i = 0; i < tGrid_NTCCompensation.RowDefinitions.Count; i++)
            //{
            //    Xceed.Wpf.Toolkit.IntegerUpDown temp = tGrid_NTCCompensation.Children.Cast<Xceed.Wpf.Toolkit.IntegerUpDown>().First(t => Grid.GetRow(t) == i && Grid.GetColumn(t) == 1);
            //    NTCUpDowns.Add(temp);
            //}
            //for (int i = 0; i < tGrid_NTCCompensation.RowDefinitions.Count; i++)
            //{
            //    Xceed.Wpf.Toolkit.IntegerUpDown temp = tGrid_NTCCompensation.Children.Cast<Xceed.Wpf.Toolkit.IntegerUpDown>().First(t => Grid.GetRow(t) == i && Grid.GetColumn(t) == 3);
            //    NTCUpDowns.Add(temp);
            //}
        }
    }
}
