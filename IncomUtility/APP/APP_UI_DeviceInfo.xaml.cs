using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IncomUtility
{ 
    /// <summary>
    /// APP_UI_DeviceInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_DeviceInfo : Window
    {
        ERROR_LIST err;
        public APP_UI_DeviceInfo()
        {
            InitializeComponent();

            tBtn_ReadDeviceInfo.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            tCmb_Param.SelectedIndex = 0;
        }

        private void tBtn_ReadDeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Read SW Version
             */
            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_SW_VERSION, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read SW Version");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            int major = result[offset];
            int minor = result[offset + 1];
            int built = Utility.getU16FromByteA(result, offset + 2);
            string SWVersion= major.ToString() + "." + minor.ToString() + "." + built.ToString();

            tTxt_Logs.AppendText("Device SW Version : " + SWVersion);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_DeviceSWVersion.Text = SWVersion;

            /*
            *  Read EEPROM Version
            */
            result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_EEPROM_VER, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read EERPOM Version");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            byte E2PVer = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];

            tTxt_Logs.AppendText("SENSOR EEPROM : " + E2PVer);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_SensorEEPROM.Text = E2PVer.ToString();

            /*
            *  Read Device SN
            */
            result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_DEVICE_SN, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read Device SN");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
           
            string DeviceSN = Encoding.Default.GetString(Quattro.getResponseValueData(result)).Trim('\0');

            tTxt_Logs.AppendText("Device SN : " + DeviceSN);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_DeviceSerialNumber.Text = DeviceSN;


            /*
            *  Read Output Type
            */
            result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_OUTPUT_DEVICE_TYPE, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read Board SN");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            int OutputType = result[offset];
            int Relay = result[offset + 1];
            int BLEModule = result [offset + 2];

            tTxt_Logs.AppendText("Output Type : " + OutputType);
            tTxt_Logs.AppendText(Environment.NewLine);    
            tTxt_Logs.AppendText("Relay Type: " + Relay);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("BLE Fitted : " + BLEModule);
            tTxt_Logs.AppendText(Environment.NewLine);
            
            if (OutputType == 0)
                tTxt_OutputDeviceType.Text = "mA Output";
            else
                tTxt_OutputDeviceType.Text = "Modbus";
            if (Relay == 0)
                tTxt_RealyOption.Text = "Not Fitted";
            else
                tTxt_RealyOption.Text = "Fitted";
            if (BLEModule == 0)
                tTxt_BLEModule.Text = "Not Fitted";
            else
                tTxt_BLEModule.Text = "Fitted";

           /*
           *  Read Sensor Info
           */
            result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_SENSOR_INFO, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read Sensor Information");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            int sensorType = result[offset];
            int gasType = result[offset + 1];
            int cellID = result[offset + 2];
            switch (sensorType)
            {
                case 0: tTxt_SensorType.Text = "ECC"; break;
                case 1: tTxt_SensorType.Text = "FLM CAT"; break;
                case 2: tTxt_SensorType.Text = "IR"; break;
                case 3: tTxt_SensorType.Text = "PID"; break;
                case 4: tTxt_SensorType.Text = "MOS"; break;
                default: tTxt_SensorType.Text = "N/A"; break;
            }
            switch (gasType)
            {
                case 0: tTxt_GasType.Text = "Flammable"; break;
                case 1:  tTxt_GasType.Text = "Toxic";  break;
                case 2: tTxt_GasType.Text = "O2";  break;
                case 3:  tTxt_GasType.Text = "VOC";break;
                default: tTxt_GasType.Text = "N/A"; break;
            }

            tCmb_CellID.SelectedIndex = cellID;

            tTxt_Logs.AppendText("Sensor Type : " + sensorType);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Gas Type: " + gasType);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Cell ID : " + cellID);
            tTxt_Logs.AppendText(Environment.NewLine);

            /*
           *  Read Gas Info
           */
            byte[] channelByte = { 0x00 };
            result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_GAS_INFO,channelByte, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read Gas Information");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            int channel = result[offset];
            int measurementUnit = result[offset + 1];
            int resolution = result[offset + 2];
            float fullScale = Utility.getF32FromByteA(result, offset + 3);
          
            tTxt_Channel.Text = channel.ToString();
            switch (measurementUnit)
            {
                case 0: tTxt_MeasuremetUnits.Text = "blank"; break;
                case 1: tTxt_MeasuremetUnits.Text = "%LEL"; break;
                case 2: tTxt_MeasuremetUnits.Text = "mA"; break;
                case 3: tTxt_MeasuremetUnits.Text = "mg/m3"; break;
                case 4: tTxt_MeasuremetUnits.Text = "g/m3"; break;
                case 5: tTxt_MeasuremetUnits.Text = "%Vol"; break;
                case 6: tTxt_MeasuremetUnits.Text = "ppm"; break;
                case 7: tTxt_MeasuremetUnits.Text = "kppm"; break;
                case 8: tTxt_MeasuremetUnits.Text = "LEL.m"; break;
                case 9: tTxt_MeasuremetUnits.Text = "A"; break;
                case 10: tTxt_MeasuremetUnits.Text = "dB"; break;
                case 11: tTxt_MeasuremetUnits.Text = "dBA"; break;
                case 12: tTxt_MeasuremetUnits.Text = "ppm.m"; break;
                default: tTxt_MeasuremetUnits.Text = ""; break;
            }
            switch (resolution)
            {
                case 0: tTxt_DecimalPlace.Text = "1 " + tTxt_MeasuremetUnits.Text; break;
                case 1: tTxt_DecimalPlace.Text = "0.1 " + tTxt_MeasuremetUnits.Text; break;
                case 2: tTxt_DecimalPlace.Text = "0.01 " + tTxt_MeasuremetUnits.Text; break;
                case 3: tTxt_DecimalPlace.Text = "0.001 " + tTxt_MeasuremetUnits.Text; break;
                default: tTxt_DecimalPlace.Text = "1 " + tTxt_MeasuremetUnits.Text;
                    resolution = 0;
                    break;
            }

            double DfullScale = Math.Round(fullScale, resolution);
            tTxt_FullScaleRange.Text = DfullScale.ToString() +" "+ tTxt_MeasuremetUnits.Text;

            tTxt_Logs.AppendText("Channel : " + channel);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Measurement Unit: " + measurementUnit);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Decimal Point : " + resolution);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Full Scale : " + DfullScale);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void tBtn_ClearAllLatchedTable_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Clear Latch tables
             */
            byte[] payload = new byte[1];
            payload[0] = (byte)(tCmb_LatchedType.SelectedIndex + 1);
            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_CLR_LATCHED_TABLES,payload, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Clear Latch Tables");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Cleared Latch Table : " + (result,(int)PACKET_CONF.COMM_POS_PAYLOAD +2));
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void tBtn_ResetToFactory_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Reset To Factory
             */
            SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_RESET_FACTORY, ref err, 500);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Reset To Factory");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("Reset To Factory : OK");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void tBtn_ResetAlarmFault_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Reset Alarms and Faults 
             */
            SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_RESET_ALARMS, ref err, 1500);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Reset Alarms and Faults");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Reset Alarms and Faults : OK");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void tBtn_WriteDeviceSN_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            /*
             * Write Device SN
             */
            byte[] serialNumber = Encoding.Default.GetBytes(tTxt_DeviceSerialNumber.Text);
            if (serialNumber.Length < 16)
            {
                byte[] ret = new byte[16 - serialNumber.Length];
                serialNumber = Utility.mergeByteArray(serialNumber, ret);
            }   

            SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_WRITE_DEVICE_SN, serialNumber, ref err, 300);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Write Device SN");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Wrote Device SN Successfully");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void tBtn_ReadConfiguration_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Read Configuration
             */
            INNCOM_CONF param = (INNCOM_CONF)tCmb_Param.SelectedItem;

            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_CONFIG, Quattro.commandToByteArray(param), ref err ,300);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read Configurations");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
           
            byte[] value = Quattro.getResponseValueData(result);
            
            int type = result[offset];
            
            byte[] IDA = new byte[2];
            Array.Copy(value, 0, IDA, 0, 2);
            string ID = BitConverter.ToString(IDA).Replace("-", string.Empty);

            byte[] ParamValue = new byte[value.Length - IDA.Length];
            Array.Copy(value, IDA.Length , ParamValue, 0, value.Length - IDA.Length);
            
            string str="";
            switch (type)
            {
                case (int)INNCOM_CONF.PARAM_TYPE_STR :
                    str = Encoding.Default.GetString(ParamValue).Trim('\0');
                    break;
                case (int)INNCOM_CONF.PARAM_TYPE_U8:
                    str = (ParamValue ,0).ToString();
                    break;
                case (int)INNCOM_CONF.PARAM_TYPE_U8A:
                    str = Encoding.Default.GetString(ParamValue).Trim('\0');
                    break;
                case (int)INNCOM_CONF.PARAM_TYPE_S8:
                    str = Convert.ToSByte(ParamValue[0]).ToString();
                    break;
                case (int)INNCOM_CONF.PARAM_TYPE_U32:
                    if (ParamValue.Length <4)
                    {
                        byte[] temp = new byte[4-ParamValue.Length];
                        ParamValue = Utility.mergeByteArray(temp, ParamValue);
                    }
                    str = Utility.getU32FromByteA(ParamValue,0).ToString();
                    break;
                case (int)INNCOM_CONF.PARAM_TYPE_U16:
                    if(ParamValue.Length == 1 )
                    {
                        byte[] temp = new byte[1];
                        ParamValue = Utility.mergeByteArray(temp, ParamValue);
                    }   
                    str = Utility.getU16FromByteA(ParamValue, 0).ToString();
                    break;
                case (int)INNCOM_CONF.PARAM_TYPE_F32:
                    if (ParamValue.Length < 4)
                    {
                        byte[] temp = new byte[4 - ParamValue.Length];
                        ParamValue = Utility.mergeByteArray(temp, ParamValue);
                    }
                    str = Utility.getF32FromByteA(ParamValue,0).ToString();
                    break;
                default:
                    str = BitConverter.ToString(ParamValue).Replace("-", string.Empty);
                    break;
            }
            tTxt_Logs.AppendText("Parameter ID : 0x" + ID + " , Value : " + str);
            tTxt_Logs.AppendText(Environment.NewLine);
        }
    }
}
