using Microsoft.Win32;
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
    /// APP_UI_EditSensorData.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_SensorData : Window
    {
        ERROR_LIST err;

        private LogToFile datas;
       
        public APP_UI_SensorData()
        {
            InitializeComponent();
        }

        private void tBtn_ReadSensorData_Click(object sender, RoutedEventArgs e)
        {
            if(!SerialPortIO.isPortOpen())
            {
                tTxt_Logs.AppendText("Incom is not connected");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Read Data From Memory
             */
            tBtn_WriteSensorData.IsEnabled = false ;
            tBtn_ReadSensorData.IsEnabled = false;

            const int FILESIZE = 1726;
            int blockNum = FILESIZE / (int)INNCOM_COMMAND_LIST.SZ_MAX_MEMORY_BLOCK;       
            int eepAddr = 0;
            int dataLen = 0;

            tPbar_DataDownBar.Maximum = blockNum;
            tPbar_DataDownBar.Value = 0;
           
            byte[] EEP_Data = new byte[(int)INNCOM_COMMAND_LIST.SZ_EEP_MEMORY];

            for (int i = 0; i <= blockNum; i++)
            {
                if (i < blockNum)
                    dataLen = (int)INNCOM_COMMAND_LIST.SZ_MAX_MEMORY_BLOCK;
                else
                    dataLen = FILESIZE - (int)INNCOM_COMMAND_LIST.SZ_MAX_MEMORY_BLOCK * i;

                byte[] payload = new byte[6];
                payload[0] = (int)INNCOM_COMMAND_LIST.MEM_TYPE_EEPROM;
                payload[1] = (byte)(eepAddr >> 24);
                payload[2] = (byte)(eepAddr >> 16);
                payload[3] = (byte)(eepAddr >> 8);
                payload[4] = (byte)eepAddr;
                payload[5] = (byte)dataLen;

                byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_DATA_FROM_MEM, payload, ref err, 500);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    tTxt_Logs.AppendText("ERROR - READ RAW GAS DATA eerpom : " + eepAddr);
                    tTxt_Logs.AppendText(Environment.NewLine);
                    return;
                }

                int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;
                int dataToRead = result[offset];
                Array.Copy(result, offset + 1, EEP_Data, eepAddr, dataToRead);

                eepAddr += dataLen;

                tPbar_DataDownBar.Value = i + 1;
                tTxt_Logs.AppendText("Read eeprom : " + eepAddr);
                tTxt_Logs.AppendText(Environment.NewLine);
                Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                Thread.Sleep(50);
            }

            tBtn_WriteSensorData.IsEnabled = true;
            tBtn_ReadSensorData.IsEnabled = true;

            datas = new LogToFile("Incom_Sensor_Dater", "bin");
            if (datas.Checked() == null)
                return;
            try
            {
                datas.Write(EEP_Data, 0, eepAddr);
                datas.Unchecked();
            }
            catch
            {
                tTxt_Logs.AppendText("Save Sensor Data : ");
                tTxt_Logs.AppendText(Environment.NewLine);
            }
            tTxt_Logs.AppendText("Successfully saved");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void tBtn_WriteSensorData_Click(object sender, RoutedEventArgs e)
        {
            //if (!SerialPortIO.isPortOpen())
            //{
            //    tTxt_Logs.AppendText("Incom is not connected");
            //    tTxt_Logs.AppendText(Environment.NewLine);
            //    return;
            //}

            /*
             * Write Data To Memory
             */
            OpenFileDialog BrowserDialog = new OpenFileDialog();
            BrowserDialog.Filter = "*.bin|*.*";
            BrowserDialog.FilterIndex = 2;

            if (BrowserDialog.ShowDialog() ?? false)
            {
                tTxt_Logs.AppendText("File was not opened");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
    }
}
}
