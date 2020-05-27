using System;
using System.Collections.Generic;
using System.IO;
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
    /// ViewLogs.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APP_UI_ViewLogs : Window
    {
        ERROR_LIST err;

        private const int grid_row_max = 300;
        private List<APP_UI_DataGrid> logCalList, logAlarmList, logFaultList, logWarningList, logReflexList, logInfoList;
     
        private string[] logCalHeader = new string[] { "Log No.","Time","Cal Type","Conc before Cal","Conc after Cal",
              "Raw cell signal","Cal Factor before","Cal Factor after", "Target Cal gas", "Correction factor", "Cal Result","Cylinder No."};
        private string[] logDefaultHeader = new string[] { "Log No.", "Time", "Event type", "Event ID", "Event Data 1", "Event Data 2", "Description" };
        private string[] logReflexHeader = new string[] { "Log No.", "Event type", "Event ID", "ADC 1", "ADC 2" };

        string[] LOG_TABLE_TYPE = { "", "Alarm", "Warning", "Fault", "Info", "Calibration", "Reflex" };
        string[] CAL_TYPE = {"", "Factory Zero Cal", "Factory Span Cal", "User Zero Cal", "User Span Cal","Analogue Out Zero Cal",
            "Analogue Out Span Cal","Voltage Out Zero Cal", "Voltage Out Span Cal", "Cell Drive Cal (PWM)"};
        string[] ALARM_TYPE = { "","Low alarm occurred",
        "High alarm occurred","Low alarm clears","High alarm clears", "Gas alarm 3 occurred","Gas alarm 3 clears" };
        string[] FAULT_TPYE = {"","Internal communication failure","Cell failure","Cell is producing a negative reading","EEPROM is corrupted",
            "MCU operating voltage failure","RAM read/write fault", "Flash memory corrupted","Code memory failure", "mA output failure", "Supplied voltage failure",
            "Internal HW Fault","Internal SW Fault","Calibration Overdue", "Fault clears"};
        string[] INFO_TYPE ={"","Instrument power on event", "Zero calibration successful","Zero calibration failed", "Span calibration successful",
            "Span calibration failed","Sensor replaced","Reset alarms and faults", "Gas configuration changed", "Time/date adjusted (RTC adjusted)", "Log memory is full",
            "BLE connection has been established", "Adhoc connection has been established","BLE connection is terminated","Enter inhibit mode",
            "Exit inhibit mode","Cleared log memory" };
        string[] WARNING_TYPE ={"","Calibration Overdue","Temperature limit exceeded sensor specification","BLE failure (BLE version only)",
            "Time/date not set (RTC not set)","Log memory corrupted (CRC not matched)","Certificate is corrupted","Over-range warning","Under-range warning",
            "Warning self-clears"   };

        const int NUMBER_OF_ALARM = 6;
        const int NUMBER_OF_WARNING = 9;
        const int NUMBER_OF_FAULT = 14;
        const int NUMBER_OF_INFO = 16;
        const int NUMBER_OF_TABLE_TYPE = 6;
        const int NUMBER_OF_CAL_TYPE = 9;

        //private LogToFile alarmLog;
        //private LogToFile faultLog;
        //private LogToFile warningLog;
        //private LogToFile infoLog;
        //private LogToFile calLog;
        //private LogToFile reflexLog;
        //private LogToFile[] logFiles;
        private LogToFile logFiles;
        Thread LogMonitor;
        private bool LogMonitorRunning = false;
        public APP_UI_ViewLogs()
        {
            InitializeComponent();

            MakeLogGrid();
        }
        private void MakeLogGrid()
        {
            logCalList = new List<APP_UI_DataGrid>();
            logAlarmList = new List<APP_UI_DataGrid>();
            logFaultList = new List<APP_UI_DataGrid>();
            logWarningList = new List<APP_UI_DataGrid>();
            logReflexList = new List<APP_UI_DataGrid>();
            logInfoList = new List<APP_UI_DataGrid>();

            //logFiles = new LogToFile[LOG_TABLE_TYPE.Length];
            //alarmLog = new LogToFile("Incom_Utility_Alarm_Log", "csv");
            //faultLog = new LogToFile("Incom_Utility_Fault_Log", "csv");
            //warningLog = new LogToFile("Incom_Utility_Warining_Log", "csv");
            //infoLog = new LogToFile("Incom_Utility_Info_Log", "csv");
            //calLog = new LogToFile("Incom_Utility_Calibration_Log", "csv");
            //reflexLog = new LogToFile("Incom_Utility_Reflex_Log", "csv");

            grid_LogCal.ItemsSource = logCalList;
            grid_LogAlarm.ItemsSource = logAlarmList;
            grid_LogFault.ItemsSource = logFaultList;
            grid_LogInfo.ItemsSource = logInfoList;
            grid_LogWarning.ItemsSource = logWarningList;
            grid_LogRelfex.ItemsSource = logReflexList;

            MakeLogGridHeader(grid_LogCal, logCalHeader);
            MakeLogGridHeader(grid_LogAlarm, logDefaultHeader);
            MakeLogGridHeader(grid_LogFault, logDefaultHeader);
            MakeLogGridHeader(grid_LogWarning, logDefaultHeader);
            MakeLogGridHeader(grid_LogRelfex, logReflexHeader);
            MakeLogGridHeader(grid_LogInfo, logDefaultHeader);
        }
        private void MakeLogGridHeader(DataGrid grid, string[] data_name)
        {
            for (int i = 0; i < data_name.Length; i++)
                grid.Columns[i].Header = data_name[i];
        }

        private void UpdateLogGrid(DataGrid grid, List<APP_UI_DataGrid> list,
                               int index, string time, string data1, float data2, float data3, float data4, float data5, float data6, float data7, float data8,
                               string data9, string data10)
        {
            if (grid.Items.Count >= grid_row_max)
            {
                list.RemoveAt(0);
            }

            list.Add(new APP_UI_DataGrid(string.Format("{0:#,0}",index), time,  data1, string.Format("{0:#,0.000}", data2),
                                        string.Format("{0:#,0}", data3), string.Format("{0:#,0.000}", data4),
                                        string.Format("{0:#,0.000}", data5), string.Format("{0:#,0.000}", data6),
                                        string.Format("{0:#,0}", data7), string.Format("{0:#,0}", data8),
                                        data9, data10
                                        ));
            grid.Items.Refresh();

            if (grid.Items.Count > 0)
            {
                grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
            }
        }
        private void UpdateLogGrid(DataGrid grid, List<APP_UI_DataGrid> list, int index, string time, string data1, byte data2, float data3, float data4, string data5)
        {
            if (grid.Items.Count >= grid_row_max)
            {
                list.RemoveAt(0);
            }
            list.Add(new APP_UI_DataGrid(string.Format("{0:#,0}", index), time, data1, string.Format("{0:#,0}", data2),
                                                string.Format("{0:#,0}", data3), string.Format("{0:#,0.000}", data4),
                                                 data5));
            grid.Items.Refresh();

            if (grid.Items.Count > 0)
            {
                grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
            }
        }
        private void UpdateLogGrid(DataGrid grid, List<APP_UI_DataGrid> list, int index,  string data1, string data2, float data3, float data4)
        {
            if (grid.Items.Count >= grid_row_max)
            {
                list.RemoveAt(0);
            }
            list.Add(new APP_UI_DataGrid(string.Format("{0:#,0}", index), data1,  data2,
                                                string.Format("{0:#,0.000}", data3), string.Format("{0:#,0.000}", data4)));
            grid.Items.Refresh();

            if (grid.Items.Count > 0)
            {
                grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
            }
        }

        private void tCmb_LogsType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LogTab == null)
                return;
            LogTab.SelectedIndex = tCmb_LogsType.SelectedIndex;
        }

        private void tBtn_DownloadLogs_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
            {
                return;
            }

            DateTime currentTime = DateTime.Now;
            /*
             * Read Log Info
             */
            byte[] eventType = new byte[1];
            eventType[0] = (byte)(tCmb_LogsType.SelectedIndex + 1);
            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_LOG_INFO, eventType, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read Log Info");
                return;
            }
            int pos = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            int numLogdata = result[pos] * 256 + result[pos + 1];

            if (numLogdata < 1)
            {
                MessageBox.Show("Number of log is zero");
                return;
            }

            tBtn_DownloadLogs.IsEnabled = false;
            tBtn_StopDownload.IsEnabled = true;

            tPbar_LogDownBar.Maximum = numLogdata;

            LogMonitorRunning = true;

            LogMonitor = new Thread(() => DownloadLog(eventType[0], numLogdata));
            LogMonitor.Start();

        }

        private void DownloadLog(byte eventType, int numLogdata)
        {
            /*
             * Read Log Data
             */
            byte[] payload = new byte[3];
            payload[0] = eventType;
            for (int index = 0; index < numLogdata; index++)
            {
                if (!LogMonitorRunning)
                    return;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tPbar_LogDownBar.Value = index + 1;
                }));
                payload[1] = (byte)(index >> 8);
                payload[2] = (byte)index;

                byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_LOG_DATA, payload, ref err, 300);
                if (err != ERROR_LIST.ERROR_NONE)
                {
                    MessageBox.Show("Index : " + index.ToString() + "  ERROR - Read Log Data");
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        tBtn_DownloadLogs.IsEnabled = true;
                        tBtn_StopDownload.IsEnabled = false;
                    }));
                    continue;
                }
                int logDataLen = result[(int)PACKET_CONF.COMM_POS_PAYLOAD + 3];
                byte[] logData = new byte[logDataLen];
                Array.Copy(result, (int)PACKET_CONF.COMM_POS_PAYLOAD + 4, logData, 0, logDataLen);

                switch (eventType)
                {
                    case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_CALIBRATION:
                        UpdateLogCal(logData, index);
                        break;
                    case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_ALARM:
                        UpdateLogEvent(logData, index);
                        break;
                    case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_FAULT:
                        UpdateLogEvent(logData, index);
                        break;
                    case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_WARNING:
                        UpdateLogEvent(logData, index);
                        break;
                    case(int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_REFLEX:
                        UpdateLogReflex(logData, index);
                        break;
                    case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_INFO:
                        UpdateLogEvent(logData, index);
                        break;
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                tBtn_DownloadLogs.IsEnabled = true;
                tBtn_StopDownload.IsEnabled = false;
            }));
        }


        private void tBtn_ReadLogInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
                return;

            /*
             * Read Log Info
             */
            byte[] eventType = new byte[1];
            eventType[0] = (byte)(tCmb_LogsType.SelectedIndex + 1);
            byte[] result = SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_LOG_INFO, eventType, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read Log Info");
                return;
            }
            int pos = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;
            int savedCount = result[pos] * 256 + result[pos + 1];
            int maxCount = result[pos + 2] * 256 + result[pos + 3];
            int size = result[pos + 4];
            string str = "Log Type : " + eventType[0].ToString() + ", Number of log : " + savedCount;
            str += " , Total number of log : " + maxCount + " , Size of log : " + size + " Bytes";

            MessageBox.Show(str);
        }

       

        private void tBtn_SaveToCSV_Click(object sender, RoutedEventArgs e)
        {
            int eventType = tCmb_LogsType.SelectedIndex;
            List<APP_UI_DataGrid> gridList;
            DataGrid grid;
            switch (eventType)
            {
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_CALIBRATION:
                    logFiles = new LogToFile("Incom_Utility_Calibration_Log", "csv");
                    grid = grid_LogCal;
                    gridList = logCalList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_ALARM:
                    logFiles = new LogToFile("Incom_Utility_Alarm_Log", "csv");
                    grid = grid_LogAlarm;
                    gridList = logAlarmList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_FAULT:
                    logFiles = new LogToFile("Incom_Utility_Fault_Log", "csv");
                    grid = grid_LogFault;
                    gridList = logFaultList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_WARNING:
                    logFiles = new LogToFile("Incom_Utility_Warining_Log", "csv");
                    grid = grid_LogWarning;
                    gridList = logWarningList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_REFLEX:
                    logFiles = new LogToFile("Incom_Utility_Reflex_Log", "csv");
                    grid = grid_LogRelfex;
                    gridList = logReflexList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_INFO:
                    logFiles = new LogToFile("Incom_Utility_Info_Log", "csv");
                    grid = grid_LogInfo;
                    gridList = logInfoList;
                    break;
                default:
                    return;
            }

            string filePath = logFiles.Checked();
            
            if (filePath == null)
                return;
           
            string strHeader = "";
            for (int i = 0;i< grid.Columns.Count; i++)
            {
                if( i == grid.Columns.Count -1)
                {
                    strHeader += grid.Columns[i].Header.ToString();
                    break;
                }
                strHeader += grid.Columns[i].Header.ToString() +",";
            }
            logFiles.SetDataHeader("Time," + strHeader);

            //MessageBox.Show("DataGrid Items count :" + grid.Items.Count);
            //MessageBox.Show("DataGrid Items count2 :" + gridList.Count);
            //MessageBox.Show("DataGrid Items count3 :" + gridList.);
            for (int i=0; i<3; i++)
            {
                string strWrite = "";
                for (int j = 0; j < grid.Columns.Count; i++)
                {
                    if (j == grid.Columns.Count - 1)
                    {
                        strWrite += gridList.ToString();
                        break;
                    }
                    strWrite += gridList[i].ToString() + ", ";
                }
                MessageBox.Show(strWrite);
            }
        }

        private void tBtn_ClrLog_Click(object sender, RoutedEventArgs e)
        {
            if (!SerialPortIO.isPortOpen())
                return;
            /*
             * Clear Log Data
             */
            byte[] payload = new byte[2];
            payload[0] = (byte)(tCmb_LogsType.SelectedIndex + 1);
            if ((bool)tChb_RecordClrLog.IsChecked)
                payload[1] = 1;

            SerialPortIO.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_CLR_LOG_DATA, payload, ref err, 800);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Clear Log Data");
                return;
            }

            MessageBox.Show("Cleared logs - " + LOG_TABLE_TYPE[payload[0]].ToString(),"Information" );
        }


        private void tBtn_StopDownload_Click(object sender, RoutedEventArgs e)
        {
            if (LogMonitorRunning)
            {
                LogMonitorRunning = false;

                tBtn_DownloadLogs.IsEnabled = true;
                tBtn_StopDownload.IsEnabled = false;
            }
        }

        void UpdateLogCal(byte[] logData, int index)
        {
            ushort u16TimeStampYear;
            byte u8TimeStampMon, u8TimeStampDay, u8TimeStampHou, u8TimeStampMin, u8TimeStampSec;
            
            byte u8CalibrationType;
            float f32GasReadingBeforeCal;
            float f32GasReadingAfterCal;
            float f32RawSignalBeforeCal;
            float f32CalFactorBeforeCal;
            float f32CalFactorAfterCal;
            float f32TargetGasConcentration;
            float f32CorrectionFactor;
            byte u8CalibrationResults;
            byte[] au8CylinderNo;
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(logData);
                int offset = (int)INNCOM_COMMAND_LIST.NUM_CYLINDER_SN;

                u16TimeStampYear = BitConverter.ToUInt16(logData, offset + 35);
                u8TimeStampMon = logData[offset + 34];
                u8TimeStampDay = logData[offset + 33];
                u8TimeStampHou = logData[offset + 32];
                u8TimeStampMin = logData[offset + 31];
                u8TimeStampSec = logData[offset + 30];

                u8CalibrationType = logData[offset + 29];
                f32GasReadingBeforeCal = BitConverter.ToSingle(logData, offset + 25);
                f32GasReadingAfterCal = BitConverter.ToSingle(logData, offset + 21);
                f32RawSignalBeforeCal = BitConverter.ToSingle(logData, offset + 17);
                f32CalFactorBeforeCal = BitConverter.ToSingle(logData, offset + 13);
                f32CalFactorAfterCal = BitConverter.ToSingle(logData, offset + 9);
                f32TargetGasConcentration = BitConverter.ToSingle(logData, offset + 5);
                f32CorrectionFactor = BitConverter.ToSingle(logData, offset + 1);
                u8CalibrationResults = logData[offset];
                au8CylinderNo = new byte[(int)INNCOM_COMMAND_LIST.NUM_CYLINDER_SN];
                for (int i = offset - 1; i >= 0; i--)
                    au8CylinderNo[i] = logData[offset - 1 - i];
            }
            else
            {
                int offset = 0;

                u16TimeStampYear = BitConverter.ToUInt16(logData, offset);
                u8TimeStampMon = logData[offset + 2];
                u8TimeStampDay = logData[offset + 3];
                u8TimeStampHou = logData[offset + 4];
                u8TimeStampMin = logData[offset + 5];
                u8TimeStampSec = logData[offset + 6];

                u8CalibrationType = logData[offset + 7];
                f32GasReadingBeforeCal = BitConverter.ToSingle(logData, offset + 8);
                f32GasReadingAfterCal = BitConverter.ToSingle(logData, offset + 12);
                f32RawSignalBeforeCal = BitConverter.ToSingle(logData, offset + 12);
                f32CalFactorBeforeCal = BitConverter.ToSingle(logData, offset + 16);
                f32CalFactorAfterCal = BitConverter.ToSingle(logData, offset + 20);
                f32TargetGasConcentration = BitConverter.ToSingle(logData, offset + 24);
                f32CorrectionFactor = BitConverter.ToSingle(logData, offset + 28);
                u8CalibrationResults = logData[offset + 32];
                au8CylinderNo = new byte[(int)INNCOM_COMMAND_LIST.NUM_CYLINDER_SN];
                for (int i = 0; i < logData.Length - offset - 33; i++)
                    au8CylinderNo[i] = logData[offset + 33 + i];
            }
            string time_str = u16TimeStampYear.ToString() + '/' + u8TimeStampMon.ToString("D2") + '/' + u8TimeStampDay.ToString("D2") + ' ';
            time_str += u8TimeStampHou.ToString("D2") + ':' + u8TimeStampMin.ToString("D2") + ':' + u8TimeStampSec.ToString("D2");

            string result_str = "";
            if (u8CalibrationResults == 0)
                result_str = "Fail - " + u8CalibrationResults.ToString();
            else
                result_str = "Pass - " + u8CalibrationResults.ToString();

            string cylinder_str = Encoding.Default.GetString(au8CylinderNo).Trim('\0');

            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateLogGrid(grid_LogCal, logCalList, index + 1, time_str, CAL_TYPE[(int)u8CalibrationType], f32GasReadingBeforeCal, f32GasReadingAfterCal,
                    f32RawSignalBeforeCal, f32CalFactorBeforeCal, f32CalFactorAfterCal, f32TargetGasConcentration, f32CorrectionFactor, result_str, cylinder_str);
            }));
        }
        private void UpdateLogEvent(byte[] logData, int index)
        {
            ushort u16TimeStampYear;
            byte u8TimeStampMon, u8TimeStampDay, u8TimeStampHou, u8TimeStampMin, u8TimeStampSec;

            byte u8EventType;
            byte u8EventId ;
            float f32GasReading;
            float f32EventData ;

            DataGrid grid;
            List<APP_UI_DataGrid> list;
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(logData);
                int offset = 0;

                u16TimeStampYear = BitConverter.ToUInt16(logData, offset+15);
                u8TimeStampMon = logData[offset + 14];
                u8TimeStampDay = logData[offset + 13];
                u8TimeStampHou = logData[offset + 12];
                u8TimeStampMin = logData[offset + 11];
                u8TimeStampSec = logData[offset + 10];

                u8EventType = logData[offset + 9];
                u8EventId = logData[offset + 8];
                f32GasReading = BitConverter.ToSingle(logData, offset + 4);
                f32EventData = BitConverter.ToSingle(logData, offset);
            }
            else
            {
                int offset = 0;

                u16TimeStampYear = BitConverter.ToUInt16(logData, offset);
                u8TimeStampMon = logData[offset + 2];
                u8TimeStampDay = logData[offset + 3];
                u8TimeStampHou = logData[offset + 4];
                u8TimeStampMin = logData[offset + 5];
                u8TimeStampSec = logData[offset + 6];

                u8EventType = logData[offset + 7];
                u8EventId = logData[offset + 8];
                f32GasReading = BitConverter.ToSingle(logData, offset + 9);
                f32EventData = BitConverter.ToSingle(logData, offset + 13);
            }

            string time_str = u16TimeStampYear.ToString() + '/' + u8TimeStampMon.ToString("D2") + '/' + u8TimeStampDay.ToString("D2") + ' ';
            time_str += u8TimeStampHou.ToString("D2") + ':' + u8TimeStampMin.ToString("D2") + ':' + u8TimeStampSec.ToString("D2");

            string event_str = "Unknown - " + u8EventType.ToString();
            if (u8EventType > 0 && u8EventType <= NUMBER_OF_TABLE_TYPE)
                event_str = LOG_TABLE_TYPE[u8EventType];

            string type_str = "Unknown - " + u8EventId.ToString();
            switch (u8EventType)
            {
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_ALARM:
                    if (u8EventId > 0 && u8EventId <= NUMBER_OF_ALARM)
                        type_str = ALARM_TYPE[u8EventId];
                    grid = grid_LogAlarm;
                    list = logAlarmList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_FAULT:
                    if (u8EventId > 0 && u8EventId <= NUMBER_OF_FAULT)
                        type_str = FAULT_TPYE[u8EventId];
                    grid = grid_LogFault;
                    list = logFaultList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_WARNING:
                    if (u8EventId > 0 && u8EventId <= NUMBER_OF_WARNING)
                        type_str = WARNING_TYPE[u8EventId];
                    grid = grid_LogWarning;
                    list = logWarningList;
                    break;
                case (int)INNCOM_COMMAND_LIST.LOG_TABLE_TYPE_INFO:
                    if (u8EventId > 0 && u8EventId <= NUMBER_OF_INFO)
                        type_str = INFO_TYPE[u8EventId];
                    grid = grid_LogInfo;
                    list = logInfoList;
                    break;
                default:
                    return;
            }      
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateLogGrid(grid, list, index + 1, time_str, event_str, u8EventId, f32GasReading, f32EventData, type_str);
            }));

        }
        void UpdateLogReflex(byte[] logData, int index)
        {
            float f32Adc1;
            float f32Adc2;
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(logData);
                int offset = 0;
                
                f32Adc1 = BitConverter.ToSingle(logData, offset + 4);
                f32Adc2 = BitConverter.ToSingle(logData, offset); ;
            }
            else
            {
                int offset = 0;

                f32Adc1 = BitConverter.ToSingle(logData, offset + 4);
                f32Adc2 = BitConverter.ToSingle(logData, offset); ;
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateLogGrid(grid_LogRelfex, logReflexList, index + 1, "tReflex Log", "-", f32Adc1, f32Adc2);
            }));
        }
    }
}
