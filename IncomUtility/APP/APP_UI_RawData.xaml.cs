using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// APP_UI_RawData.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_RawData : Window
    {
        ERROR_LIST err;
        DateTime now;
        public APP_UI_RawData()
        {
            InitializeComponent();
        }

        private void tBtn_GetGasReading_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Get Gas Reading
             */
            byte[] channel = { 0x00 };
            byte[] result=SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_GAS_READING, channel, ref err, 300);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Get Gas Reading");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            int recivedChannel = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
            byte[] channelData = new byte[4];
            
            Array.Copy(result, (int)PACKET_CONF.COMM_POS_PAYLOAD + 4, channelData, 0, 4);
            int status = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 8] * 256 + result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 9];
            if (BitConverter.IsLittleEndian)
                Array.Reverse(channelData);
            float value = BitConverter.ToSingle(channelData, 0);
          
            now = DateTime.Now;
            tTxt_Logs.AppendText(now.ToLongTimeString()  +" Channel : " + recivedChannel  +", Readings : " + value.ToString("F1") +
                ", Status : " + status);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void tBtn_ReadAnalogueOutput_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Read Analogue Output
             */
            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_ANALOGUE_OUTPUT, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ ANLOGUE TYPE");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            int pos = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            byte mAOutputType = result[pos];
            int targetOutput = result[pos + 1] << 24 | result[pos + 2] << 16 | result[pos + 3] << 8 |  result[pos + 4];
            int loopBack = result[pos + 5] << 24 | result[pos + 6] << 16 | result[pos + 7] << 8 | result[pos + 8];
            float f32targetOutput = Convert.ToSingle(targetOutput);
            float f32loopBakc = Convert.ToSingle(loopBack);

            if (mAOutputType == 1)
                tTxt_Logs.AppendText("Analogue Type : " + mAOutputType + " - sink mode");
            else if (mAOutputType == 2)
                tTxt_Logs.AppendText("Analogue Type : "+ mAOutputType + " - source mode");
            else
                tTxt_Logs.AppendText("Analogue Type : " + mAOutputType + " - Not specified");
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Target output (mA) : " + f32targetOutput );
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Loopback (mA): " + f32loopBakc);
            tTxt_Logs.AppendText(Environment.NewLine);
        }
    }
}
