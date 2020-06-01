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
    /// APP_UI_CommLog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_CommLog : Window
    {
        public static bool packetCheck = false;
        public static byte[] u8Txbuffer = new byte[2048];
        public static byte[] u8Rxbuffer = new byte[2048];
        public static Mutex isSended;
        public static int u8TxbufferLength ;
        public static int u8RxbufferLength ;
        public Thread monitorSendLog;
        public static void getSend(byte[] txBuffer, byte[] rxBuffer , int txLength, int rxLength)
        {       
            Array.Copy(txBuffer, u8Txbuffer, txLength);
            Array.Copy(rxBuffer, u8Rxbuffer, rxLength);
            isSended.ReleaseMutex();
        }
        public void setText()
        {
            while (packetCheck)
            {
                isSended.WaitOne();
                byte[] txbuffer = new byte[u8TxbufferLength];
                byte[] rxbuffer = new byte[u8RxbufferLength];
                Array.Copy(u8Txbuffer, txbuffer, u8TxbufferLength);
                Array.Copy(u8Rxbuffer, rxbuffer, u8RxbufferLength);
                DateTime now = DateTime.Now;
                tTxt_Log.AppendText(now.ToLongTimeString() + " TX : " + BitConverter.ToString(txbuffer));
                tTxt_Log.AppendText(Environment.NewLine);
                tTxt_Log.AppendText(now.ToLongTimeString() + " RX : " + BitConverter.ToString(rxbuffer));
                tTxt_Log.AppendText(Environment.NewLine);
            }
        }
        public APP_UI_CommLog()
        {
            InitializeComponent();
            isSended = new Mutex();
            isSended.WaitOne();
            monitorSendLog = new Thread(setText);
        }
    }
}
