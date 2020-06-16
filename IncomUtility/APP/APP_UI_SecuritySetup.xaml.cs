using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    /// APP_UI_SecuritySetup.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_SecuritySetup : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;

        int offset = (int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ;

        public APP_UI_SecuritySetup()
        {
            InitializeComponent();

            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void tBtn_CheckUserCerInfo_Click(object sender, RoutedEventArgs e)
        {
            checkUserCerInfo();
        }

        private void tBtn_ReadCryptCISN_Click(object sender, RoutedEventArgs e)
        {
            readCryptSN();
        }

        private void tBtn_RequestDeviceCertInfo_Click(object sender, RoutedEventArgs e)
        {
            requestDeviceCertInfo();
        }

        private void tBtn_CreateKeyPair_Click(object sender, RoutedEventArgs e)
        {
            createKeyPair();
        }

        private void tBtn_GetDevicePublicKey_Click(object sender, RoutedEventArgs e)
        {
            getDevicePublicKey();
        }

        private void tBtn_TransferCertificate_Click(object sender, RoutedEventArgs e)
        {
            transferCertificate();
        }

        private void tBtn_VerifyCertificate_Click(object sender, RoutedEventArgs e)
        {
            verifyCertificate();
        }

        private void tBtn_DonwloadCertificate_Click(object sender, RoutedEventArgs e)
        { 
           downloadCertificate();
        }
        /*
         *  Need Debug
         */
        private void checkUserCerInfo()
        {
            /*
             * Check User Certificate
             */

            string str = tTxt_UserCertName.Text;
            int userNameSize = 20;
            byte[] sendData = new byte[4 + userNameSize];

            DateTime setDate = tDate_ExpirationDate.SelectedDate.Value;

            int year = setDate.Year;
            sendData[0] = (byte)(year >> 8);
            sendData[1] = (byte)year;
            sendData[2] = (byte)setDate.Month;
            sendData[3] = (byte)setDate.Day;

            if(str.Length > userNameSize +4)
            {
                return;
            }

            Encoding.ASCII.GetBytes(str, 0, str.Length, sendData, 4);

            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_CHECK_USER_CERT_INFO,sendData, ref err,1000);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read User Certificate Information");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            byte user = result[(int)PACKET_CONF.COMM_RESPONSE_SZ + (int)PACKET_CONF.COMM_POS_PAYLOAD];
            tTxt_Logs.AppendText("User Cert Status : " + user);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void createKeyPair()
        {
            /*
             * Create Key Pair
             */
            SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_CREATE_KEY_PAIR, ref err ,300);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Create Key Pair");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("Key Pair has been created successfully");
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void getDevicePublicKey()
        {
            /*
             *  Get Device Public Key
             */
            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_GET_DEVICE_PUBLIC_KEY, ref err ,500);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Get Device Public Key");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
          
            byte[] stringValue = QuattroProtocol.getResponseValueData(result);
            string str = BitConverter.ToString(stringValue).Replace("-", string.Empty).Trim('\0');
            tTxt_Logs.AppendText("Device Public Key : " + str);
            tTxt_Logs.AppendText(Environment.NewLine);
          
        }

        private void readCryptSN()
        {
            /*
             * Read Crypt IC SN
             */
            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_CRYPT_SN, ref err, 500);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Read Crypt SN");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            byte[] stringValue = QuattroProtocol.getResponseValueData(result);
            string str = BitConverter.ToString(stringValue).Replace("-", string.Empty).Trim('\0');
            tTxt_Logs.AppendText("Crypt IC SN : " + str);
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        private void requestDeviceCertInfo()
        {
            /*
            * Read Device Certificate Information
            */
            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_REQUEST_DEVICE_CERT_INFO, ref err, 1000);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Request Cert Info");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            byte[] value = QuattroProtocol.getResponseValueData(result);
          
            int year = (int)value[0] << 8 | (int)(value[1]);
            int month = value[2];
            int day = value[3];

            byte[] stringValue = new byte[value.Length - 4];
            Array.Copy(value, 4, stringValue, 0, stringValue.Length);
     
            if (!Utility.isTimeCheck(year, month, day))
            {
                tTxt_Logs.AppendText("ERROR - READ TIME");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            string str = Encoding.Default.GetString(stringValue).Trim('\0');
            tTxt_Logs.AppendText("Device SN : " + str);
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Expiration Date : "+ year.ToString() + " " + month.ToString() + " " + day.ToString());
            tTxt_Logs.AppendText(Environment.NewLine);
        }

        /*
         * Not Used
         */
         /*
        private void sendUserCertificate()
        {

            OpenFileDialog BrowserDialog = new OpenFileDialog();
            BrowserDialog.InitialDirectory = "C:\\";

            Nullable<bool> fileOpenResult = BrowserDialog.ShowDialog();

            if (fileOpenResult == false)
            {
                tTxt_Logs.AppendText("File was not opened");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }


            string filename = BrowserDialog.FileName;
            byte[] data = System.IO.File.ReadAllBytes(filename);
           
            tTxt_Logs.AppendText("CERT FILE SIZE : " + data.Length.ToString());
            tTxt_Logs.AppendText(Environment.NewLine);


            string userName = tTxt_UserCertficateName.Text;
            string MobileSN = tTxt_MobileSN.Text;
            string ApplicationID = tTxt_ApplicationID.Text;

            byte[] userNameData = new byte[(int)INNCOM_CONF.NUM_USER_NAME];
            byte[] MobileSNData = new byte[(int)INNCOM_CONF.NUM_MOBILE_SN];
            byte[] ApplicationData = new byte[(int)INNCOM_CONF.NUM_APPLICATION_ID];
            byte[] sendData = null;
            int len = userName.Length;
            if (len > (int)INNCOM_CONF.NUM_USER_NAME)
            {
                len = (int)INNCOM_CONF.NUM_USER_NAME;
            }
            Encoding.ASCII.GetBytes(userName, 0, len, userNameData, 0);
            sendData = userNameData;

            len = MobileSN.Length;
            if (len > (int)INNCOM_CONF.NUM_MOBILE_SN)
            {
                len = (int)INNCOM_CONF.NUM_MOBILE_SN;
            }
            Encoding.ASCII.GetBytes(MobileSN, 0, len, MobileSNData, 0);
            sendData = Utility.mergeByteArray(sendData, MobileSNData);



            len = ApplicationID.Length;
            if (len > (int)INNCOM_CONF.NUM_APPLICATION_ID)
            {
                len = (int)INNCOM_CONF.NUM_APPLICATION_ID;
            }
            Encoding.ASCII.GetBytes(ApplicationID, 0, len, ApplicationData, 0);
            sendData = Utility.mergeByteArray(sendData, ApplicationData);


            sendData = Utility.mergeByteArray(sendData, data);

            
             SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_CMD_SEND_USER_CERT, sendData,ref err, 700);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Send User Certificate file");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            tTxt_Logs.AppendText("SENT USER CERT FILE SUCCESSFULLY");
            tTxt_Logs.AppendText(Environment.NewLine);


        }
        */
        /*
         * Need Debug
         */
        private void transferCertificate()
        {
            OpenFileDialog BrowserDialog = new OpenFileDialog();
            BrowserDialog.InitialDirectory = "C:\\";

            Nullable<bool> fileOpenResult = BrowserDialog.ShowDialog();

            if (fileOpenResult == false)
            {
                tTxt_Logs.AppendText("File was not opened");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            string filename = BrowserDialog.FileName;
            byte[] data = System.IO.File.ReadAllBytes(filename);

            tTxt_Logs.AppendText("CERT FILE SIZE : " + data.Length.ToString());
            tTxt_Logs.AppendText(Environment.NewLine);


            COMM_COMMAND_LIST sendCommand;
            if(tCmb_CertificateType.SelectedIndex == 0)
            {
                sendCommand = COMM_COMMAND_LIST.COMM_CMD_TRANSFER_DEVICE_CERT;
            }
            else
            {
                sendCommand = COMM_COMMAND_LIST.COMM_CMD_TRANSFER_CA_CERT;
            }

            SerialPortIO.sendCommand(sendCommand, data, ref err, 1000);
            if(err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - CANNOT TRANSFER CERT FILE");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("TRANSFERED CERT FILE SUCCESSFULLY");
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void verifyCertificate()
        {
            COMM_COMMAND_LIST sendCommand;
            if (tCmb_CertificateType.SelectedIndex == 0)
            {
                sendCommand = COMM_COMMAND_LIST.COMM_CMD_VERIFY_DEVICE_CERT;
            }
            else
            {
                sendCommand = COMM_COMMAND_LIST.COMM_CMD_VERIFY_CA_CERT;
            }
            byte[] result = SerialPortIO.sendCommand(sendCommand, ref err , 700);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - Verify Certification");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            /*
             * Invalid case
             */
            if (result[(int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_COMM_SZ] == 0)
            {
                tTxt_Logs.AppendText("CERT VERIFY STATUS : " + result[(int)PACKET_CONF.COMM_POS_PAYLOAD + (int)PACKET_CONF.COMM_RESPONSE_SZ].ToString());
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }
            tTxt_Logs.AppendText("CERT VERIFY STATUS SUCCESS");
            tTxt_Logs.AppendText(Environment.NewLine);

        }

        private void readDeviceCertificateInfo(ref int TotalSize ,ref int ChunkSize)
        {
            if (tCmb_CertificateType.SelectedIndex == 1)
            {
                tTxt_Logs.AppendText("CA Certificate Cannot read");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            /*
             * Read Device Certifiace Information
             */

            byte[] result = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_COMD_READ_DEVICE_CERT_INFO, ref err, 1000);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxt_Logs.AppendText("ERROR - READ CERTIFICATE INFORMAITON");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            TotalSize = Utility.getU16FromByteA(result, offset + 16);
            ChunkSize = Utility.getU16FromByteA(result, offset + 20);

            tTxt_Logs.AppendText("Total Size : " + TotalSize.ToString());
            tTxt_Logs.AppendText(Environment.NewLine);
            tTxt_Logs.AppendText("Chunk Size : " + ChunkSize.ToString());
            tTxt_Logs.AppendText(Environment.NewLine);

            Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);


        }
        private void downloadCertificate()
        {
            int totalSize = 0;
            int chunkSize = 0;
            //int totalSize = 0x2F0;
            //int chunkSize = 0x3C;
            readDeviceCertificateInfo(ref totalSize, ref chunkSize);
            
            /*
             * Read Device Certifiace Data
             */
            int blockNum = totalSize / chunkSize;
            int chunkAddress = 0;
            int dataLen = 0;

            /*
             * Need Debug Chunk Data how work
             */
            Thread.Sleep(2000);

            byte[] chunkBuffer = new byte[totalSize * 2];

            for (int i = 0; i <= blockNum; i++)
            {
                if (chunkAddress == totalSize)
                {
                    return;
                }

                byte[] payload = new byte[1];
                payload[0] = (byte)i;

                byte[] datas = SerialPortIO.sendCommand(COMM_COMMAND_LIST.COMM_COMD_READ_DEVICE_CERT_DATA, payload, ref err, 800);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    tTxt_Logs.AppendText("ERROR - READ eerpom : " + chunkAddress);
                    tTxt_Logs.AppendText(Environment.NewLine);
                    return;
                }

                dataLen = datas[offset];

                try
                {
                    Array.Copy(datas, offset + 1, chunkBuffer, chunkAddress, dataLen);
                }
                catch
                {
                    tTxt_Logs.AppendText("ERROR - Array Copy Error");
                    tTxt_Logs.AppendText(Environment.NewLine);

                    tTxt_Logs.AppendText("ERROR - READ eerpom : " + chunkAddress);
                    tTxt_Logs.AppendText(Environment.NewLine);
                    return;
                }
                chunkAddress += dataLen;

                tTxt_Logs.AppendText("Read chunk : " + chunkAddress);
                tTxt_Logs.AppendText(Environment.NewLine);
                Dispatcher.Invoke((ThreadStart)(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }

            byte[] certificateData = null;
            if (chunkAddress == totalSize)
            {
                certificateData = new byte[totalSize];

                Array.Copy(chunkBuffer, certificateData, chunkAddress);  
            }
            else
            {
                tTxt_Logs.AppendText("Failed Download");
                tTxt_Logs.AppendText(Environment.NewLine);
                return;
            }

            string certificate = "-----BEGIN CERTIFICATE-----\n" + Convert.ToBase64String(certificateData) + "\n-----END CERTIFICATE-----";
            tTxt_Logs.AppendText(certificate);
            tTxt_Logs.AppendText(Environment.NewLine);

            /*
             * Save Certificate
             */

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Incom Device Certificate"; // Default file name
            dlg.DefaultExt = ".cer"; // Default file extension
            dlg.Filter = "Certificate files (*.cer)|.cer| All fils |*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == false)
            {
                tTxt_Logs.AppendText("File Selected Error");
                tTxt_Logs.AppendText(Environment.NewLine);

            }
            else
            {
                string filename = dlg.FileName;
                System.IO.File.WriteAllText(filename, certificate);
                MessageBox.Show("Successfully saved");

            }
        

        }
    }
}
