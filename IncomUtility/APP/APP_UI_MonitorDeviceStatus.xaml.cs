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
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace IncomUtility
{
    /// <summary>
    /// APP_UI_MonitorDeviceStatus.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_MonitorDeviceStatus : Window
    {
        private List<APP_UI_DataGrid> instrument_status_grid_data_list;
        private List<APP_UI_DataGrid> fault_status_grid_data_list;
        private List<APP_UI_DataGrid> warning_status_grid_data_list;

        Thread timeLock;

        SerialPortIO serial;
        ERROR_LIST err;
        public APP_UI_MonitorDeviceStatus()
        {
            InitializeComponent();

            instrument_status_grid_data_list = new List<APP_UI_DataGrid>();
            fault_status_grid_data_list = new List<APP_UI_DataGrid>();
            warning_status_grid_data_list = new List<APP_UI_DataGrid>();

            instrument_status_grid_data.ItemsSource = instrument_status_grid_data_list;
            fault_status_grid_data.ItemsSource = fault_status_grid_data_list;
            warning_status_grid_data.ItemsSource = warning_status_grid_data_list;

            makeDataGrid(instrument_status_grid_data, instrument_status_grid_data_list, InstrumentStatus);
            makeDataGrid(fault_status_grid_data, fault_status_grid_data_list, FaultStatus);
            makeDataGrid(warning_status_grid_data, warning_status_grid_data_list, WarningStatus);

            tTxtMemo1.AppendText(Environment.NewLine);
        }

        private string[] InstrumentStatus = { "Fault", "Warning", "Gas Alarm 1", "Gas Alarm 2", "Inhibit", "Over-range", "BLE Status" };
        private string[] FaultStatus = {"Internal communication", "Cell Failure (No cell / PWM)","Cell is producing a negative",
            "EEPROM corruption", "uP Op voltage error", "RAM falut","Flash memory fault", "Code memory fault","mA output fault",
            "Supplied voltage Fault","Internal HW Fault", "Internal SW Fault", "Calibration Overdue"};
        private string[] WarningStatus = { "Calibration Overdue", " Temperature limit", "BLE failure (BLE version", "Time/date not set (RTC",
            "Log memory corrupted", "Certificate is corrupted", "Over-range warning", "Under-range warning" };

        private void makeDataGrid(DataGrid grid, List<APP_UI_DataGrid> list, string[] status)
        {
            for (int i = 0; i < status.Length; i++)
            {
                list.Add(new APP_UI_DataGrid(string.Format("Bit" + i + " - " + status[i]), ""));
            }
            for (int i = status.Length; i < 16; i++)
            {
                list.Add(new APP_UI_DataGrid("Bit" + i + " - Reserved", ""));

            }
            grid.Items.Refresh();
        }

        private void makeDataGrid(DataGrid grid, List<APP_UI_DataGrid> list, string[] status, string[] value)
        {
            list.Clear();

            for (int i = 0; i < status.Length; i++)
            {
                list.Add(new APP_UI_DataGrid(string.Format("Bit" + i + " - " + status[i]), value[i]));
            }
            for (int i = status.Length; i < 16; i++)
            {
                list.Add(new APP_UI_DataGrid("Bit" + i + " - Reserved", value[i]));

            }
            grid.Items.Refresh();
        }
        private void updateDataGrid(byte[] u8RXbuffer, DataGrid grid, List<APP_UI_DataGrid> list, string[] status)
        {
            string[] value = new string[16];
            byte currentByte = u8RXbuffer[u8RXbuffer.Length - 4];
            byte pos = 1;

            for (int i = 0; i < 16; i++)
            {
                if (i == 8)
                {
                    currentByte = u8RXbuffer[u8RXbuffer.Length - 5];
                    pos = 1;
                }
                if ((currentByte & pos) == pos)
                    value[i] = "True";
                else
                    value[i] = "False";

                pos <<= 1;
            }

            makeDataGrid(grid, list, status, value);

        }
        private void tBtn_ReadDeviceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (serial == null)
                serial = new SerialPortIO();
            
            if (!serial.isPortOpen())
            {
                tTxtMemo1.AppendText("Incom is not connected");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            /* 
             * Read Device Status
             */
            byte[] u8RXbuffer = serial.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_DEVICE_STATUS,ref err);
            if (err != ERROR_LIST.ERROR_NONE ) {
                tTxtMemo1.AppendText("ERROR - READ INCOM STATUS");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            updateDataGrid(u8RXbuffer, instrument_status_grid_data, instrument_status_grid_data_list, InstrumentStatus);

            /*
             * Read Warning Status
             */
            byte[] cmd_ReadFaultOption = { 0x01 };
            u8RXbuffer = serial.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_FAULT_DETAILS,cmd_ReadFaultOption,ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxtMemo1.AppendText("ERROR : READ WARNING DETAILS");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            updateDataGrid(u8RXbuffer, warning_status_grid_data, warning_status_grid_data_list, WarningStatus);

            /*
             * Read Fault Status
             */
            cmd_ReadFaultOption[0] = 0x02;
            u8RXbuffer = serial.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_FAULT_DETAILS,cmd_ReadFaultOption,ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxtMemo1.AppendText("ERROR : READ FAULT DETAILS");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            updateDataGrid(u8RXbuffer, fault_status_grid_data, fault_status_grid_data_list, FaultStatus);
        }

        private void tBtn_ReadSwitchStauts_Click(object sender, RoutedEventArgs e)
        {
            if (serial == null)
                serial = new SerialPortIO();

            if (!serial.isPortOpen())
            {
                tTxtMemo1.AppendText("Incom is not connected");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            /*
            *Read Switch Status
            */
            byte[] u8RXbuffer = serial.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_SWITCH_STATUS,ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxtMemo1.AppendText("ERROR - READ SWITCH STATUS");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            if(u8RXbuffer ==null)
            switch (u8RXbuffer[u8RXbuffer.Length - 4])
            {
                case 0:
                    tCmb_UpButton.SelectedIndex = 0;
                    tCmb_DownButton.SelectedIndex = 0;
                    break;
                case 1:
                    tCmb_UpButton.SelectedIndex = 0;
                    tCmb_DownButton.SelectedIndex = 1;
                    break;
                case 2:
                    tCmb_UpButton.SelectedIndex = 1;
                    tCmb_DownButton.SelectedIndex = 0;
                    break;
                case 3:
                    tCmb_UpButton.SelectedIndex = 1;
                    tCmb_DownButton.SelectedIndex = 1;
                    break;
            }

            /*
            *Read Inhibit Switch
            */
            u8RXbuffer = serial.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_INHIBIT_SWITCH,ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxtMemo1.AppendText("ERROR - READ INHIBIT SWITCH STATUS");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            if (u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3] < 2)
                tCmb_InhibitSwitch.SelectedIndex = u8RXbuffer[u8RXbuffer.Length - 4];

            /*
             * Read SInk/Source Switch
             */ 
            u8RXbuffer = serial.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_ANALOGUE_OUTPUT,ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxtMemo1.AppendText("ERROR - READ ANLOGUE TYPE");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            tCmb_SinkSourceSwitchs.SelectedIndex = u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
        }

        private void tBtn_ReadTime_Click(object sender, RoutedEventArgs e)
        {
            if (serial == null)
                serial = new SerialPortIO();

            if (!serial.isPortOpen())
            {
                tTxtMemo1.AppendText("Incom is not connected");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            /*
             *  Read Time
             */
            byte[] u8RXbuffer = serial.sendCommand(COMM_COMMAND_LIST.COMM_CMD_READ_TIME,ref err);

            if (err != ERROR_LIST.ERROR_NONE)
            {
                tTxtMemo1.AppendText("ERROR - READ TIME");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            try
            {
                int year = (int)u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3] << 8 | (int)(u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 4]);
                int month = u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 5];
                int day = u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 6];
                int hour = u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 7];
                int minute = u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 8];
                int second = u8RXbuffer[(int)PACKET_CONF.COMM_POS_PAYLOAD + 9];
                
                if (!isTimeCheck(year, month, day, hour, minute, second))
                {
                    tTxtMemo1.AppendText("ERROR - READ TIME");
                    tTxtMemo1.AppendText(Environment.NewLine);
                    return;
                }

                DateTime readingTime = new DateTime(year, month, day, hour, minute, second);
                tDate_DatePicker.SelectedDate = readingTime;
                tTime_TimePicker.Value = readingTime;
            }
            catch(IndexOutOfRangeException)
            {
                tTxtMemo1.AppendText("ERROR - READ TIME - INDEX OUT RANGE");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
        }

        private void tBtn_SetTime_Click(object sender, RoutedEventArgs e)
        {
            timeLock = new Thread(setTime);
            timeLock.Start();
            Thread.Sleep(50);     
        }
        private void setTime()
        {
            if (serial == null)
                serial = new SerialPortIO();

            if (!serial.isPortOpen())
            {
                tTxtMemo1.AppendText("Incom is not connected");
                tTxtMemo1.AppendText(Environment.NewLine);
                return;
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                tBtn_SetTime.IsEnabled = false;
                tBtn_ReadTime.IsEnabled = false;

                byte[] timeToSend = new byte[7];

                DateTime setDate = tDate_DatePicker.SelectedDate.Value;
                DateTime setTime = (DateTime)tTime_TimePicker.Value;

                int year = setDate.Year;
                timeToSend[0] = (byte)(year >> 8);
                timeToSend[1] = (byte)year;
                timeToSend[2] = (byte)setDate.Month;
                timeToSend[3] = (byte)setDate.Day;
                timeToSend[4] = (byte)setTime.Hour;
                timeToSend[5] = (byte)setTime.Minute;
                timeToSend[6] = (byte)setTime.Second;

                serial.sendCommand(COMM_COMMAND_LIST.COMM_CMD_WRITE_TIME, timeToSend, ref err,300);

                if (err == ERROR_LIST.ERROR_NONE)
                    tTxtMemo1.AppendText("Successfully wrote the datetime");
                else
                {
                    tTxtMemo1.AppendText("ERROR - WRITE TIME");
                    tTxtMemo1.AppendText(Environment.NewLine);
                    tTxtMemo1.AppendText(err.ToString());
                }

                tTxtMemo1.AppendText(Environment.NewLine);
            }));

            Thread.Sleep(500);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                tBtn_ReadTime.IsEnabled = true;
                tBtn_SetTime.IsEnabled = true;
            }));

        }
        private bool isTimeCheck(int year, int month,int day, int hour, int minute, int second)
        {
            if (year < 999 || year > 10000)
                return false;

            if (month < 1 || month > 12)
                return false;

            if (day < 1 || day > 31)
                return false;

            if (hour < 0 || hour > 24)
                return false;

            if (minute < 0 || minute > 60)
                return false;

            if (second < 0 || second > 60)
                return false;

            return true;
        }
    }
}
