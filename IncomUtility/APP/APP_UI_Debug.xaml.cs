using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// APP_UI_Debug.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_Debug : Window
    {
        public APP_UI_Debug()
        {
            InitializeComponent();
        }

        QuattroProtocol quattro = new QuattroProtocol();
        ERROR_LIST err;
        DateTime now;

        private string CheckInput(string InputText)
        {

            if (!IsHex(InputText))
            {
                tTxt_Log.AppendText("Input is not hex number");
                tTxt_Log.AppendText(Environment.NewLine);
                return null;
            }
            if (InputText.Length == 1)
            {
                return string.Concat("0", InputText);
            }

            return InputText;

        }
        private void sendCMD()
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Log.AppendText("Port is not open");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }

            byte[] CMD;
            if (tBtn_CMD1.Text != "" && tBtn_CMD2.Text != "")
            {
                string s1 = CheckInput(tBtn_CMD1.Text);
                string s2 = CheckInput(tBtn_CMD2.Text);

                if (s1 == null || s2 == null)
                {
                    tTxt_Log.AppendText("Input is not hex number");
                    tTxt_Log.AppendText(Environment.NewLine);
                    return;
                }
                CMD = Utility.hexStringToByteArray(string.Concat(s1, s2));
            }
            else
            {
                tTxt_Log.AppendText("Input is not hex number");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }

            /*
             *  Check Additional Input
             */
            if (tBtn_Payload.Text != "")
            {
                string addtionalCmd = tBtn_Payload.Text;
                if (addtionalCmd.Length % 2 == 1)
                {
                    addtionalCmd = string.Concat("0", addtionalCmd);
                }


                if (!IsHex(addtionalCmd))
                {
                    tTxt_Log.AppendText("Input is not hex number");
                    tTxt_Log.AppendText(Environment.NewLine);
                    return;
                }

                CMD = Utility.mergeByteArray(CMD, Utility.hexStringToByteArray(addtionalCmd));
            }

            /*
             * Build Packet
             */
            now = DateTime.Now;
            byte[] u8TXbuffer = QuattroProtocol.buildCMDPacket((byte)PACKET_CONF.COMM_SYSTEM_MFG_PC, (byte)PACKET_CONF.COMM_SYSTEM_INCOM, CMD, ref err);
            tTxt_Log.AppendText(now.ToLongTimeString() + " TX : " + BitConverter.ToString(u8TXbuffer));
            tTxt_Log.AppendText(Environment.NewLine);

            /*
             * SendCommand
             */
            byte[] u8RXbuffer = QuattroProtocol.sendCommand(CMD, ref err, (int)tUpdown_DelayTime.Value);
            if(err == ERROR_LIST.ERROR_RECIVE_DATA_NONE)
            {
                tTxt_Log.AppendText("Delay Time is too Short");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }
            if (u8RXbuffer != null)
            {
                tTxt_Log.AppendText(now.ToLongTimeString() + " RX : " + BitConverter.ToString(u8RXbuffer));
                tTxt_Log.AppendText(Environment.NewLine);
            }
            if (err == ERROR_LIST.ERROR_NCK)
            {
                tTxt_Log.AppendText("Recieve Data is Wrong");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Log.AppendText("Write Failed!");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }

          
        }

        private bool IsHex(IEnumerable<char> chars)
        {
            bool isHex;
            foreach (var c in chars)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }
        private void tBtn_SendCMD_Click(object sender, RoutedEventArgs e)
        {
            sendCMD();
        }
     
    }
}
