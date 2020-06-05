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

        public void setText(byte [] txbuffer , byte[] rxbuffer)
        {
            DateTime now = DateTime.Now;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                tTxt_Log.AppendText(now.ToLongTimeString() + " TX : " + BitConverter.ToString(txbuffer));
                tTxt_Log.AppendText(Environment.NewLine);
                tTxt_Log.AppendText(now.ToLongTimeString() + " RX : " + BitConverter.ToString(rxbuffer));
                tTxt_Log.AppendText(Environment.NewLine);
            }));

        }
        public APP_UI_CommLog()
        {
            InitializeComponent();
        }
    }
}
