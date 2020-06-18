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
    public enum NUMBER_OF_TABLE
    {   
        NUMBER_OF_ALARM = 6,
        NUMBER_OF_WARNING = 9,
        NUMBER_OF_FAULT = 14,
        NUMBER_OF_INFO = 16,
        NUMBER_OF_TABLE_TYPE = 6,
        NUMBER_OF_CAL_TYPE = 9,
    }
    public partial class APP_UI_ViewLogs : Window
    {
        ERROR_LIST err = ERROR_LIST.ERROR_NONE;

        private const int grid_row_max = 300;
        private List<UIDataGrid> logCalList, logAlarmList, logFaultList, logWarningList, logReflexList, logInfoList;
     
        private string[] logCalHeader = 
        { 
            "Log No.",
            "Time",
            "Cal Type",
            "Conc before Cal",
            "Conc after Cal",
            "Raw cell signal",
            "Cal Factor before",
            "Cal Factor after", 
            "Target Cal gas", 
            "Correction factor", 
            "Cal Result",
            "Cylinder No."
        };
        private string[] logDefaultHeader = 
        { 
            "Log No.", 
            "Time", 
            "Event type", 
            "Event ID", 
            "Event Data 1", 
            "Event Data 2",
            "Description" 
        };
        private string[] logReflexHeader =
        { 
            "Log No.", 
            "Event type", 
            "Event ID", 
            "ADC 1",
            "ADC 2" 
        };

        private string[] LOG_TABLE_TYPE_STRINGA =
        { 
            "", 
            "Alarm", 
            "Warning",
            "Fault", 
            "Info", 
            "Calibration",
            "Reflex"
        };
        private string[] CAL_TYPE = 
        {
            "", 
            "Factory Zero Cal",
            "Factory Span Cal", 
            "User Zero Cal",
            "User Span Cal",
            "Analogue Out Zero Cal",
            "Analogue Out Span Cal",
            "Voltage Out Zero Cal", 
            "Voltage Out Span Cal", 
            "Cell Drive Cal (PWM)"
        };
        private string[] ALARM_TYPE =
        { 
            "",
            "Low alarm occurred",
            "High alarm occurred",
            "Low alarm clears",
            "High alarm clears",
            "Gas alarm 3 occurred",
            "Gas alarm 3 clears" 
        };
        private string[] FAULT_TPYE = 
        {
            "",
            "Internal communication failure",
            "Cell failure",
            "Cell is producing a negative reading",
            "EEPROM is corrupted",
            "MCU operating voltage failure",
            "RAM read/write fault", 
            "Flash memory corrupted",
            "Code memory failure", 
            "mA output failure", 
            "Supplied voltage failure",
            "Internal HW Fault",
            "Internal SW Fault",
            "Calibration Overdue", 
            "Fault clears"
        };
        private string[] INFO_TYPE =
        {
            "",
            "Instrument power on event", 
            "Zero calibration successful",
            "Zero calibration failed", 
            "Span calibration successful",
            "Span calibration failed",
            "Sensor replaced",
            "Reset alarms and faults",
            "Gas configuration changed", 
            "Time/date adjusted (RTC adjusted)", 
            "Log memory is full",
            "BLE connection has been established", 
            "Adhoc connection has been established",
            "BLE connection is terminated",
            "Enter inhibit mode",
            "Exit inhibit mode",
            "Cleared log memory" 
        };
        private string[] WARNING_TYPE =
        {
            "",
            "Calibration Overdue",
            "Temperature limit exceeded sensor specification",
            "BLE failure (BLE version only)",
            "Time/date not set (RTC not set)",
            "Log memory corrupted (CRC not matched)",
            "Certificate is corrupted",
            "Over-range warning",
            "Under-range warning",
            "Warning self-clears"   };


        private LogToFile logFiles = null;

        Thread LogMonitor = null;

        private bool LogMonitorRunning = false;

        public APP_UI_ViewLogs()
        {
            InitializeComponent();

            makeLogGrid();
        }
        
        private void makeLogGrid()
        {
            logCalList = new List<UIDataGrid>();
            logAlarmList = new List<UIDataGrid>();
            logFaultList = new List<UIDataGrid>();
            logWarningList = new List<UIDataGrid>();
            logReflexList = new List<UIDataGrid>();
            logInfoList = new List<UIDataGrid>();

            grid_LogCal.ItemsSource = logCalList;
            grid_LogAlarm.ItemsSource = logAlarmList;
            grid_LogFault.ItemsSource = logFaultList;
            grid_LogInfo.ItemsSource = logInfoList;
            grid_LogWarning.ItemsSource = logWarningList;
            grid_LogRelfex.ItemsSource = logReflexList;

            makeLogGridHeader(grid_LogCal, logCalHeader);
            makeLogGridHeader(grid_LogAlarm, logDefaultHeader);
            makeLogGridHeader(grid_LogFault, logDefaultHeader);
            makeLogGridHeader(grid_LogWarning, logDefaultHeader);
            makeLogGridHeader(grid_LogRelfex, logReflexHeader);
            makeLogGridHeader(grid_LogInfo, logDefaultHeader);
        }
        
        private void makeLogGridHeader(DataGrid grid, string[] data_name)
        {
            for (int i = 0; i < data_name.Length; i++)
                grid.Columns[i].Header = data_name[i];
        }

        private void updateLogGrid(DataGrid grid, List<UIDataGrid> list,
                               int index, string time, string data1, float data2, float data3, float data4, float data5, float data6, float data7, float data8,
                               string data9, string data10)
        {
            if (grid.Items.Count >= grid_row_max)
            {
                list.RemoveAt(0);
            }

            list.Add(new UIDataGrid(string.Format("{0}",index), time,  data1, string.Format("{0:f2}", data2),
                                        string.Format("{0:f1}", data3), string.Format("{0:f2}", data4),
                                        string.Format("{0:f2}", data5), string.Format("{0:f2}", data6),
                                        string.Format("{0:f1}", data7), string.Format("{0:f1}", data8),
                                        data9, data10
                                        ));
            grid.Items.Refresh();

            if (grid.Items.Count > 0)
            {
                grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
            }
        }
        private void updateLogGrid(DataGrid grid, List<UIDataGrid> list, int index, string time, string data1, byte data2, float data3, float data4, string data5)
        {
            if (grid.Items.Count >= grid_row_max)
            {
                list.RemoveAt(0);
            }
            list.Add(new UIDataGrid(string.Format("{0}", index), time, data1, string.Format("{0}", data2),
                                                string.Format("{0:f1}", data3), string.Format("{0:f3}", data4),
                                                 data5));
            grid.Items.Refresh();

            if (grid.Items.Count > 0)
            {
                grid.ScrollIntoView(grid.Items[grid.Items.Count - 1]);
            }
        }
    
        private void updateLogGrid(DataGrid grid, List<UIDataGrid> list, int index,  string data1, string data2, float data3, float data4)
        {
            if (grid.Items.Count >= grid_row_max)
            {
                list.RemoveAt(0);
            }
            list.Add(new UIDataGrid(string.Format("{0}", index), data1,  data2,
                                                string.Format("{0:f3}", data3), string.Format("{0:f3}", data4)));
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
            readyToDownloadLog();          
        }

        private void tBtn_ReadLogInfo_Click(object sender, RoutedEventArgs e)
        {
            readLogInfo();     
        }

        private void tBtn_SaveToCSV_Click(object sender, RoutedEventArgs e)
        {
            saveToCSV();        
        }

        private void tBtn_ClrLog_Click(object sender, RoutedEventArgs e)
        {
            clearLog();
        }

        private void tBtn_StopDownload_Click(object sender, RoutedEventArgs e)
        {
            stopDownload();
        }

        private void readyToDownloadLog()
        {
            /*
             * Read Log Info
             */
            byte[] eventType = new byte[1];
            eventType[0] = (byte)(tCmb_LogsType.SelectedIndex + 1);
            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_LOG_INFO, eventType, ref err);
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

            DataGrid grid;
            List<UIDataGrid> gridList;
            switch (eventType[0])
            {
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_CALIBRATION:
                    {
                        grid = grid_LogCal;
                        gridList = logCalList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_ALARM:
                    {
                        grid = grid_LogAlarm;
                        gridList = logAlarmList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_FAULT:
                    {
                        grid = grid_LogFault;
                        gridList = logFaultList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_WARNING:
                    {
                        grid = grid_LogWarning;
                        gridList = logWarningList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_REFLEX:
                    {
                        grid = grid_LogRelfex;
                        gridList = logReflexList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_INFO:
                    {
                        grid = grid_LogInfo;
                        gridList = logInfoList;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }

            gridList.Clear();
            grid.Items.Refresh();
            tBtn_DownloadLogs.IsEnabled = false;
            tBtn_StopDownload.IsEnabled = true;

            tPbar_LogDownBar.Maximum = numLogdata;

            LogMonitorRunning = true;

            LogMonitor = new Thread(() => downloadLog(eventType[0], numLogdata));

            LogMonitor.Start();
        }

        private void downloadLog(byte eventType, int numLogdata)
        {
            /*
             * Read Log Data
             */
            byte[] payload = new byte[3];
            payload[0] = eventType;
            for (int index = 0; index < numLogdata; index++)
            {
                if (!LogMonitorRunning)
                {
                    return;
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tPbar_LogDownBar.Value = index + 1;
                }));
                payload[1] = (byte)(index >> 8);
                payload[2] = (byte)index;

                byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_LOG_DATA, payload, ref err, 300);
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
                    case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_CALIBRATION:
                        {
                            updateLogCal(logData, index);
                            break;
                        }
                    case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_ALARM:
                        {
                            updateLogEvent(logData, index);
                            break;
                        }
                    case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_FAULT:
                        {
                            updateLogEvent(logData, index);
                            break;
                        }
                    case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_WARNING:
                        {
                            updateLogEvent(logData, index);
                            break;
                        }
                    case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_REFLEX:
                        {
                            UpdateLogReflex(logData, index);
                            break;
                        }
                    case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_INFO:
                        {
                            updateLogEvent(logData, index);
                            break;
                        }
                }
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                tBtn_DownloadLogs.IsEnabled = true;
                tBtn_StopDownload.IsEnabled = false;
            }));
        }

        private void readLogInfo()
        {
            /*
            * Read Log Info
            */
            byte[] eventType = new byte[1];
            eventType[0] = (byte)(tCmb_LogsType.SelectedIndex + 1);

            byte[] result = QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_READ_LOG_INFO, eventType, ref err);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Read Log Info");
                return;
            }

            int pos = (int)PACKET_CONF.COMM_POS_PAYLOAD + 3;

            int savedCount = Utility.getU16FromByteA(result, pos);
            int maxCount = Utility.getU16FromByteA(result, pos + 2);
            int size = result[pos + 4];

            string str = "Log Type : " + eventType[0].ToString() + ", Number of log : " + savedCount;
            str += " , Total number of log : " + maxCount + " , Size of log : " + size + " Bytes";

            MessageBox.Show(str);
        }

        private void saveToCSV()
        {
            int eventType = tCmb_LogsType.SelectedIndex + 1;
            List<UIDataGrid> gridList;
            DataGrid grid;
            switch (eventType)
            {
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_CALIBRATION:
                    {
                        logFiles = new LogToFile("Incom_Utility_Calibration_Log", "csv");
                        grid = grid_LogCal;
                        gridList = logCalList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_ALARM:
                    {
                        logFiles = new LogToFile("Incom_Utility_Alarm_Log", "csv");
                        grid = grid_LogAlarm;
                        gridList = logAlarmList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_FAULT:
                    {
                        logFiles = new LogToFile("Incom_Utility_Fault_Log", "csv");
                        grid = grid_LogFault;
                        gridList = logFaultList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_WARNING:
                    {
                        logFiles = new LogToFile("Incom_Utility_Warining_Log", "csv");
                        grid = grid_LogWarning;
                        gridList = logWarningList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_REFLEX:
                    {
                        logFiles = new LogToFile("Incom_Utility_Reflex_Log", "csv");
                        grid = grid_LogRelfex;
                        gridList = logReflexList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_INFO:
                    {
                        logFiles = new LogToFile("Incom_Utility_Info_Log", "csv");
                        grid = grid_LogInfo;
                        gridList = logInfoList;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }

            string filePath = logFiles.Checked();

            if (filePath == null)
            {
                return;
            }

            string strHeader = "";
            int columCount = grid.Columns.Count;
            for (int i = 0; i < columCount; i++)
            {
                if (i == grid.Columns.Count - 1)
                {
                    strHeader += grid.Columns[i].Header.ToString();
                    break;
                }
                strHeader += grid.Columns[i].Header.ToString() + ",";
            }
            logFiles.SetDataHeader(strHeader);

            for (int i = 0; i < gridList.Count; i++)
            {
                string strWrite = UIDataGrid.getDataToStr(gridList[i], columCount);
                logFiles.Write(strWrite);
            }

            logFiles.Unchecked();
            MessageBox.Show("Saved CSV file");
        }

        private void clearLog()
        {
            /*
             * Clear Log Data
             */
            byte[] payload = new byte[2];
            payload[0] = (byte)(tCmb_LogsType.SelectedIndex + 1);
            if ((bool)tChb_RecordClrLog.IsChecked)
                payload[1] = 1;

            QuattroProtocol.sendCommand(INNCOM_COMMAND_LIST.COMM_CMD_CLR_LOG_DATA, payload, ref err, 800);
            if (err != ERROR_LIST.ERROR_NONE)
            {
                MessageBox.Show("ERROR - Clear Log Data");
                return;
            }

            MessageBox.Show("Cleared logs - " + LOG_TABLE_TYPE_STRINGA[payload[0]].ToString(), "Information");
        }

        public void stopDownload()
        {
            if (LogMonitorRunning)
            {
                LogMonitorRunning = false;

                tBtn_DownloadLogs.IsEnabled = true;
                tBtn_StopDownload.IsEnabled = false;
            }

        }

        private void updateLogCal(byte[] logData, int index)
        {
            int offset = 0;

            ushort u16TimeStampYear = Utility.getU16FromByteA(logData, offset);
            byte u8TimeStampMon = logData[offset + 2];
            byte u8TimeStampDay = logData[offset + 3];
            byte u8TimeStampHou = logData[offset + 4];
            byte u8TimeStampMin = logData[offset + 5];
            byte u8TimeStampSec = logData[offset + 6];

            byte u8CalibrationType = logData[offset + 7];
            float f32GasReadingBeforeCal = Utility.getF32FromByteA(logData, offset + 8);
            float f32GasReadingAfterCal = Utility.getF32FromByteA(logData, offset + 12);
            float f32RawSignalBeforeCal = Utility.getF32FromByteA(logData, offset + 16);
            float f32CalFactorBeforeCal = Utility.getF32FromByteA(logData, offset + 20);
            float f32CalFactorAfterCal = Utility.getF32FromByteA(logData, offset + 24);
            float f32TargetGasConcentration = Utility.getF32FromByteA(logData, offset + 28);
            float f32CorrectionFactor = Utility.getF32FromByteA(logData, offset + 32);
            byte u8CalibrationResults = logData[offset + 36];

            byte[] au8CylinderNo = new byte[(int)INNCOM_CONF.NUM_CYLINDER_SN];
            for (int i = 0; i < logData.Length - offset - 37; i++)
                au8CylinderNo[i] = logData[offset + 37 + i];

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
                updateLogGrid(grid_LogCal, logCalList, index + 1, time_str, CAL_TYPE[(int)u8CalibrationType], f32GasReadingBeforeCal, f32GasReadingAfterCal,
                    f32RawSignalBeforeCal, f32CalFactorBeforeCal, f32CalFactorAfterCal, f32TargetGasConcentration, f32CorrectionFactor, result_str, cylinder_str);
            }));
        }

        private void updateLogEvent(byte[] logData, int index)
        {
            System.Windows.Controls.DataGrid grid;
            List<UIDataGrid> list;

            int offset = 0;

            ushort u16TimeStampYear = Utility.getU16FromByteA(logData, offset);
            byte u8TimeStampMon = logData[offset + 2];
            byte u8TimeStampDay = logData[offset + 3];
            byte u8TimeStampHou = logData[offset + 4];
            byte u8TimeStampMin = logData[offset + 5];
            byte u8TimeStampSec = logData[offset + 6];

            byte u8EventType = logData[offset + 7];
            byte u8EventId = logData[offset + 8];
            float f32GasReading = Utility.getF32FromByteA(logData, offset + 9);
            float f32EventData = Utility.getF32FromByteA(logData, offset + 13);

            string time_str = u16TimeStampYear.ToString() + '/' + u8TimeStampMon.ToString("D2") + '/' + u8TimeStampDay.ToString("D2") + ' ';
            time_str += u8TimeStampHou.ToString("D2") + ':' + u8TimeStampMin.ToString("D2") + ':' + u8TimeStampSec.ToString("D2");

            string event_str = "Unknown - " + u8EventType.ToString();
            if (u8EventType > 0 && u8EventType <= (int)NUMBER_OF_TABLE.NUMBER_OF_TABLE_TYPE)
            {
                event_str = LOG_TABLE_TYPE_STRINGA[u8EventType];
            }

            string type_str = "Unknown - " + u8EventId.ToString();
            switch (u8EventType)
            {
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_ALARM:
                    {
                        if (u8EventId > 0 && u8EventId <= (int)NUMBER_OF_TABLE.NUMBER_OF_ALARM)
                        {
                            type_str = ALARM_TYPE[u8EventId];
                        }
                        grid = grid_LogAlarm;
                        list = logAlarmList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_FAULT:
                    {
                        if (u8EventId > 0 && u8EventId <= (int)NUMBER_OF_TABLE.NUMBER_OF_FAULT)
                        {
                            type_str = FAULT_TPYE[u8EventId];
                        }
                        grid = grid_LogFault;
                        list = logFaultList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_WARNING:
                    {
                        if (u8EventId > 0 && u8EventId <= (int)NUMBER_OF_TABLE.NUMBER_OF_WARNING)
                        {
                            type_str = WARNING_TYPE[u8EventId];
                        }
                        grid = grid_LogWarning;
                        list = logWarningList;
                        break;
                    }
                case (int)LOG_TABLE_TYPE.LOG_TABLE_TYPE_INFO:
                    {
                        if (u8EventId > 0 && u8EventId <= (int)NUMBER_OF_TABLE.NUMBER_OF_INFO)
                        {
                            type_str = INFO_TYPE[u8EventId];
                        }
                        grid = grid_LogInfo;
                        list = logInfoList;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                updateLogGrid(grid, list, index + 1, time_str, event_str, u8EventId, f32GasReading, f32EventData, type_str);
            }));

        }

        private void UpdateLogReflex(byte[] logData, int index)
        {
            int offset = 0;

            float f32Adc1 = Utility.getF32FromByteA(logData ,offset);
            float f32Adc2 = Utility.getF32FromByteA(logData, offset + 4);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                updateLogGrid(grid_LogRelfex, logReflexList, index + 1, "tReflex Log", "-", f32Adc1, f32Adc2);
            }));
        }

        public void threadClose()
        {
            if (LogMonitorRunning)
                LogMonitor.Abort();
        }

    }
}
