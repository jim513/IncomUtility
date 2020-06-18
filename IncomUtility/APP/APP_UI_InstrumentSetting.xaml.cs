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
using Xceed.Wpf.Toolkit;
using MessageBox = System.Windows.MessageBox;

namespace IncomUtility.APP
{
    /// <summary>
    /// APP_UI_InstrumentSetting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_InstrumentSetting : Window
    {

        //SenParamTable paramTable = null;
        //CONFIG_PARAM_TABLE_STRUCT param = null;
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;
        int offset = (int)INNCOM_CONF.SZ_PARAM_INDEX;

        INCOM_DEVICE_CONFIG_STRUCT tIncom_cfg = null;
        CRC16 crc16 = null;
        public APP_UI_InstrumentSetting()
        {
            InitializeComponent();

            //paramTable = new SenParamTable();
            //param = new CONFIG_PARAM_TABLE_STRUCT(0,0,0);
            tIncom_cfg = new INCOM_DEVICE_CONFIG_STRUCT();
            crc16 = new CRC16();
        }

        private byte[] getParmeterConfigruations(INNCOM_CONF_LIST configuration_setting)
        {

            /*
             * Make Payload using Incom Parameter configuration
             */
            byte[] payload = new byte[2];
            payload = Utility.getBytesFromU16((ushort)configuration_setting);

            /*
             * Send Command
             */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CONFIG, payload, ref err, 350);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                return null;
            }

            byte[] parmaValue = QuattroProtocol.getResponseValueData(result);

            return parmaValue;
        }

        private byte[] setParmeterConfigruations<T>(INNCOM_CONF_LIST configuration_setting, T dataToWrite)
        {
            /*
             * Make Payload using Incom Parameter configuration
             */
            byte[] payload = null;
            byte[] dataToArray = null;
            payload = Utility.getBytesFromU16((ushort)configuration_setting);

            if (typeof(T) == typeof(float))
            {
                dataToArray = Utility.getBytesFromF32((float)(object)dataToWrite);
            }
            else if (typeof(T) == typeof(ushort))
            {
                dataToArray = Utility.getBytesFromU16((ushort)(object)dataToWrite);
            }
            else if (typeof(T) == typeof(byte))
            {
                dataToArray = new byte[1];
                dataToArray[0] = (byte)(object)dataToWrite;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                dataToArray = new byte[1];
                dataToArray[0] = (byte)(sbyte)(object)dataToWrite; ;
            }
            else if (typeof(T) == typeof(byte[]))
            {
                dataToArray = (byte[])(object)dataToWrite;
            }
            else
            {
                return null;
            }
            payload = Utility.mergeByteArray(payload, dataToArray);

            int delayTime = (int)Constants.defaultSleep;
            if ((int)configuration_setting >= (int)INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD1)
            {
                if ((int)configuration_setting <= (int)INNCOM_CONF_LIST.SEC_PARAM_OTP_KEY)
                {
                    delayTime = 400;
                }
                if ((int)configuration_setting <= (int)INNCOM_CONF_LIST.GAS_PARAM_DISPLAY_RESOLUTION)
                {
                    delayTime = 1000;
                }
            }
            /*
             * Send Command
             */
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_WRITE_CONFIG, payload, ref err, delayTime);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                return null;
            }

            byte[] parmaValue = QuattroProtocol.getResponseValueData(result);
            return parmaValue;
        }
        private void tBtn_ReadAlarms_Click(object sender, RoutedEventArgs e)
        {
            readAlarm();
        }

        private void tBtn_ReadCal_Click(object sender, RoutedEventArgs e)
        {
            readCal();
        }

        private void tBtn_ReadCircuitCal_Click(object sender, RoutedEventArgs e)
        {
            readCircuitCal();
        }

        private void tBtn_ReadDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            readDeviceInfo();
        }
        private void tBtn_ReadGasSetting_Click(object sender, RoutedEventArgs e)
        {
            readGasSetting();
        }

        private void tBtn_ReadGeneral_Click(object sender, RoutedEventArgs e)
        {
            readGeneral();
        }

        private void tBtn_ReadmAOutput_Click(object sender, RoutedEventArgs e)
        {
            readmAOutput();
        }

        private void tBtn_ReadModbus_Click(object sender, RoutedEventArgs e)
        {
            readModbus();
        }

        private void tBtn_ReadNTC_Click(object sender, RoutedEventArgs e)
        {
            readNTC();
        }

        private void tBtn_ReadRelay_Click(object sender, RoutedEventArgs e)
        {
            readRelay();
        }

        private void tBtn_ReadSecurity_Click(object sender, RoutedEventArgs e)
        {
            readSecurity();
        }

        private void tBtn_ReadUL2075_Click(object sender, RoutedEventArgs e)
        {
            readUL2075();
        }

        private void tBtn_WriteAlarms_Click(object sender, RoutedEventArgs e)
        {
            writeAlarm();
        }

        private void tBtn_WriteCal_Click(object sender, RoutedEventArgs e)
        {
            writeCal();
        }

        private void tBtn_WriteCircuiotCal_Click(object sender, RoutedEventArgs e)
        {
            writeCircuitCal();
        }

        private void tBtn_WriteDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            writeDeviceInfo();
        }

        private void tBtn_WriteGasSetting_Click(object sender, RoutedEventArgs e)
        {
            writeGasSetting();
        }

        private void tBtn_WriteGeneral_Click(object sender, RoutedEventArgs e)
        {
            writeGeneral();
        }

        private void tBtn_WritemAOutput_Click(object sender, RoutedEventArgs e)
        {
            writemAOutput();
        }

        private void tBtn_WriteModbus_Click(object sender, RoutedEventArgs e)
        {
            writeModbus();
        }

        private void tBtn_WriteNTC_Click(object sender, RoutedEventArgs e)
        {
            writeNTC();
        }

        private void tBtn_WriteRelay_Click(object sender, RoutedEventArgs e)
        {
            writeRelay();
        }

        private void tBtn_WriteSecurity_Click(object sender, RoutedEventArgs e)
        {
            writeSecurity();
        }

        private void tBtn_WriteUL2075_Click(object sender, RoutedEventArgs e)
        {
            writeUL2075();
        }

        private void tBtn_GasSettingApply_Click(object sender, RoutedEventArgs e)
        {
            gasSettingApply();
        }

        private void tBtn_Save_Click(object sender, RoutedEventArgs e)
        {
            saveToFile();
        }

        private void tBtn_OpenFiie_Click(object sender, RoutedEventArgs e)
        {
            OpenFromFile();
        }

        private void tBtn_ReadFromIncom_Click(object sender, RoutedEventArgs e)
        {
            readFromIncom();
        }

        private void tBtn_UpdateIncom_Click(object sender, RoutedEventArgs e)
        {
            updateIncom();
        }

        private void readAlarm()
        {
            /*
             * Read Alarm threshold 1
             */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD1);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM THRESHOLD 1");
                return;
            }
            float alarmThresholdvalue = Utility.getF32FromByteA(value, offset);
            tUpdown_AlarmThreshold1.Value = alarmThresholdvalue;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
             * Read Alarm threshold 2
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD2);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM THRESHOLD 2");
                return;
            }
            alarmThresholdvalue = Utility.getF32FromByteA(value, offset);
            tUpdown_AlarmThreshold2.Value = alarmThresholdvalue;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Alarm threshold 3
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD3);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM THRESHOLD 3");
                return;
            }
            alarmThresholdvalue = Utility.getF32FromByteA(value, offset);
            tUpdown_AlarmThreshold3.Value = alarmThresholdvalue;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Alarm Trigger 1
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER1);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Trigger 1");
                return;
            }
            tCmb_AlarmTrigger1.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
            * Read Alarm Trigger 2
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER2);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Trigger 2");
                return;
            }
            tCmb_AlarmTrigger2.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
            * Read Alarm Trigger 3
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER3);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Trigger 3");
                return;
            }
            tCmb_AlarmTrigger3.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Alarm Latching
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.ALARM_PARAM_LATCHING);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read ALARM Latching");
                return;
            }
            tCmb_AlarmLacting.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Gas Alarm Parameters");

        }

        private void readCal()
        {
            /*
            * Read Cal Interval
            */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.CAL_PARAM_CAL_INTERVAL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Calibration Interval");
                return;
            }
            ushort CalInterval = Utility.getU16FromByteA(value, offset);
            tUpdown_CalInterval.Value = CalInterval;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Cal Con
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CAL_PARAM_CAL_CONC);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Calibration Conc.");
                return;
            }
            float CalCon = Utility.getF32FromByteA(value, offset);
            tUpdown_CalCon.Value = CalCon;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
             * Read Last Cal Date
              */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CAL_PARAM_LAST_CAL_DATE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Last Calibration Date");
                return;
            }
            int year = Utility.getU16FromByteA(value, offset);
            byte month = value[offset + 2];
            byte day = value[offset + 3];

            if (Utility.isTimeCheck(year, month, day))
            {
                tDate_LastCalDate.SelectedDate = new DateTime(year, month, day);
                Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            else
            {
                MessageBox.Show("Not Calibrated yet");
                /*
                 *  Do Something When Last Calibration is Wrong
                 *  ex) No Calibrate previous
                 */
            }
            /*
            * Read Days Since Last Cal
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CAL_PARAM_DAYS_SINCE_LAST_CAL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Days Since Last Cal");
                return;
            }
            int days = Utility.getU16FromByteA(value, offset);
            tUpdown_DayPassed.Value = days;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Calibration Parameters");
        }

        private void readCircuitCal()
        {
            /*
            * Read 4-20mA Offset sink
            */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Offset Sink");
                return;
            }
            float _420mAOffsetSink = Utility.getF32FromByteA(value, offset);
            tUpdown_4to20mAOffsetSink.Value = _420mAOffsetSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read 4-20mA Span Sink
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Span Sink");
                return;
            }
            float _420mASpanSink = Utility.getF32FromByteA(value, offset);
            tUpdown_4to20mASpanSink.Value = _420mASpanSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read 4-20mA Offset Source
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Offset Source");
                return;
            }
            float _420mAOffsetSource = Utility.getF32FromByteA(value, offset);
            tUpdown_4to20mAOffsetSource.Value = _420mAOffsetSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read 4-20mA Span Source
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read 4-20mA Span Source");
                return;
            }
            float _420mASpanSource = Utility.getF32FromByteA(value, offset);
            tUpdown_4to20mASpanSource.Value = _420mASpanSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Loop Offset Sink
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read  Loop Offset Sink");
                return;
            }
            float loopOffsetSink = Utility.getF32FromByteA(value, offset);
            tUpdown_LoopbakcOffsetSink.Value = loopOffsetSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
             * Read Loop Span Sink
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SINK);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Loop Span Sink");
                return;
            }
            float loopSpanSink = Utility.getF32FromByteA(value, offset);
            tUpdown_LoopbackSpanSink.Value = loopSpanSink;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
           * Read Loop Offset Source
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read  Loop Offset Source");
                return;
            }
            float loopOffsetSource = Utility.getF32FromByteA(value, offset);
            tUpdown_LoopbackOffsetSource.Value = loopOffsetSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
           * Read Loop Span Source
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SOURCE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Loop Span Source");
                return;
            }
            float loopSpanSource = Utility.getF32FromByteA(value, offset);
            tUpdown_LoopbackSpanSource.Value = loopSpanSource;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
          * Read Voltage Out Offset
          */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_OFFSET);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Voltage Out Offset");
                return;
            }
            float voltagegOutOffset = Utility.getF32FromByteA(value, offset);
            tUpdown_VoltageOutputOffset.Value = voltagegOutOffset;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
          * Read Voltage Out Span
          */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_SPAN);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Voltage Out Span");
                return;
            }
            float voltagegOutSpan = Utility.getF32FromByteA(value, offset);
            tUpdown_VoltageOutputSpan.Value = voltagegOutSpan;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Circuit Calibration Parameters");

        }

        private void readDeviceInfo()
        {
            /*
             * Read Device Serial Number
             */

            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.DEV_INFO_PARAM_DEVICE_SN);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Device Serial Number");
                return;
            }

            string str = Encoding.Default.GetString(value, 2, value.Length - 2).Trim('\0');
            tTxt_DeviceSerialNumber.Text = str;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             *  Write Board Serial Number
             */

            value = getParmeterConfigruations(INNCOM_CONF_LIST.DEV_INFO_PARAM_BOARD_SN);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Board Serial Number");
                return;
            }

            str = Encoding.Default.GetString(value, 2, value.Length - 2).Trim('\0');
            tTxt_BoardSerialNumber.Text = str;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Device Info");
        }

        private void readGasSetting()
        {
            /*
            * Read Gas Name
            */

            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_USER_GAS_NAME);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read User Gas Name");
                return;
            }
            string str = Encoding.Default.GetString(value, 2, value.Length - 2).Trim('\0');
            tTxt_GasName.Text = str;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Unit Conversion
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_UNIT_CONVERSION);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Gas Unit Conversion");
                return;
            }
            tUpdown_UnitConversion.Value = Utility.getF32FromByteA(value, offset);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Correction Factor
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_CORRECTION_FACTOR);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Correction Factor");
                return;
            }
            tUpdown_CorrectionFactor.Value = Utility.getF32FromByteA(value, offset);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Measurement Unit
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_MEASUREMENT_UNITS);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Measurment Unit");
                return;
            }
            tCmb_MeasurementUnit.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
           * Read Measuring Range
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_MEASURING_RANGE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Measuring Range");
                return;
            }
            tUpdown_MeasuringRange.Value = Utility.getF32FromByteA(value, offset);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
            * Read Deadband
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_DEADBAND);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Deadband");
                return;
            }
            tCmb_Deadband.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Gas Type
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_GAS_TYPE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Gas Type");
                return;
            }
            tCmb_GasType.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
            * Read Target Channel
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_TARGET_CHANNEL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Target Channel");
                return;
            }
            tUpdown_TargetChannel.Value = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
             * Read Decimal Points
              */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_DECIMAL_POINT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Decimal Points");
                return;
            }
            tCmb_DecimalPoints.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
            * Read Display Resolution
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_DISPLAY_RESOLUTION);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Display Resolution");
                return;
            }
            tUpdown_DisplayResoultion.Value = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Gas Parameters");
        }

        private void readGeneral()
        {
            /*
            * Read Parameters Location Tag
            */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_LOCATION_TAG);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Parameters Location Tag");
                return;
            }
            string str = Encoding.Default.GetString(value, 2, value.Length - 2).Trim('\0');
            tTxt_LocationTag.Text = str;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read LED Control
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_LED_CONTROL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read LED Control");
                return;
            }
            tCmb_LEDControl.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Alarm Operation Mode
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_ALARM_OP_MODE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Alarm Operation Mode");
                return;
            }
            tCmb_AlarmOperationMode.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Safety Mode
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_OP_MODE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Saftey Mode");
                return;
            }
            tCmb_SafetyMode.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Calibration Overdue
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_CAL_OVERDUE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Calibration Overdue");
                return;
            }
            tCmb_CalOverdue.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
           * Read Passcode
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_PASSCODE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Passcode");
                return;
            }
            tUpdown_Passcode1.Value = value[offset];
            tUpdown_Passcode2.Value = value[offset + 1];
            tUpdown_Passcode3.Value = value[offset + 2];
            tUpdown_Passcode4.Value = value[offset + 3];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read General Parameters");
        }

        private void readmAOutput()
        {
            /*
             * Read mA Fault Current
             */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_FAULT_CURRENT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read mA Fault Current");
                return;
            }
            tUpdown_FaultCurrent.Value = value[offset] / 10;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
           * Read mA Warning Current
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_WARNING_CURRENT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read mA Warning Current");
                return;
            }
            tUpdown_WarningCurrent.Value = value[offset] / 10;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
           * Read mA Over Range Current
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_OVERRANGE_CURRENT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read mA Over Range Current");
                return;
            }
            tUpdown_OverRangeCurrent.Value = value[offset] / 10;
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Inhibit Timeout
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_INHIBIT_TIMEOUT);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Inhibit Timeout");
                return;
            }
            tUpdown_InhibitTimeout.Value = Utility.getU16FromByteA(value, offset);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
             * Read Device Output Type
             */
            value = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_OUTPUT_DEVICE_TYPE, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
                tUpdown_InhibitCurrent.Value = 0;
            tUpdown_InhibitCurrent.Value = Utility.getF32FromByteA(value, (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ + 3);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read mA Output Parameters");
        }

        private void readModbus()
        {
            /*
           * Read Slave ID
           */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_SLAVE_ID);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Slave ID");
                return;
            }
            tUpdown_SlaveID.Value = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Baudrate
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_BAUDRATE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Baudrate");
                return;
            }
            tCmb_Baudrate.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Parity 
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_PARITY);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Parity");
                return;
            }
            tCmb_Parity.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Flow Control
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_FLOW_CONTROL);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Flow Control");
                return;
            }
            tCmb_FlowControl.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Data bits
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_DATABITS);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Databits");
                return;
            }
            tCmb_Databits.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Stop bits
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_STOPBITS);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Stopbits");
                return;
            }
            tCmb_Stopbits.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Modbus Settings");
        }

        private void readNTC()
        {
            /*
             * Read NTC
             */
            byte[] value = null;
            for (int i = 0; i < (int)INNCOM_CONF.NUM_NTC_COMP; i++)
            {
                value = getParmeterConfigruations(INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP1 + i);
                if (value == null)
                {
                    MessageBox.Show("ERROR - Read NTC " + i + 1);
                    return;
                }
                IntegerUpDown NTCUpDown = (IntegerUpDown)this.FindName(String.Format("tUpdown_NTCComp{0}", i + 1));
                NTCUpDown.Value = (sbyte)value[offset];
                Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            MessageBox.Show("Read NTC Parameters");

        }

        private void readRelay()
        {
            /*
             * Read Relay Trigger Event1
             */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_TRIGGER1);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Relay Trigger Event1");
                return;
            }
            tCmb_TriggerEvent1.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
             * Read Relay Trigger Event2
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_TRIGGER2);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Relay Trigger Event2");
                return;
            }
            tCmb_TriggerEvent2.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Relay Initial State1
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_INIT_STATE1);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Relay Initial State1");
                return;
            }
            tCmb_InitialState1.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Relay Initial State2
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_INIT_STATE2);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Relay Initial State2");
                return;
            }
            tCmb_InitialState2.SelectedIndex = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Read Relay On Delay Time
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_ON_DELAY_TIME);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Relay On Delay Time");
                return;
            }
            tUpdown_OnDelayTime.Value = Utility.getU16FromByteA(value, offset);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
           * Read Relay Off Delay Time
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_OFF_DELAY_TIME);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Relay Off Delay Time");
                return;
            }
            tUpdown_OffDelayTime.Value = Utility.getU16FromByteA(value, offset);
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Relay Settings");

        }

        private void readSecurity()
        {
            /*
             * Read Login Retry Count
             */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_NUM_RETRY);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Login Retry Count");
                return;
            }
            tUpdown_LoginRetry.Value = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
             * Read Login Lock Time
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_LOGIN_LOCK_TIME);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read Login Lock Time");
                return;
            }
            tUpdown_LoginLockTime.Value = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
             * Read OTP Connection
             */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_OTP_CONNECTION);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read OTP Connection");
                return;
            }
            tUpdown_OTPConnection.Value = value[offset];
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            /*
           * Read OTP Key
           */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_OTP_KEY);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read OTP Key");
                return;
            }
            tTxt_OTPKey.Text = Encoding.Default.GetString(value, 2, value.Length - 2).Trim('\0');
            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            MessageBox.Show("Read Security Parameters");
        }

        private void readUL2075()
        {
            /*
            * Read UL2075
            */
            byte[] value = null;
            for (int i = 0; i < (int)INNCOM_CONF.NUM_HISTOGRAM; i++)
            {
                value = getParmeterConfigruations(INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD1 + i);
                if (value == null)
                {
                    MessageBox.Show("ERROR - Read UL2075 : " + i + 1);
                    return;
                }
                DecimalUpDown UL2075 = (DecimalUpDown)this.FindName(String.Format("tUpdown_UL2075Count{0}", i + 1));
                UL2075.Value = (byte)value[offset];
                Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            MessageBox.Show("Read UL2075 Parameters");
        }

        private void writeAlarm()
        {
            /*
             * Check Measuring Range
             */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_MEASURING_RANGE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Get Measuring Range");
                return;
            }
            float measuringRange = Utility.getF32FromByteA(value, offset);

            if (measuringRange < tUpdown_AlarmThreshold1.Value)
            {
                MessageBox.Show("Gas alarm threshold  calibration concentration should not exceed measuring range!!");
                return;
            }
            if (measuringRange < tUpdown_AlarmThreshold2.Value)
            {
                MessageBox.Show("Gas alarm threshold  calibration concentration should not exceed measuring range!!");
                return;
            }
            if (measuringRange < tUpdown_AlarmThreshold3.Value)
            {
                MessageBox.Show("Gas alarm threshold  calibration concentration should not exceed measuring range!!");
                return;
            }

            /*
             *  Checking Alarm Range
             */
            if (tCmb_AlarmTrigger1.SelectedIndex == 0)
            {
                if (tCmb_AlarmTrigger2.SelectedIndex == 0)
                {
                    if (tUpdown_AlarmThreshold1.Value > tUpdown_AlarmThreshold2.Value)
                    {
                        MessageBox.Show("Gas alarm 1 should not exceed gas alarm 2!");
                        return; ;
                    }

                    if (tCmb_AlarmTrigger3.SelectedIndex == 0)
                    {
                        if (tUpdown_AlarmThreshold2.Value > tUpdown_AlarmThreshold3.Value)
                        {
                            MessageBox.Show("Gas alarm 2 should not exceed gas alarm 3!");
                            return; ;
                        }

                    }
                }
            }
            /*
             * Write Alarm Configuration
             */
            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD1, (float)tUpdown_AlarmThreshold1.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Threshold1");
                return;
            }
            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD2, (float)tUpdown_AlarmThreshold2.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Threshold2");
                return;
            }
            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD3, (float)tUpdown_AlarmThreshold3.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Threshold3");
                return;
            }
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER1, (byte)tCmb_AlarmTrigger1.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Trigger1");
                return;
            }
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER2, (byte)tCmb_AlarmTrigger2.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Trigger2");
                return;
            }
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER3, (byte)tCmb_AlarmTrigger3.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Trigger3");
                return;
            }
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.ALARM_PARAM_LATCHING, (byte)tCmb_AlarmLacting.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Lacting");
                return;
            }


            MessageBox.Show("Write Gas Alarm Parameters");
        }

        private void writeCal()
        {
            /*
             * Check Measuring Range
             */
            byte[] value = getParmeterConfigruations(INNCOM_CONF_LIST.GAS_PARAM_MEASURING_RANGE);
            if (value == null)
            {
                MessageBox.Show("ERROR - Get Measuring Range");
                return;
            }
            float measuringRange = Utility.getF32FromByteA(value, offset);

            if (measuringRange < tUpdown_CalCon.Value)
            {
                MessageBox.Show("Target calibration concentration should not exceed measuring range!!");
                return;
            }

            /*
            * Write Calibration Interval
            */
            value = setParmeterConfigruations<ushort>(INNCOM_CONF_LIST.CAL_PARAM_CAL_INTERVAL, (ushort)tUpdown_CalInterval.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Calibration Interval");
                return;
            }


            /*
            * Write Calibration Conc
            */
            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CAL_PARAM_CAL_CONC, (float)tUpdown_CalCon.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Calibration Conc.");
                return;
            }

            if ((bool)tChb_UpdateCal.IsChecked)
            {

                /*
                * Write Last Calibration Data
                */
                byte[] year = Utility.getBytesFromU16((ushort)tDate_LastCalDate.SelectedDate.Value.Year);
                byte[] payload = new byte[2];
                payload[0] = (byte)tDate_LastCalDate.SelectedDate.Value.Month;
                payload[1] = (byte)tDate_LastCalDate.SelectedDate.Value.Day;
                payload = Utility.mergeByteArray(year, payload);

                value = setParmeterConfigruations<byte[]>(INNCOM_CONF_LIST.CAL_PARAM_LAST_CAL_DATE, payload);
                if (value == null)
                {
                    MessageBox.Show("ERROR - Write Last Calibration Date.");
                    return;
                }

                /*
                * Write Passed Day Since Last Calibration
                */
                value = setParmeterConfigruations<ushort>(INNCOM_CONF_LIST.CAL_PARAM_DAYS_SINCE_LAST_CAL, (ushort)tUpdown_DayPassed.Value);
                if (value == null)
                {
                    MessageBox.Show("ERROR - Write Days Since Last Calibartion.");
                    return;
                }
            }

            MessageBox.Show("Write Calibration Parameters");
        }

        private void writeCircuitCal()
        {
            /*
            * Write Circuit Calibration
            */
            byte[] value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SINK, (float)tUpdown_4to20mAOffsetSink.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Offset Sink");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SOURCE, (float)tUpdown_4to20mAOffsetSource.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Offset Source");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SINK, (float)tUpdown_4to20mASpanSink.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Span Sink");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SOURCE, (float)tUpdown_4to20mASpanSource.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Span Source");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SINK, (float)tUpdown_LoopbakcOffsetSink.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Loop Offest Sink");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SOURCE, (float)tUpdown_LoopbackOffsetSource.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Loop Offest Source");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SINK, (float)tUpdown_LoopbackSpanSink.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Loop Span Sink");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SOURCE, (float)tUpdown_LoopbackSpanSource.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write 4-20mA Loop Span Source");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_OFFSET, (float)tUpdown_VoltageOutputOffset.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Voltage Output Offset");
                return;
            }

            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_SPAN, (float)tUpdown_VoltageOutputSpan.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Voltage Output Span");
                return;
            }

            MessageBox.Show("Write Circuit Calibration Parameters");
        }

        private void writeDeviceInfo()
        {
            /*
           * Write Device Serial Number
           */
            string str = tTxt_DeviceSerialNumber.Text;

            int str_len = str.Length;
            if (str_len > (int)INNCOM_CONF.NUM_DEVICE_SN)
            {
                str_len = (int)INNCOM_CONF.NUM_DEVICE_SN;
            }

            byte[] sendText = new byte[(int)INNCOM_CONF.NUM_DEVICE_SN];
            Encoding.ASCII.GetBytes(str, 0, str_len, sendText, 0);

            byte[] value = setParmeterConfigruations<byte[]>(INNCOM_CONF_LIST.DEV_INFO_PARAM_DEVICE_SN, sendText);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Device Serial Number");
                return;
            }


            /*
             *  Write Board Serial Number
             */
            str = tTxt_BoardSerialNumber.Text;

            str_len = str.Length;
            if (str_len > (int)INNCOM_CONF.NUM_DEVICE_SN)
            {
                str_len = (int)INNCOM_CONF.NUM_DEVICE_SN;
            }

            sendText = new byte[(int)INNCOM_CONF.NUM_DEVICE_SN];
            Encoding.ASCII.GetBytes(str, 0, str_len, sendText, 0);

            value = setParmeterConfigruations<byte[]>(INNCOM_CONF_LIST.DEV_INFO_PARAM_BOARD_SN, sendText);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Board Serial Number");
                return;
            }

            MessageBox.Show("Write Device Info");
        }

        private void writeGasSetting()
        {
            /*
             * Write Gas Name
             */
            string str = tTxt_GasName.Text;

            int str_len = str.Length;
            if (str_len > (int)INNCOM_CONF.NUM_GAS_NAME)
            {
                str_len = (int)INNCOM_CONF.NUM_GAS_NAME;
            }

            byte[] sendText = new byte[(int)INNCOM_CONF.NUM_GAS_NAME];
            Encoding.ASCII.GetBytes(str, 0, str_len, sendText, 0);

            byte[] value = setParmeterConfigruations<byte[]>(INNCOM_CONF_LIST.GAS_PARAM_USER_GAS_NAME, sendText);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write User Gas Name");
                return;
            }

            /*
           * Write Unit Conversion
           */
            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.GAS_PARAM_UNIT_CONVERSION, (float)tUpdown_UnitConversion.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Gas Unit Conversion");
                return;
            }

            /*
           * Write Correction Factor
           */
            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.GAS_PARAM_CORRECTION_FACTOR, (float)tUpdown_CorrectionFactor.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Correction Factor");
                return;
            }

            /*
           * Write Measurement Unit
           */
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.GAS_PARAM_MEASUREMENT_UNITS, (byte)tCmb_MeasurementUnit.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Measurment Unit");
                return;
            }

            /*
           * Write Measuring Range
           */
            value = setParmeterConfigruations<float>(INNCOM_CONF_LIST.GAS_PARAM_MEASURING_RANGE, (float)tUpdown_MeasuringRange.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Measuring Range");
                return;
            }
            /*
            * Write Deadband
             */
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.GAS_PARAM_DEADBAND, (byte)tCmb_Deadband.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Deadband");
                return;
            }
            /*
             * Write Gas Type
            */
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.GAS_PARAM_GAS_TYPE, (byte)tCmb_GasType.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Gas Type");
                return;
            }
            /*
            * Write Target Channel
             */
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.GAS_PARAM_TARGET_CHANNEL, (byte)tUpdown_TargetChannel.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Target Channel");
                return;
            }
            /*
             * Write Decimal Points
              */
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.GAS_PARAM_DECIMAL_POINT, (byte)tCmb_DecimalPoints.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Decimal Points");
                return;
            }
            /*
            * Write Display Resolution
             */
            value = setParmeterConfigruations<byte>(INNCOM_CONF_LIST.GAS_PARAM_DISPLAY_RESOLUTION, (byte)tUpdown_DisplayResoultion.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Display Resolution");
                return;
            }

            MessageBox.Show("Write Gas Parameters");
        }

        private void writeGeneral()
        {
            /*
           * Write Location Tag
           */
            string str = tTxt_LocationTag.Text;

            int str_len = str.Length;
            if (str_len > (int)INNCOM_CONF.NUM_LOCATION_TAG)
            {
                str_len = (int)INNCOM_CONF.NUM_LOCATION_TAG;
            }

            byte[] sendText = new byte[(int)INNCOM_CONF.NUM_LOCATION_TAG];
            Encoding.ASCII.GetBytes(str, 0, str_len, sendText, 0);

            byte[] value = setParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_LOCATION_TAG, sendText);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Location Tag");
                return;
            }

            /*
             * Write LED Control
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_LED_CONTROL, (byte)tCmb_LEDControl.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write LED Control");
                return;
            }

            /*
            * Write Alarm Operation Mode
            */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_ALARM_OP_MODE, (byte)tCmb_AlarmOperationMode.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Alarm Operation Mode");
                return;
            }
            /*
             * Write Safety Mode
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_OP_MODE, (byte)tCmb_SafetyMode.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Saftey Mode");
                return;
            }

            /*
             * Write Calibration Overdue
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_CAL_OVERDUE, (byte)tCmb_CalOverdue.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Calibration Overdue");
                return;
            }
            /*
           * Write Passcode
           */
            int passcodeSize = 4;
            byte[] passcode = new byte[passcodeSize];

            passcode[0] = (byte)tUpdown_Passcode1.Value;
            passcode[1] = (byte)tUpdown_Passcode2.Value;
            passcode[2] = (byte)tUpdown_Passcode3.Value;
            passcode[3] = (byte)tUpdown_Passcode4.Value;
            value = setParmeterConfigruations(INNCOM_CONF_LIST.GENERAL_PARAM_PASSCODE, passcode);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Passcode");
                return;
            }

            MessageBox.Show("Write General Parameters");
        }

        private void writemAOutput()
        {
            /*
            * Write mA Fault Current
            */
            byte[] value = setParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_FAULT_CURRENT, (byte)(tUpdown_FaultCurrent.Value * 10));
            if (value == null)
            {
                MessageBox.Show("ERROR - Write mA Fault Current");
                return;
            }

            /*
             * Write mA Warning Current
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_WARNING_CURRENT, (byte)(tUpdown_WarningCurrent.Value * 10));
            if (value == null)
            {
                MessageBox.Show("ERROR - Write mA Warning Current");
                return;
            }

            /*
            * Write mA Over Range Current
            */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_OVERRANGE_CURRENT, (byte)(tUpdown_OverRangeCurrent.Value * 10));
            if (value == null)
            {
                MessageBox.Show("ERROR - Write mA Over Range Current");
                return;
            }
            /*
             * Write Inhibit Timeout
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MA_PARAM_INHIBIT_TIMEOUT, (ushort)tUpdown_InhibitTimeout.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Inhibit Timeout");
                return;
            }

            MessageBox.Show("Write mA Output Parameters");
        }

        private void writeModbus()
        {
            /*
            * Write Slave ID
            */
            byte[] value = setParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_SLAVE_ID, (byte)tUpdown_SlaveID.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Slave ID");
                return;
            }

            /*
             * Write Baudrate
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_BAUDRATE, (byte)tCmb_Baudrate.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Baudrate");
                return;
            }

            /*
             * Write Parity 
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_PARITY, (byte)tCmb_Parity.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Parity");
                return;
            }

            /*
             * Write Flow Control
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_FLOW_CONTROL, (byte)tCmb_FlowControl.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Flow Control");
                return;
            }

            /*
             * Write Data bits
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_DATABITS, (byte)tCmb_Databits.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Databits");
                return;
            }

            /*
             * Write Stop bits
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.MODBUS_PARAM_STOPBITS, (byte)tCmb_Stopbits.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Stopbits");
                return;
            }

            MessageBox.Show("Write Modbus Settings");
        }

        private void writeNTC()
        {
            /*
            * Write NTC
            */
            byte[] value = null;
            for (int i = 0; i < (int)INNCOM_CONF.NUM_NTC_COMP; i++)
            {
                IntegerUpDown NTCUpDown = (IntegerUpDown)this.FindName(String.Format("tUpdown_NTCComp{0}", i + 1));
                value = setParmeterConfigruations(INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP1 + i, (sbyte)NTCUpDown.Value);
                if (value == null)
                {
                    MessageBox.Show("ERROR - Write NTC " + i + 1);
                    return;
                }
            }
            MessageBox.Show("Write NTC Parameters");

        }

        private void writeRelay()
        {
            /*
             * Write Relay Trigger Event1
             */
            byte[] value = setParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_TRIGGER1, (byte)tCmb_TriggerEvent1.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Relay Trigger Event1");
                return;
            }
            /*
            * Write Relay Trigger Event2
            */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_TRIGGER2, (byte)tCmb_TriggerEvent2.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Relay Trigger Event2");
                return;
            }

            /*
             * Write Relay Initial State1
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_INIT_STATE1, (byte)tCmb_InitialState1.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Relay Initial State1");
                return;
            }

            /*
             * Write Relay Initial State2
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_INIT_STATE2, (byte)tCmb_InitialState2.SelectedIndex);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Relay Initial State2");
                return;
            }

            /*
             * Write Relay On Delay Time
             */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_ON_DELAY_TIME, (ushort)tUpdown_OnDelayTime.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Relay On Delay Time");
                return;
            }
            /*
    * Write Relay Off Delay Time
    */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.RELAY_PARAM_OFF_DELAY_TIME, (ushort)tUpdown_OffDelayTime.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Relay Off Delay Time");
                return;
            }

            MessageBox.Show("Write Relay Settings");

        }

        private void writeSecurity()
        {
            /*
             * Write Login Retry Count
             */
            byte[] value = setParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_NUM_RETRY, (byte)tUpdown_LoginRetry.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Login Retry Count");
                return;
            }
            /*
            * Write Login Lock Time
            */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_LOGIN_LOCK_TIME, (byte)tUpdown_LoginLockTime.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write Login Lock Time");
                return;
            }
            /*
            * Write OTP Connection
            */
            value = setParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_OTP_CONNECTION, (byte)tUpdown_OTPConnection.Value);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write OTP Connection");
                return;
            }
            /*
            * Read OTP Key
            */
            value = getParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_OTP_KEY);
            if (value == null)
            {
                MessageBox.Show("ERROR - Read OTP Key");
                return;
            }

            /*
            * Write OTP Key
             */

            byte[] OTPKey = new byte[value.Length - 2];
            Array.Copy(value, OTPKey, value.Length - 2);
            value = setParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_OTP_KEY, OTPKey);
            if (value == null)
            {
                MessageBox.Show("ERROR - Write OTP Key");
                return;
            }

            MessageBox.Show("Write Security Parameters");
        }

        private void writeUL2075()
        {
            /*
          * Write UL2075
          */
            byte[] value = null;
            for (int i = 0; i < (int)INNCOM_CONF.NUM_HISTOGRAM; i++)
            {
                DecimalUpDown UL2075 = (DecimalUpDown)this.FindName(String.Format("tUpdown_UL2075Count{0}", i + 1));
                value = setParmeterConfigruations(INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD1 + i, (byte)UL2075.Value);
                if (value == null)
                {
                    MessageBox.Show("ERROR - Write UL2075 : " + i + 1);
                    return;
                }
            }
            MessageBox.Show("Write UL2075 Parameters");
        }

        private void gasSettingApply()
        {

            tUpdown_CalCon.Value = tUpdown_CalCon.Value * tUpdown_UnitConversion.Value;
            tUpdown_AlarmThreshold1.Value = tUpdown_AlarmThreshold1.Value * tUpdown_UnitConversion.Value;
            tUpdown_AlarmThreshold2.Value = tUpdown_AlarmThreshold2.Value * tUpdown_UnitConversion.Value;
            tUpdown_AlarmThreshold3.Value = tUpdown_AlarmThreshold3.Value * tUpdown_UnitConversion.Value;
            tUpdown_MeasuringRange.Value = tUpdown_MeasuringRange.Value * tUpdown_UnitConversion.Value;
        }

        private void getIncomConfigurationFromWindow()
        {
            byte[] classToBytes = null;
            int crc16_sz = 2;

            /*
             * Get Modbus Parameters 
            */
            tIncom_cfg.tModbusCfg.u8_SlaveId = (byte)tUpdown_SlaveID.Value;
            tIncom_cfg.tModbusCfg.u8_Parity = (byte)tCmb_Parity.SelectedIndex;
            tIncom_cfg.tModbusCfg.u8_Baudrate = (byte)tCmb_Baudrate.SelectedIndex;
            tIncom_cfg.tModbusCfg.u8_FlowCtrl = (byte)tCmb_FlowControl.SelectedIndex;
            tIncom_cfg.tModbusCfg.u8_Databits = (byte)tCmb_Databits.SelectedIndex;
            tIncom_cfg.tModbusCfg.u8_Stopbits = (byte)tCmb_Stopbits.SelectedIndex;

            classToBytes = tIncom_cfg.tModbusCfg.Serialize();
            tIncom_cfg.tModbusCfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            /*
             * Get Relay Parameters 
             */
            tIncom_cfg.tRelayCfg.u8_TriggerEvent1 = (byte)tCmb_TriggerEvent1.SelectedIndex;
            tIncom_cfg.tRelayCfg.u8_TriggerEvent2 = (byte)tCmb_TriggerEvent2.SelectedIndex;
            tIncom_cfg.tRelayCfg.u8_InitialState1 = (byte)tCmb_InitialState1.SelectedIndex;
            tIncom_cfg.tRelayCfg.u8_InitialState2 = (byte)tCmb_InitialState2.SelectedIndex;

            tIncom_cfg.tRelayCfg.u16_OnDelayTime = (ushort)tUpdown_OnDelayTime.Value;
            tIncom_cfg.tRelayCfg.u16_OffDelayTime = (ushort)tUpdown_OffDelayTime.Value;

            classToBytes = tIncom_cfg.tRelayCfg.Serialize();
            tIncom_cfg.tRelayCfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);


            /*
             *Get mA Output Parameters 
             */
            tIncom_cfg.tmAOutputCfg.u8_FaultCurrent = (byte)(tUpdown_FaultCurrent.Value * 10);
            tIncom_cfg.tmAOutputCfg.u8_WarningCurrent = (byte)(tUpdown_WarningCurrent.Value * 10);
            tIncom_cfg.tmAOutputCfg.u8_OverrangeCurrent = (byte)(tUpdown_OverRangeCurrent.Value * 10);
            tIncom_cfg.tmAOutputCfg.u16_InhibitTimeout = (ushort)tUpdown_InhibitTimeout.Value;

            classToBytes = tIncom_cfg.tmAOutputCfg.Serialize();
            tIncom_cfg.tmAOutputCfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            /*
             *Get General Parameters 
             */
            string str = tTxt_LocationTag.Text;

            int str_len = str.Length;
            if (str_len > (int)INNCOM_CONF.NUM_LOCATION_TAG)
            {
                str_len = (int)INNCOM_CONF.NUM_LOCATION_TAG;
            }
            Encoding.ASCII.GetBytes(str, 0, str_len, tIncom_cfg.tGeneralCfg.u8_LocationTag, 0);

            tIncom_cfg.tGeneralCfg.u8_LEDCtrl = (byte)tCmb_LEDControl.SelectedIndex;
            tIncom_cfg.tGeneralCfg.u8_AlarmMode = (byte)tCmb_AlarmOperationMode.SelectedIndex;
            tIncom_cfg.tGeneralCfg.u8_SafeMode = (byte)tCmb_SafetyMode.SelectedIndex;
            tIncom_cfg.tGeneralCfg.u8_CalOverDueOption = (byte)tCmb_CalOverdue.SelectedIndex;

            tIncom_cfg.tGeneralCfg.u8_passcode[0] = (byte)tUpdown_Passcode1.Value;
            tIncom_cfg.tGeneralCfg.u8_passcode[1] = (byte)tUpdown_Passcode2.Value;
            tIncom_cfg.tGeneralCfg.u8_passcode[2] = (byte)tUpdown_Passcode3.Value;
            tIncom_cfg.tGeneralCfg.u8_passcode[3] = (byte)tUpdown_Passcode4.Value;

            classToBytes = tIncom_cfg.tGeneralCfg.Serialize();
            tIncom_cfg.tGeneralCfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);


            /* 
             * Get Circuit Calibration Parameters 
             */

            tIncom_cfg.tCircuitCalCfg.f32_mAOutputSinkOffset = (float)tUpdown_4to20mAOffsetSink.Value;
            tIncom_cfg.tCircuitCalCfg.f32_mAOutputSinkSpan = (float)tUpdown_4to20mASpanSink.Value;
            tIncom_cfg.tCircuitCalCfg.f32_mAOutputSourceOffset = (float)tUpdown_4to20mAOffsetSource.Value;
            tIncom_cfg.tCircuitCalCfg.f32_mAOutputSourceSpan = (float)tUpdown_4to20mASpanSource.Value;
            tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSinkOffset = (float)tUpdown_LoopbakcOffsetSink.Value;
            tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSinkSpan = (float)tUpdown_LoopbackSpanSink.Value;
            tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSourceOffset = (float)tUpdown_LoopbackOffsetSource.Value;
            tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSourceSpan = (float)tUpdown_LoopbackSpanSource.Value;
            tIncom_cfg.tCircuitCalCfg.f32_VoltageOutputOffset = (float)tUpdown_VoltageOutputOffset.Value;
            tIncom_cfg.tCircuitCalCfg.f32_VoltageOutputSpan = (float)tUpdown_VoltageOutputSpan.Value;

            for (int i = 0; i < 26; i++)
            {
                tIncom_cfg.u8Reserved[i] = 0xff;
            }

            classToBytes = tIncom_cfg.tCircuitCalCfg.Serialize();
            tIncom_cfg.tCircuitCalCfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);
            /*
             * Get Device Info Parameters 
             */
            str = tTxt_DeviceSerialNumber.Text;

            str_len = str.Length;
            if (str_len > (int)INNCOM_CONF.NUM_DEVICE_SN)
            {
                str_len = (int)INNCOM_CONF.NUM_DEVICE_SN;
            }
            Encoding.ASCII.GetBytes(str, 0, str_len, tIncom_cfg.tDeviceInfo.u8_DeviceSerialNum, 0);

            str = tTxt_BoardSerialNumber.Text;

            str_len = str.Length;
            if (str_len > (int)INNCOM_CONF.NUM_BOARD_SN)
            {
                str_len = (int)INNCOM_CONF.NUM_BOARD_SN;
            }
            Encoding.ASCII.GetBytes(str, 0, str_len, tIncom_cfg.tDeviceInfo.u8_BoardSerialNum, 0);

            classToBytes = tIncom_cfg.tDeviceInfo.Serialize();
            tIncom_cfg.tDeviceInfo.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);


            /*
             * Get Security Parameters
             */
            tIncom_cfg.tSecurityCfg.u8_num_login_retry = (byte)tUpdown_LoginRetry.Value;
            tIncom_cfg.tSecurityCfg.u8_login_locktime = (byte)tUpdown_LoginLockTime.Value;

            tIncom_cfg.tSecurityCfg.u8_otp_limits = (byte)tUpdown_OTPConnection.Value;
            tIncom_cfg.tSecurityCfg.u8_reserved = 0;

            classToBytes = getParmeterConfigruations(INNCOM_CONF_LIST.SEC_PARAM_OTP_KEY);

            if (classToBytes != null)
            {
                tTxt_OTPKey.Text = Encoding.Default.GetString(classToBytes, 2, classToBytes.Length - 2).Trim('\0');
                Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                str = tTxt_OTPKey.Text;

                str_len = str.Length;
                if (str_len > (int)INNCOM_CONF.NUM_OTP_KEY)
                {
                    str_len = (int)INNCOM_CONF.NUM_OTP_KEY;
                }
                Encoding.ASCII.GetBytes(str, 0, str_len, tIncom_cfg.tSecurityCfg.u8_otp_key, 0);
            }
            else
            {
                MessageBox.Show("ERROR - OTP KEy ");
                return;
            }
            classToBytes = tIncom_cfg.tSecurityCfg.Serialize();
            tIncom_cfg.tSecurityCfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            /* 
             * Get NTC Compensation Table 
             */
            for (int i = 0; i < (int)INNCOM_CONF.NUM_NTC_COMP; i++)
            {
                IntegerUpDown NTCUpDown = (IntegerUpDown)this.FindName(String.Format("tUpdown_NTCComp{0}", i + 1));
                tIncom_cfg.tNtcCfg.s8_NtcTempComp[i] = (sbyte)NTCUpDown.Value;
            }
            classToBytes = tIncom_cfg.tNtcCfg.Serialize();
            tIncom_cfg.tNtcCfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            /*
             * Get UL2075 Histogram Table 
             */
            for (int i = 0; i < (int)INNCOM_CONF.NUM_HISTOGRAM; i++)
            {
                DecimalUpDown UL2075 = (DecimalUpDown)this.FindName(String.Format("tUpdown_UL2075Count{0}", i + 1));
                tIncom_cfg.tUL2075Cfg.u8_Histogram[i] = (byte)UL2075.Value;
            }
            classToBytes = tIncom_cfg.tUL2075Cfg.Serialize();
            tIncom_cfg.tUL2075Cfg.u16_crc = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

        }

        private void setIncomConfigurationToWindow()
        {
            /*
             * Set Modbus
             */

            tUpdown_SlaveID.Value = tIncom_cfg.tModbusCfg.u8_SlaveId;
            tCmb_Parity.SelectedIndex = tIncom_cfg.tModbusCfg.u8_Parity;
            tCmb_Baudrate.SelectedIndex = tIncom_cfg.tModbusCfg.u8_Baudrate;
            tCmb_FlowControl.SelectedIndex = tIncom_cfg.tModbusCfg.u8_FlowCtrl;
            tCmb_Databits.SelectedIndex = tIncom_cfg.tModbusCfg.u8_Databits;
            tCmb_Stopbits.SelectedIndex = tIncom_cfg.tModbusCfg.u8_Stopbits;

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             *set Relay Parameters
             */
            tCmb_TriggerEvent1.SelectedIndex = tIncom_cfg.tRelayCfg.u8_TriggerEvent1;
            tCmb_TriggerEvent2.SelectedIndex = tIncom_cfg.tRelayCfg.u8_TriggerEvent2;
            tCmb_InitialState1.SelectedIndex = tIncom_cfg.tRelayCfg.u8_InitialState1;
            tCmb_InitialState2.SelectedIndex = tIncom_cfg.tRelayCfg.u8_InitialState2;

            tUpdown_OnDelayTime.Value = tIncom_cfg.tRelayCfg.u16_OnDelayTime;
            tUpdown_OffDelayTime.Value = tIncom_cfg.tRelayCfg.u16_OffDelayTime;

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             *Set mA Output Parameters 
             */
            tUpdown_FaultCurrent.Value = tIncom_cfg.tmAOutputCfg.u8_FaultCurrent / 10;
            tUpdown_WarningCurrent.Value = tIncom_cfg.tmAOutputCfg.u8_WarningCurrent / 10;
            tUpdown_OverRangeCurrent.Value = tIncom_cfg.tmAOutputCfg.u8_OverrangeCurrent / 10;
            tUpdown_InhibitTimeout.Value = tIncom_cfg.tmAOutputCfg.u16_InhibitTimeout;

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             *Set General Parameters 
             */
            string str = Encoding.Default.GetString(tIncom_cfg.tGeneralCfg.u8_LocationTag, 0, tIncom_cfg.tGeneralCfg.u8_LocationTag.Length).Trim('\0');
            tTxt_LocationTag.Text = str;

            tCmb_LEDControl.SelectedIndex = tIncom_cfg.tGeneralCfg.u8_LEDCtrl;
            tCmb_AlarmOperationMode.SelectedIndex = tIncom_cfg.tGeneralCfg.u8_AlarmMode;
            tCmb_SafetyMode.SelectedIndex = tIncom_cfg.tGeneralCfg.u8_SafeMode;
            tCmb_CalOverdue.SelectedIndex = tIncom_cfg.tGeneralCfg.u8_CalOverDueOption;

            tUpdown_Passcode1.Value = tIncom_cfg.tGeneralCfg.u8_passcode[0];
            tUpdown_Passcode2.Value = tIncom_cfg.tGeneralCfg.u8_passcode[1];
            tUpdown_Passcode3.Value = tIncom_cfg.tGeneralCfg.u8_passcode[2];
            tUpdown_Passcode4.Value = tIncom_cfg.tGeneralCfg.u8_passcode[3];

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /* 
             * Set Circuit Calibration Parameters 
             */
            tUpdown_4to20mAOffsetSink.Value = tIncom_cfg.tCircuitCalCfg.f32_mAOutputSinkOffset;
            tUpdown_4to20mASpanSink.Value = tIncom_cfg.tCircuitCalCfg.f32_mAOutputSinkSpan;
            tUpdown_4to20mAOffsetSource.Value = tIncom_cfg.tCircuitCalCfg.f32_mAOutputSourceOffset;
            tUpdown_4to20mASpanSource.Value = tIncom_cfg.tCircuitCalCfg.f32_mAOutputSourceSpan;
            tUpdown_LoopbakcOffsetSink.Value = tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSinkOffset;
            tUpdown_LoopbackSpanSink.Value = tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSinkSpan;
            tUpdown_LoopbackOffsetSource.Value = tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSourceOffset;
            tUpdown_LoopbackSpanSource.Value = tIncom_cfg.tCircuitCalCfg.f32_mALoopbackSourceSpan;
            tUpdown_VoltageOutputOffset.Value = tIncom_cfg.tCircuitCalCfg.f32_VoltageOutputOffset;
            tUpdown_VoltageOutputSpan.Value = tIncom_cfg.tCircuitCalCfg.f32_VoltageOutputSpan;

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Set Device Info Parameters 
             */
            str = Encoding.Default.GetString(tIncom_cfg.tDeviceInfo.u8_DeviceSerialNum, 0, tIncom_cfg.tDeviceInfo.u8_DeviceSerialNum.Length).Trim('\0');
            tTxt_DeviceSerialNumber.Text = str;

            str = Encoding.Default.GetString(tIncom_cfg.tDeviceInfo.u8_BoardSerialNum, 0, tIncom_cfg.tDeviceInfo.u8_BoardSerialNum.Length).Trim('\0');
            tTxt_BoardSerialNumber.Text = str;

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);

            /*
             * Set Security Parameters
             */

            tUpdown_LoginRetry.Value = tIncom_cfg.tSecurityCfg.u8_num_login_retry;
            tUpdown_LoginLockTime.Value = tIncom_cfg.tSecurityCfg.u8_login_locktime;
            tUpdown_OTPConnection.Value = tIncom_cfg.tSecurityCfg.u8_otp_limits;

            str = Encoding.Default.GetString(tIncom_cfg.tSecurityCfg.u8_otp_key, 0, tIncom_cfg.tSecurityCfg.u8_otp_key.Length).Trim('\0');
            tTxt_OTPKey.Text = str;

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /* 
             * Set NTC Compensation Table 
             */
            for (int i = 0; i < (int)INNCOM_CONF.NUM_NTC_COMP; i++)
            {
                IntegerUpDown NTCUpDown = (IntegerUpDown)this.FindName(String.Format("tUpdown_NTCComp{0}", i + 1));
                NTCUpDown.Value = tIncom_cfg.tNtcCfg.s8_NtcTempComp[i];
            }

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


            /*
             * Get UL2075 Histogram Table 
             */
            for (int i = 0; i < (int)INNCOM_CONF.NUM_HISTOGRAM; i++)
            {
                DecimalUpDown UL2075 = (DecimalUpDown)this.FindName(String.Format("tUpdown_UL2075Count{0}", i + 1));
                UL2075.Value = tIncom_cfg.tUL2075Cfg.u8_Histogram[i];
            }

        }

        private void saveToFile()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Incom Configuration"; // Default file name
            dlg.DefaultExt = ".icfg"; // Default file extension
            dlg.Filter = "Incom configuration files (*.icfg)|.icfg| All fils |*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == false)
            {
                return;
            }
            string filename = dlg.FileName;
            getIncomConfigurationFromWindow();

            byte[] saveData = tIncom_cfg.getDataToByteArray();

            System.IO.File.WriteAllBytes(filename, saveData);

            MessageBox.Show("Successfully saved");
        }

        private void OpenFromFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "icfg"; // Default file extension
            dlg.Filter = "Incom configuration files (*.icfg)|*.icfg| All fils |*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == false)
            {
                return;
            }
            string filename = dlg.FileName;
            byte[] data = System.IO.File.ReadAllBytes(filename);

            result = tIncom_cfg.setDataFromByteArray(data);
            if (result == false)
            {
                MessageBox.Show("Wrong File");
                return;
            }

            result = verifyIncomConfiguration(ref data);
            if (result == false)
            {
                return;
            }

            setIncomConfigurationToWindow();

            MessageBox.Show("successfully Open File");
        }

        private bool verifyIncomConfiguration(ref byte[] data)
        {
            int crc16_sz = 2;
            byte[] classToBytes = null;
            ushort CalCRC = 0;

            classToBytes = tIncom_cfg.tModbusCfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tModbusCfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tRelayCfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tRelayCfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tmAOutputCfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tmAOutputCfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tGeneralCfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tGeneralCfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tCircuitCalCfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tCircuitCalCfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tDeviceInfo.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tDeviceInfo.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tSecurityCfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tSecurityCfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tNtcCfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tNtcCfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            classToBytes = tIncom_cfg.tUL2075Cfg.Serialize();
            CalCRC = crc16.UpdateCRC16(classToBytes, (uint)classToBytes.Length, crc16_sz);

            if (CalCRC != tIncom_cfg.tUL2075Cfg.u16_crc)
            {
                MessageBox.Show("Wrong File");
                return false;
            }

            return true;
        }

        private void readFromIncom()
        {
            int blockNum = tIncom_cfg.totalSize / (int)INNCOM_CONF.SZ_MAX_MEMORY_BLOCK;
            int eepAddr = 0;
            int dataLen = 0;
            byte[] EEP_Data = new byte[(int)INNCOM_CONF.SZ_EEP_MEMORY];

            for (int i = 0; i <= blockNum; i++)
            {
                if (i < blockNum)
                {
                    dataLen = (int)INNCOM_CONF.SZ_MAX_MEMORY_BLOCK;
                }
                else
                {
                    dataLen = tIncom_cfg.totalSize - (int)INNCOM_CONF.SZ_MAX_MEMORY_BLOCK * i;
                    if (dataLen == 0)
                    {
                        break;
                    }
                }

                byte[] payload = new byte[6];
                payload[0] = (int)INNCOM_CONF.MEM_TYPE_FLASH;
                payload[1] = (byte)(eepAddr >> 24);
                payload[2] = (byte)(eepAddr >> 16);
                payload[3] = (byte)(eepAddr >> 8);
                payload[4] = (byte)eepAddr;
                payload[5] = (byte)dataLen;

                byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_DATA_FROM_MEM, payload, ref err, 500);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - READ DATA FORM INCOM MEMORY");
                    return;
                }

                int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
                int dataToRead = result[offset];
                Array.Copy(result, offset + 1, EEP_Data, eepAddr, dataToRead);

                eepAddr += dataLen;
            }

            byte[] data = new byte[eepAddr];
            Array.Copy(EEP_Data, data, eepAddr);

            tIncom_cfg.setDataFromByteArray(data);

            bool resultVerification = verifyIncomConfiguration(ref data);
            if (!resultVerification)
            {
                MessageBox.Show("ERROR - READ DATA FORM INCOM MEMORY");
                return;
            }

            setIncomConfigurationToWindow();
            MessageBox.Show("Successfully Read From Incom");

        }

        private void updateIncom()
        {
            getIncomConfigurationFromWindow();

            byte[] data = tIncom_cfg.getDataToByteArray();

            int blockNum = tIncom_cfg.totalSize / (int)INNCOM_CONF.SZ_MAX_MEMORY_BLOCK;
            int eepAddr = 0;
            int dataLen = 0;

            for (int i = 0; i <= blockNum; i++)
            {
                if (i < blockNum)
                {
                    dataLen = (int)INNCOM_CONF.SZ_MAX_MEMORY_BLOCK;
                }
                else
                {
                    dataLen = tIncom_cfg.totalSize - (int)INNCOM_CONF.SZ_MAX_MEMORY_BLOCK * i;

                    if(dataLen == 0)
                    {
                        break;
                    }
                }

                byte[] payload = new byte[6 + dataLen];
                payload[0] = (int)INNCOM_CONF.MEM_TYPE_EEPROM;
                payload[1] = (byte)(eepAddr >> 24);
                payload[2] = (byte)(eepAddr >> 16);
                payload[3] = (byte)(eepAddr >> 8);
                payload[4] = (byte)eepAddr;
                payload[5] = (byte)dataLen;

                Array.Copy(data, eepAddr, payload, 6, dataLen);

                byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_WRITE_DATA_TO_MEM, payload, ref err, 1000);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("ERROR - WRITE DATA TO INCOM MEMORY");
                    return;
                }
                eepAddr += dataLen;
            }

            MessageBox.Show("Successfully Write To Incom");
        }

    }
}
