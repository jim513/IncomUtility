using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// APP_UI_BLEInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_BLEInfo : Window
    {
        ERROR_LIST err;

        public APP_UI_BLEInfo()
        {
            InitializeComponent();

            tBtn_getBLEInfo.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void tBtn_getBLEInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_BLEInfo.AppendText("Incom is not connected");
                tTxt_BLEInfo.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Read BLE Name
             */
            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_BLE_DEVICE_NAME, ref err);
            if(err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_BLEInfo.AppendText("ERROR - Read BLE Name");
                tTxt_BLEInfo.AppendText(Environment.NewLine);
                return;
            }
            
            string BLEname = Encoding.Default.GetString(Quattro.getResponseValueData(result)).Trim('\0');
            
            tTxt_BLEInfo.AppendText("BLE Device Name : " + BLEname);
            tTxt_BLEInfo.AppendText(Environment.NewLine);
            tTxt_BLEDeviceName.Text = BLEname;

            /*
             * Read BLE Mac Address
             */
            result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_BLE_MAC_ADDR, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_BLEInfo.AppendText("ERROR - Read BLE Mac Addreess");
                tTxt_BLEInfo.AppendText(Environment.NewLine);
                return;
            }
            string BLEMacAddr = BitConverter.ToString(Quattro.getResponseValueData(result)).Replace("-", ":");

            tTxt_BLEInfo.AppendText("BLE MAC Address : " + BLEMacAddr);
            tTxt_BLEInfo.AppendText(Environment.NewLine);
            tTxt_BLEMacAddress.Text = BLEMacAddr;

            /*
             *  Read BLE SW Version
             */
            result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_BLE_SW_VER, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_BLEInfo.AppendText("ERROR - Read BLE SW Version");
                tTxt_BLEInfo.AppendText(Environment.NewLine);
                return;
            }
           
            string BLESWVersion = Encoding.Default.GetString(Quattro.getResponseValueData(result)).Trim('\0');

            tTxt_BLEInfo.AppendText("BLE SW Version : " + BLESWVersion);
            tTxt_BLEInfo.AppendText(Environment.NewLine);
            tTxt_BLESWVersion.Text = BLESWVersion;

            /*
             * Read BLE TX Power
             */
            result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_BLE_TX_POWER, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_BLEInfo.AppendText("ERROR - Read BLE TW Power");
                tTxt_BLEInfo.AppendText(Environment.NewLine);
                return;
            }
            byte BLETXPower = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            tTxt_BLEInfo.AppendText("BLE TX Power : " + BLETXPower);
            tTxt_BLEInfo.AppendText(Environment.NewLine);
            tTxt_BLETXPower.Text = BLETXPower.ToString();

            /*
             * Read BLE Instrument ID
             */        
             result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_BLE_INST_ID, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_BLEInfo.AppendText("ERROR - Read Instrument ID");
                tTxt_BLEInfo.AppendText(Environment.NewLine);
                return;
            }
            string BLEInstID = Encoding.Default.GetString(Quattro.getResponseValueData(result)).Trim('\0');

            tTxt_BLEInfo.AppendText("BLE Instrument Id : " + BLEInstID);
            tTxt_BLEInfo.AppendText(Environment.NewLine);
            tTxt_BLEInstID.Text = BLEInstID;
        }
    }
}
