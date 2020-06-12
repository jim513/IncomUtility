using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace IncomUtility
{
    /// <summary>
    /// APP_UI_Communication.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_Communication : Window
    {
        public APP_UI_Communication()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(initSerialPort);
        }
    
        private void btnCommunicationOk_Click(object sender, RoutedEventArgs e)
        {
            communucationWithSerial();         
        }

        private void communucationWithSerial()
        {
            if (cmbPort.SelectedItem == null)
                return;

            SerialPortIO.mutex.WaitOne();

            if (SerialPortIO.serialPort.IsOpen)
            {
                SerialPortIO.serialPort.Close();
            }

            SerialPortIO.serialPort.PortName = cmbPort.Text;
            SerialPortIO.serialPort.BaudRate = Convert.ToInt32(cmbBaudRate.Text);
            SerialPortIO.serialPort.DataBits = Convert.ToInt32(cmbDataBits.Text);

            switch (cmbStopBits.SelectedIndex)
            {
                case 0:
                    {
                        SerialPortIO.serialPort.StopBits = StopBits.One;
                        break;
                    }
                case 1:
                    {
                        SerialPortIO.serialPort.StopBits = StopBits.OnePointFive;
                        break;
                    }
                case 2:
                    {
                        SerialPortIO.serialPort.StopBits = StopBits.Two;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            switch (cmbParity.SelectedIndex)
            {
                case 0:
                    {
                        SerialPortIO.serialPort.Parity = Parity.None;
                        break;
                    }
                case 1:
                    {
                        SerialPortIO.serialPort.Parity = Parity.Odd;
                        break;
                    }
                case 2:
                    {
                        SerialPortIO.serialPort.Parity = Parity.Even;
                        break;
                    }
                case 3:
                    {
                        SerialPortIO.serialPort.Parity = Parity.Mark;
                        break;
                    }
                case 4:
                    {
                        SerialPortIO.serialPort.Parity = Parity.Space;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            try
            {
                SerialPortIO.serialPort.Open();
            }
            catch
            {
                SerialPortIO.mutex.ReleaseMutex();
                MessageBox.Show("연결에 실패하였습니다");
                return;
            }

            SerialPortIO.mutex.ReleaseMutex();

            this.Close();
        }
        void initSerialPort(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cmbPort.Items.Add(port);
            }
            if (ports.Length > 0)
            {
                cmbPort.SelectedIndex = 0;
            }
        }

    }
}
