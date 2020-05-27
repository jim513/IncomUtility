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
            int major = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            int minor = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 4];
            int built = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 5] * 256 + result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 6];
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
            int payloadLen = result[(int)PACKET_CONF.COMM_POS_LEN] * 256 + result[(int)PACKET_CONF.COMM_POS_LEN + 1];
            byte[] DeviceSNArray = new byte[payloadLen - 3];
            Array.Copy(result, (int)PACKET_CONF.COMM_POS_PAYLOAD + 3, DeviceSNArray, 0, payloadLen - 3);
            string DeviceSN = Encoding.Default.GetString(DeviceSNArray).Trim('\0');

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
            int OutputType = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            int Relay = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 4];
            int BLEModule = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 5];

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
            int sensorType = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            int gasType = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 4];
            int cellID = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 5];
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
           *  Read Sensor Info
           */
            byte[] channelByte = { 0x00 };
            result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_GAS_INFO,channelByte, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read Gas Information");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            int channel = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            int measurementUnit = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 4];
            int resolution = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 5];
            byte[] fullScaleArray = new byte[4];
            for( int i=0; i < 4; i++)
                fullScaleArray[i]= result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 6 + i];
            if (BitConverter.IsLittleEndian)
                Array.Reverse(fullScaleArray);

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

            float fullScale = BitConverter.ToSingle(fullScaleArray, 0);
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

            tTxt_Logs.AppendText("Cleared Latch Table : " + result[(int)PACKET_CONF.COMM_POS_PAYLOAD +2]);
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
                serialNumber = Quattro.mergeByteArray(serialNumber, ret);
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


        /*
         * Not Completed
         */
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

            int type = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            
            byte[] IDA = new byte[2];
            Array.Copy(result, (int)PACKET_CONF.COMM_POS_PAYLOAD + 3, IDA, 0, 2);
            string ID = BitConverter.ToString(IDA).Replace("-", string.Empty);

            int payloadLen = result[(int)PACKET_CONF.COMM_POS_LEN] * 256 + result[(int)PACKET_CONF.COMM_POS_LEN + 1];
            byte[] value = new byte[payloadLen - 5];
            Array.Copy(result, (int)PACKET_CONF.COMM_POS_PAYLOAD + 5, value, 0, payloadLen - 5);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);
            string str="";
            switch (type)
            {
                case (int)INNCOM_COMMAND_LIST.PARAM_TYPE_STR :
                    str = Encoding.Default.GetString(value).Trim('\0');
                    break;
                case (int)INNCOM_COMMAND_LIST.PARAM_TYPE_U8:
                    str = value[0].ToString();
                    break;
                case (int)INNCOM_COMMAND_LIST.PARAM_TYPE_U8A:
                    str = Encoding.Default.GetString(value).Trim('\0');
                    break;
                case (int)INNCOM_COMMAND_LIST.PARAM_TYPE_S8:
                    str = ((sbyte)value[0]).ToString();
                    break;
                case (int)INNCOM_COMMAND_LIST.PARAM_TYPE_U32:
                    if (value.Length <4)
                    {
                        byte[] temp = new byte[4-value.Length];
                        value = Quattro.mergeByteArray(temp, value);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(value);
                    }
                    str = BitConverter.ToUInt32(value,0).ToString();
                    break;
                case (int)INNCOM_COMMAND_LIST.PARAM_TYPE_U16:
                    if(value.Length == 1 )
                    {
                        byte[] temp = new byte[1];
                        value = Quattro.mergeByteArray(temp, value);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(value);
                    }   
                    str = BitConverter.ToUInt16(value, 0).ToString();
                    break;
                case (int)INNCOM_COMMAND_LIST.PARAM_TYPE_F32:
                    if (value.Length < 4)
                    {
                        byte[] temp = new byte[4 - value.Length];
                        value = Quattro.mergeByteArray(temp, value);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(value);
                    }
                    str = BitConverter.ToSingle(value,0).ToString();
                    break;
                default:
                    str = BitConverter.ToString(value).Replace("-", string.Empty);
                    break;
            }
            tTxt_Logs.AppendText("Parameter ID : 0x"+ ID + " , Value : "+str );
            tTxt_Logs.AppendText(Environment.NewLine);
        }
    }
}
