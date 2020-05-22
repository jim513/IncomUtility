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
        Quattro quattro = new Quattro();
        SerialPortIO serial = new SerialPortIO();
        ERROR_LIST err;
        private string CheckInput(string InputText)
        {
            int value;

            if (!int.TryParse(InputText, out value))
            {
                tTxt_Log.AppendText("Input is not hex number");
                tTxt_Log.AppendText(Environment.NewLine);
                return null;
            }
            if (InputText.Length == 1)
                return string.Concat("0", InputText);

            if (value > 255 || value < 0)
                return null;
            else
                return InputText;
        }

        DateTime now;
        private void tBtn_SendCMD_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.serialPort.IsOpen)
            {
                tTxt_Log.AppendText("Port is not open");
                tTxt_Log.AppendText(Environment.NewLine);
                return ;
            }

            byte[] CMD;
            if (tBtn_CMD1.Text != "" && tBtn_CMD2.Text != "")
            {
                string s1 = CheckInput(tBtn_CMD1.Text);
                string s2 = CheckInput(tBtn_CMD2.Text);
               
                CMD = Quattro.StringToByteArray(string.Concat(s1, s2));
            }
            else
            {
                tTxt_Log.AppendText("Input is not hex number");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }

            if(tBtn_Payload.Text !="")
            {
                string s3 = tBtn_Payload.Text;
                int value;
                if (!int.TryParse(s3, out value))
                {
                    tTxt_Log.AppendText("Input is not hex number");
                    tTxt_Log.AppendText(Environment.NewLine);
                    return; 
                }
                if (s3.Length == 1)
                    s3= string.Concat("0", s3);
                CMD = Quattro.mergeByteArray(CMD, Quattro.StringToByteArray(s3));
            }

            now = DateTime.Now;
            byte[] u8TXbuffer =quattro.buildCMDPacket((byte)PACKET_CONF.COMM_SYSTEM_MFG_PC, (byte)PACKET_CONF.COMM_SYSTEM_INCOM, CMD, ref err);
            tTxt_Log.AppendText(now.ToLongTimeString() + " TX : " + BitConverter.ToString(u8TXbuffer));
            tTxt_Log.AppendText(Environment.NewLine);

            writePacket(ref u8TXbuffer, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Log.AppendText("Write Failed!");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }
            byte[] u8RXbuffer = readPacket(ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Log.AppendText("Read Failed!");
                tTxt_Log.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Log.AppendText(now.ToLongTimeString() + " RX : " + BitConverter.ToString(u8RXbuffer));
            tTxt_Log.AppendText(Environment.NewLine);
        }
        private void writePacket(ref byte[] sendbuffer, ref ERROR_LIST err)
        {
            if (!SerialPortIO.serialPort.IsOpen)
            {
                err = ERROR_LIST.ERROR_PORT_NOT_OPEN;
                return;
            }
            if (sendbuffer == null)
            {
                err = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return;
            }
            SerialPortIO.mutex.WaitOne();
            
            while (SerialPortIO.serialPort.BytesToRead > 0)           
                SerialPortIO.serialPort.ReadExisting();          
        
            SerialPortIO.serialPort.Write(sendbuffer, 0, sendbuffer.Length);
            err = ERROR_LIST.ERROR_NONE;
            Thread.Sleep(100);
        }

        private byte[] readPacket(ref ERROR_LIST err)
        {
            if (!SerialPortIO.serialPort.IsOpen)
            {
                err = ERROR_LIST.ERROR_PORT_NOT_OPEN;
                return null;
            }

            int readBytes = SerialPortIO.serialPort.BytesToRead;
            byte[] readBuffer = new byte[readBytes];

            if (SerialPortIO.serialPort.BytesToRead > 1)
            {
                SerialPortIO.serialPort.Read(readBuffer, 0, readBytes);
            }
            else
            {
                err = ERROR_LIST.ERROR_RECIVE_DATA_NONE;
                readBuffer = null;
            }

            SerialPortIO.mutex.ReleaseMutex();
            err = ERROR_LIST.ERROR_NONE;
            return readBuffer;
        }
    }
}
