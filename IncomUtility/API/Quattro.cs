using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IncomUtility
{
    public enum PACKET_CONF
    {
        COMM_SOF = 0x3C,
        COMM_EOF = 0x3E,
        COMM_VERSION = 0x00,

        COMM_SYSTEM_MFG_PC = 0xEE,
        COMM_SYSTEM_DEBUG_PC = 0xCC,
        COMM_SYSTEM_MOBILE = 0xAA,
        COMM_SYSTEM_INCOM = 0xBB,
        COMM_SYSTEM_OELD = 0xBB,
        COMM_SYSTEM_OPTIMA = 0xBB,
        COMM_SYSTEM_EXCEL = 0xBB,
        COMM_DEBUG_PC = 0xDD,

        COMM_POS_SOF = 0x00,
        COMM_POS_VER = 0x01,
        COMM_POS_SRC = 0x02,
        COMM_POS_DEST = 0x03,
        COMM_POS_SEQID = 0x04,
        COMM_POS_LEN = 0x05,
        COMM_POS_PAYLOAD = 0x07,
        COMM_RESULT_OK = 1,

        COMM_SOF_SZ = 1,
        COMM_VER_SZ = 1,
        COMM_SRC_SZ = 1,
        COMM_DEST_SZ = 1,
        COMM_SEQ_SZ = 1,
        COMM_LEN_SZ = 2,
        COMM_CRC_SZ = 2,
        COMM_EOF_SZ = 1,

        COMM_COMM_SZ = 2,
        COMM_RESULT_SZ = 1,
        COMM_RESPONSE_SZ = COMM_COMM_SZ + COMM_RESULT_SZ,

        COMM_AFTER_PAYLOAD_OVERHEAD = COMM_CRC_SZ + COMM_EOF_SZ,
        COMM_LEN_OVERHEAD = COMM_SOF_SZ + COMM_VER_SZ + COMM_SRC_SZ + COMM_DEST_SZ + COMM_SEQ_SZ
            + COMM_LEN_SZ + COMM_CRC_SZ + COMM_EOF_SZ,

        SZ_COMM_BUFFER = 2048,
    }
    public enum COMM_COMMAND_LIST
    {
        COMM_CMD_READ_SW_VERSION = 0x0101,
        COMM_CMD_READ_BOARD_SN = 0x0102,
        COMM_CMD_WRITE_BOARD_SN = 0x0103,
        COMM_CMD_READ_DEVICE_SN = 0x0104,
        COMM_CMD_WRITE_DEVICE_SN = 0x0105,

        COMM_CMD_GET_GAS_READING = 0x0201,
        COMM_CMD_READ_GAS_INFO = 0x0202,
        COMM_CMD_INTERNAL_TEMP = 0x0203,

        COMM_CMD_READ_TIME = 0x0301,
        COMM_CMD_WRITE_TIME = 0x0302,

        COMM_CMD_READ_RAW_ADC = 0x0401,

        // Reset Commands ,
        COMM_CMD_RESET_FACTORY = 0x0501,
        COMM_CMD_RESET_ALARMS = 0x0502,

        // Security Commands 
        COMM_CMD_READ_CRYPT_SN = 0x0601,
        COMM_CMD_REQUEST_DEVICE_CERT_INFO = 0x0602,
        COMM_CMD_CHECK_USER_CERT_INFO = 0x0603,
        COMM_CMD_SEND_USER_CERT = 0x0604,
        COMM_CMD_REQUEST_DEVICE_CERT = 0x0605,
        COMM_CMD_REQUEST_DEVICE_SIGNATURE = 0x0606,
        COMM_CMD_SEND_USER_SIGNATURE = 0x0607,
        COMM_CMD_CREATE_SESSION_KEY = 0x0608,
        COMM_CMD_CREATE_KEY_PAIR = 0x0609,
        COMM_CMD_GET_DEVICE_PUBLIC_KEY = 0x060A,
        COMM_CMD_TRANSFER_CA_CERT = 0x060B,
        COMM_CMD_TRANSFER_DEVICE_CERT = 0x060C,
        COMM_CMD_VERIFY_CA_CERT = 0x060D,
        COMM_CMD_VERIFY_DEVICE_CERT = 0x060E,
        COMM_CMD_SIGN_MESSAGE = 0x060F,

        //BLE INFO COMMANDS 
        COMM_CMD_GET_BLE_DEVICE_NAME = 0x0701,
        COMM_CMD_GET_BLE_MAC_ADDR = 0x0702,
        COMM_CMD_GET_BLE_SW_VER = 0x0703,
        COMM_CMD_GET_BLE_TX_POWER = 0x0704,
        COMM_CMD_GET_BLE_INST_ID = 0x0705,

        //Factory Mode 
        COMM_CMD_REQUEST_CHALLENGE_NUM = 0x0801,
        COMM_CMD_SEND_RESPONSE_NUM = 0x0802,
    }
    public enum INNCOM_COMMAND_LIST
    {
        COMM_CMD_READ_DEVICE_STATUS = 0x7001,
        COMM_CMD_READ_FAULT_DETAILS = 0x7002,
        COMM_CMD_READ_ANALOGUE_OUTPUT = 0x7003,
        COMM_CMD_READ_VOLTAGE_OUTPUT = 0x7004,
        COMM_CMD_READ_VOLTAGE_LEVELS = 0x7005,
        COMM_CMD_READ_CELL_VOLTAGE_CURRENT = 0x7006,
        COMM_CMD_READ_RAW_CELL_SIGNAL = 0x7007,

        COMM_CMD_READ_RAW_GAS_DATA = 0x7009,

        COMM_CMD_READ_OUTPUT_DEVICE_TYPE = 0x7101,
        COMM_CMD_READ_EEPROM_VER = 0x7102,
        COMM_CMD_READ_SENSOR_INFO = 0x7103,

        COMM_CMD_READ_CONFIG = 0x7201,
        COMM_CMD_WRITE_CONFIG = 0x7202,

        //H/W Control

        COMM_CMD_RELEASE_LEDS = 0x7302,
        COMM_CMD_FORCE_RELAYS = 0x7303,
        COMM_CMD_RELEASE_RELAYS = 0x7304,
        COMM_CMD_FORCE_ANALOGUE_OUTPUT = 0x7305,
        COMM_CMD_RELEASE_ANALOGUE_OUTPUT = 0x7306,
        COMM_CMD_FORCE_VOLTAGE_OUTPUT = 0x7307,
        COMM_CMD_RELEASE_VOLTAGE_OUTPUT = 0x7308,
        COMM_CMD_CTRL_WATCHDOG = 0x7309,
        COMM_CMD_INHIBIT_OUTPUT = 0x730A,
        COMM_CMD_RELEASE_OUTPUT = 0x730B,
        COMM_CMD_CTRL_INTERNAL_LEDS = 0x730C,
        COMM_CMD_REPLACE_SENSOR = 0x730D,
        COMM_CMD_EXIT_REPLACE_SENSOR = 0x730E,
        COMM_CMD_CANCEL_SENSOR_REPLACEMENT = 0x730F,

        COMM_CMD_ENABLE_REFLEX_TEST = 0x730F,

        //Calibration
        COMM_CMD_START_ZERO_CAL = 0x7401,
        COMM_CMD_STOP_ZERO_CAL = 0x7402,
        COMM_CMD_ACCEPT_ZERO_CAL = 0x7403,
        COMM_CMD_START_SPAN_CAL = 0x7404,

        COMM_CMD_ACCEPT_SPAN_CAL = 0x7406,

        //Calibrate Analogue Output
        COMM_CMD_START_ANALOGUE_ZERO_CAL = 0x7501,
        COMM_CMD_STOP_ANALOGUE_ZERO_CAL = 0x7502,
        COMM_CMD_ACCEPT_ANALOGUE_ZERO_CAL = 0x7503,
        COMM_CMD_START_ANALOGUE_SPAN_CAL = 0x7504,
        COMM_CMD_STOP_ANALOGUE_SPAN_CAL = 0x7505,
        COMM_CMD_ACCPET_ANALOGUE_SPAN_CAL = 0x7506,

        //Calibrate Voltage Output
        COMM_CMD_START_VOLTAGE_ZERO_CAL = 0x7601,
        COMM_CMD_STOP_VOLTAGE_ZERO_CAL = 0x7602,
        COMM_CMD_ACCEPT_VOLTAGE_ZERO_CAL = 0x7603,
        COMM_CMD_START_VOLTAGE_SPAN_CAL = 0x7604,
        COMM_CMD_STOP_VOLTAGE_SPAN_CAL = 0x7605,
        COMM_CMD_ACCPET_VOLTAGE_SPAN_CAL = 0x7606,

        //Switches
        COMM_CMD_READ_INHIBIT_SWITCH = 0x7701,
        COMM_CMD_READ_SWITCH_STATUS = 0x7702,

        //Calibrate Cell Drive,
        COMM_CMD_START_SEN_DRIVE_CAL = 0x7801,
        COMM_CMD_STOP_SEN_DRIVE_CAL = 0x7802,
        COMM_CMD_INC_DRIVE_DUTY = 0x7803,
        COMM_CMD_DEC_DRIVE_DUTY = 0x7804,
        COMM_CMD_ACCPET_SEN_DRIVE_CAL = 0x7805,

        //Calibrate Cell Drive
        COMM_CMD_ENUMERATE_LOGS = 0x7901,
        COMM_CMD_WRITE_LOGS = 0x7902,
        COMM_CMD_CLR_LATCHED_TABLES = 0x7903,
        COMM_CMD_READ_LOG_INFO = 0x7904,
        COMM_CMD_READ_LOG_DATA = 0x7905,
        COMM_CMD_CLR_LOG_DATA = 0x7906,

        //Non-volatile Memory
        COMM_CMD_READ_DATA_FROM_MEM = 0x7A01,
        COMM_CMD_WRITE_DATA_TO_MEM = 0x7A02,

        //Simulation Mode   
        COMM_CMD_START_SIMULATION = 0x7B01,
        COMM_CMD_STOP_SIMULATION = 0x7B02,

        //Debug command,
        DBG_CMD_READ_GAS_DATA = 0x8756,

        SZ_EEP_MEMORY = 4096,
        SZ_MAX_MEMORY_BLOCK = 128,
        MEM_TYPE_EEPROM = 1,
        MEM_TYPE_FLASH = 0,

        CERT_NOT_PRESENT = 0,
        CERT_VALID = 1,
        CERT_INVALID = 2,
        CERT_EXPIRED = 3,

    }
    public enum INNCOM_CONF
    {
        MODBUS_PARAM_SLAVE_ID = 0x0101,
        MODBUS_PARAM_BAUDRATE = 0x0102,
        MODBUS_PARAM_PARITY = 0x0103,
        MODBUS_PARAM_FLOW_CONTROL = 0x0104,
        MODBUS_PARAM_DATABITS = 0x0105,
        MODBUS_PARAM_STOPBITS = 0x0106,

        RELAY_PARAM_TRIGGER1 = 0x0201,
        RELAY_PARAM_TRIGGER2 = 0x0202,
        RELAY_PARAM_INIT_STATE1 = 0x0203,
        RELAY_PARAM_INIT_STATE2 = 0x0204,
        RELAY_PARAM_ON_DELAY_TIME = 0x0205,
        RELAY_PARAM_OFF_DELAY_TIME = 0x0206,

        MA_PARAM_FAULT_CURRENT = 0x0301,
        MA_PARAM_WARNING_CURRENT = 0x0302,
        MA_PARAM_OVERRANGE_CURRENT = 0x0303,
        MA_PARAM_INHIBIT_TIMEOUT = 0x0304,

        GENERAL_PARAM_LOCATION_TAG = 0x0401,
        GENERAL_PARAM_LED_CONTROL = 0x0402,
        GENERAL_PARAM_ALARM_OP_MODE = 0x0403,
        GENERAL_PARAM_OP_MODE = 0x0404,
        GENERAL_PARAM_CAL_OVERDUE = 0x0405,
        GENERAL_PARAM_PASSCODE = 0x0406,

        ALARM_PARAM_THRESHOLD1 = 0x0501,
        ALARM_PARAM_THRESHOLD2 = 0x0502,
        ALARM_PARAM_THRESHOLD3 = 0x0503,
        ALARM_PARAM_TRIGGER1 = 0x0504,
        ALARM_PARAM_TRIGGER2 = 0x0505,
        ALARM_PARAM_TRIGGER3 = 0x0506,
        ALARM_PARAM_LATCHING = 0x0507,

        CAL_PARAM_CAL_INTERVAL = 0x0601,
        CAL_PARAM_CAL_CONC = 0x0602,
        CAL_PARAM_LAST_CAL_DATE = 0x0603,
        CAL_PARAM_DAYS_SINCE_LAST_CAL = 0x0604,

        GAS_PARAM_USER_GAS_NAME = 0x0701,
        GAS_PARAM_MEASURING_RANGE = 0x0702,
        GAS_PARAM_CORRECTION_FACTOR = 0x0703,
        GAS_PARAM_MEASUREMENT_UNITS = 0x0704,
        GAS_PARAM_UNIT_CONVERSION = 0x0705,
        GAS_PARAM_DEADBAND = 0x0706,
        GAS_PARAM_GAS_TYPE = 0x0707,
        GAS_PARAM_TARGET_CHANNEL = 0x0708,
        GAS_PARAM_DECIMAL_POINT = 0x0709,
        GAS_PARAM_DISPLAY_RESOLUTION = 0x070A,

        CIRCUIT_PARAM_420MA_OFFSET_SINK = 0x0801,
        CIRCUIT_PARAM_420MA_OFFSET_SOURCE = 0x0802,
        CIRCUIT_PARAM_420MA_SPAN_SINK = 0x0803,
        CIRCUIT_PARAM_420MA_SPAN_SOURCE = 0x0804,
        CIRCUIT_PARAM_420MA_LOOP_OFFSET_SINK = 0x0805,
        CIRCUIT_PARAM_420MA_LOOP_OFFSET_SOURCE = 0x0806,
        CIRCUIT_PARAM_420MA_LOOP_SPAN_SINK = 0x0807,
        CIRCUIT_PARAM_420MA_LOOP_SPAN_SOURCE = 0x0808,
        CIRCUIT_PARAM_VOLTAGE_OUT_OFFSET = 0x0809,
        CIRCUIT_PARAM_VOLTAGE_OUT_SPAN = 0x080A,

        DEV_INFO_PARAM_DEVICE_SN = 0x0901,
        DEV_INFO_PARAM_BOARD_SN = 0x0902,

        SEC_PARAM_NUM_RETRY = 0x0A01,
        SEC_PARAM_LOGIN_LOCK_TIME = 0x0A02,
        SEC_PARAM_OTP_CONNECTION = 0x0A03,
        SEC_PARAM_OTP_KEY = 0x0A04,

        NTC_PARAM_TEMP_COMP1 = 0x0B01,
        NTC_PARAM_TEMP_COMP2 = 0x0B02,
        NTC_PARAM_TEMP_COMP3 = 0x0B03,
        NTC_PARAM_TEMP_COMP4 = 0x0B04,
        NTC_PARAM_TEMP_COMP5 = 0x0B05,
        NTC_PARAM_TEMP_COMP6 = 0x0B06,
        NTC_PARAM_TEMP_COMP7 = 0x0B07,
        NTC_PARAM_TEMP_COMP8 = 0x0B08,
        NTC_PARAM_TEMP_COMP9 = 0x0B09,
        NTC_PARAM_TEMP_COMP10 = 0x0B0A,
        NTC_PARAM_TEMP_COMP11 = 0x0B0B,
        NTC_PARAM_TEMP_COMP12 = 0x0B0C,

        UL2075_ALARM_THRESHOLD1 = 0x0C01,
        UL2075_ALARM_THRESHOLD2 = 0x0C02,
        UL2075_ALARM_THRESHOLD3 = 0x0C03,
        UL2075_ALARM_THRESHOLD4 = 0x0C04,
        UL2075_ALARM_THRESHOLD5 = 0x0C05,
        UL2075_ALARM_THRESHOLD6 = 0x0C06,
        UL2075_ALARM_THRESHOLD7 = 0x0C07,
        UL2075_ALARM_THRESHOLD8 = 0x0C08,
        UL2075_ALARM_THRESHOLD9 = 0x0C09,
        UL2075_ALARM_THRESHOLD10 = 0x0C0A,
        UL2075_ALARM_THRESHOLD11 = 0x0C0B,
        UL2075_ALARM_THRESHOLD12 = 0x0C0C,
        UL2075_ALARM_THRESHOLD13 = 0x0C0D,
        UL2075_ALARM_THRESHOLD14 = 0x0C0E,
        UL2075_ALARM_THRESHOLD15 = 0x0C0F,
        UL2075_ALARM_THRESHOLD16 = 0x0C10,
        UL2075_ALARM_THRESHOLD17 = 0x0C11,
        UL2075_ALARM_THRESHOLD18 = 0x0C12,
        UL2075_ALARM_THRESHOLD19 = 0x0C13,
        UL2075_ALARM_THRESHOLD20 = 0x0C14,
        UL2075_ALARM_THRESHOLD21 = 0x0C15,
        UL2075_ALARM_THRESHOLD22 = 0x0C16,
        UL2075_ALARM_THRESHOLD23 = 0x0C17,
        UL2075_ALARM_THRESHOLD24 = 0x0C18,
        UL2075_ALARM_THRESHOLD25 = 0x0C19,
        UL2075_ALARM_THRESHOLD26 = 0x0C1A,
        UL2075_ALARM_THRESHOLD27 = 0x0C1B,
        UL2075_ALARM_THRESHOLD28 = 0x0C1C,
        UL2075_ALARM_THRESHOLD29 = 0x0C1D,
        UL2075_ALARM_THRESHOLD30 = 0x0C1E,
        UL2075_ALARM_THRESHOLD31 = 0x0C1F,
        UL2075_ALARM_THRESHOLD32 = 0x0C20,
        UL2075_ALARM_THRESHOLD33 = 0x0C21,
        UL2075_ALARM_THRESHOLD34 = 0x0C22,
        UL2075_ALARM_THRESHOLD35 = 0x0C23,
        UL2075_ALARM_THRESHOLD36 = 0x0C24,
        UL2075_ALARM_THRESHOLD37 = 0x0C25,
        UL2075_ALARM_THRESHOLD38 = 0x0C26,
        UL2075_ALARM_THRESHOLD39 = 0x0C27,
        UL2075_ALARM_THRESHOLD40 = 0x0C28,

        NUM_HISTOGRAM = 40,
        NUM_NTC_COMP = 12,
        NUM_LOCATION_TAG = 25,
        NUM_BOARD_SN = 16,
        NUM_DEVICE_SN = 16,
        NUM_GAS_NAME = 15,
        NUM_OTP_KEY = 32,
        
        PARAM_TYPE_U8 = 0,
        PARAM_TYPE_S8 = 1,
        PARAM_TYPE_U16 = 2,
        PARAM_TYPE_S16 = 3,
        PARAM_TYPE_U32 = 4,
        PARAM_TYPE_S32 = 5,
        PARAM_TYPE_U64 = 6,
        PARAM_TYPE_S64 = 7,
        PARAM_TYPE_F32 = 8,
        PARAM_TYPE_STR = 9,
        PARAM_TYPE_U8A = 10,
        PARAM_TYPE_CUSTOM = 11,

        NUM_CONFIG_PARAM = 111,
        NUM_CYLINDER_SN = 20,

        LOG_TABLE_TYPE_ALARM = 1,
        LOG_TABLE_TYPE_WARNING = 2,
        LOG_TABLE_TYPE_FAULT = 3,
        LOG_TABLE_TYPE_INFO = 4,
        LOG_TABLE_TYPE_CALIBRATION = 5,
        LOG_TABLE_TYPE_REFLEX = 6,
    }

    public enum ERROR_LIST
    {
        ERROR_NONE = 0,
        ERROR_PORT_NOT_OPEN = 1,
        ERROR_INPUT_DATA_NONE = 2,
        ERROR_INPUT_DATA_WRONG = 3,
        ERROR_RECIVE_DATA_NONE = 4,
        ERROR_RECIVE_DATA_WRONG = 5,
        ERROR_SOF_WRONG = 6,
        ERROR_EOF_WRONG = 7,
        ERROR_EXCEPTION = 8,
        ERROR_CRC_WRONG = 9,
        ERROR_NCK = 10

    }
    public class CONFIG_PARAM_TABLE_STRUCT {
        public ushort u16_parm_index;
        public byte u8_param_type;
        public byte u8_length;

        public CONFIG_PARAM_TABLE_STRUCT(ushort index, byte type, byte length)
        {
            u16_parm_index = index;
            u8_param_type = type;
            u8_length = length;
        }
    }

    //CONFIG_PARAM_TABLE_STRUCT[] SenParamTable = new CONFIG_PARAM_TABLE_STRUCT 
    //    CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MODBUS_PARAM_SLAVE_ID, 0, 1);
    public class SenParamTable 
    {
        public List<CONFIG_PARAM_TABLE_STRUCT> SenParamTableList = new List<CONFIG_PARAM_TABLE_STRUCT>();

        public SenParamTable()
        {
            InitTable();
        }
        private void InitTable()
        {
            //Modbus Settings
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MODBUS_PARAM_SLAVE_ID, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MODBUS_PARAM_BAUDRATE, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MODBUS_PARAM_PARITY, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MODBUS_PARAM_FLOW_CONTROL, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MODBUS_PARAM_DATABITS, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MODBUS_PARAM_STOPBITS, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));

            //Relay Settings
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.RELAY_PARAM_TRIGGER1, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.RELAY_PARAM_TRIGGER2, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.RELAY_PARAM_INIT_STATE1, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.RELAY_PARAM_INIT_STATE2, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.RELAY_PARAM_ON_DELAY_TIME, (byte)INNCOM_CONF.PARAM_TYPE_U16, 2));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.RELAY_PARAM_OFF_DELAY_TIME, (byte)INNCOM_CONF.PARAM_TYPE_U16, 2));

            //mA Output Settings
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MA_PARAM_FAULT_CURRENT, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MA_PARAM_WARNING_CURRENT, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MA_PARAM_OVERRANGE_CURRENT, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.MA_PARAM_INHIBIT_TIMEOUT, (byte)INNCOM_CONF.PARAM_TYPE_U16, 2));

            //General,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GENERAL_PARAM_LOCATION_TAG, (byte)INNCOM_CONF.PARAM_TYPE_STR, 25));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GENERAL_PARAM_LED_CONTROL, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GENERAL_PARAM_ALARM_OP_MODE, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GENERAL_PARAM_OP_MODE, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GENERAL_PARAM_CAL_OVERDUE, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GENERAL_PARAM_PASSCODE, (byte)INNCOM_CONF.PARAM_TYPE_U8A, 4));


            //Gas Alarm Settings,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.ALARM_PARAM_THRESHOLD1, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.ALARM_PARAM_THRESHOLD2, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.ALARM_PARAM_THRESHOLD3, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.ALARM_PARAM_TRIGGER1, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.ALARM_PARAM_TRIGGER2, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.ALARM_PARAM_TRIGGER3, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.ALARM_PARAM_LATCHING, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));

            //Gas Calibration Settings,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CAL_PARAM_CAL_INTERVAL, (byte)INNCOM_CONF.PARAM_TYPE_U16, 2));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CAL_PARAM_CAL_CONC, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CAL_PARAM_LAST_CAL_DATE, (byte)INNCOM_CONF.PARAM_TYPE_U32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CAL_PARAM_DAYS_SINCE_LAST_CAL, (byte)INNCOM_CONF.PARAM_TYPE_U16, 2));


            //Gas Settings,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_USER_GAS_NAME, (byte)INNCOM_CONF.PARAM_TYPE_STR, 15));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_MEASURING_RANGE, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_CORRECTION_FACTOR, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_MEASUREMENT_UNITS, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_UNIT_CONVERSION, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_DEADBAND, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_GAS_TYPE, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_TARGET_CHANNEL, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_DECIMAL_POINT, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.GAS_PARAM_DISPLAY_RESOLUTION, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));


            //Circuit Calibration,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_OFFSET_SINK, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_OFFSET_SOURCE, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_SPAN_SINK, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_SPAN_SOURCE, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SINK, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SOURCE, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_LOOP_SPAN_SINK, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_420MA_LOOP_SPAN_SOURCE, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_VOLTAGE_OUT_OFFSET, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.CIRCUIT_PARAM_VOLTAGE_OUT_SPAN, (byte)INNCOM_CONF.PARAM_TYPE_F32, 4));

            //Device Information,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.DEV_INFO_PARAM_DEVICE_SN, (byte)INNCOM_CONF.PARAM_TYPE_STR, (byte)INNCOM_CONF.NUM_DEVICE_SN));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.DEV_INFO_PARAM_BOARD_SN, (byte)INNCOM_CONF.PARAM_TYPE_STR, 16));

            //Security,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.SEC_PARAM_NUM_RETRY, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.SEC_PARAM_LOGIN_LOCK_TIME, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.SEC_PARAM_OTP_CONNECTION, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.SEC_PARAM_OTP_KEY, (byte)INNCOM_CONF.PARAM_TYPE_STR, 32));

            //NTC Compensation Table,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP1, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP2, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP3, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP4, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP5, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP6, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP7, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP8, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP9, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP10, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP11, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.NTC_PARAM_TEMP_COMP12, (byte)INNCOM_CONF.PARAM_TYPE_S8, 1));

            //UL2075 histogram,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD1, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD2, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD3, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD4, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD5, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD6, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD7, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD8, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD9, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD10, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD11, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD12, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD13, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD14, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD15, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD16, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD17, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD18, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD19, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD20, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD21, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD22, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD23, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD24, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD25, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD26, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD27, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD28, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD29, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD30, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD31, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD32, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD33, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD34, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD35, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD36, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD37, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD38, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD39, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF.UL2075_ALARM_THRESHOLD40, (byte)INNCOM_CONF.PARAM_TYPE_U8, 1));
        }
        public bool SearchParmCfg(ushort u16_param, ref CONFIG_PARAM_TABLE_STRUCT data)
        {
            bool b_found = false;

            for (int u16_index = 0; u16_index < (int)INNCOM_CONF.NUM_CONFIG_PARAM; u16_index++)
            {
                if (SenParamTableList[u16_index].u16_parm_index == u16_param) {

                    b_found = true;
                    data = SenParamTableList[u16_index];
                    break;
                }
            }
            
        return b_found;

        }
    };

    class Quattro : CRC16
    {
        public byte[] buildCMDPacket(byte src, byte dest, byte[] payload, ref ERROR_LIST ret)
        {
            if (payload == null)
            {
                ret = ERROR_LIST.ERROR_INPUT_DATA_NONE;
                return null;
            }
            int totalPackageLength = (int)PACKET_CONF.COMM_LEN_OVERHEAD + payload.Length;
            if (totalPackageLength <= (int)PACKET_CONF.COMM_LEN_OVERHEAD)
            {
                ret = ERROR_LIST.ERROR_INPUT_DATA_WRONG;
                return null;
            }
            byte[] CMD = new byte[totalPackageLength];

            CMD[(int)PACKET_CONF.COMM_POS_SOF] = (byte)PACKET_CONF.COMM_SOF;
            CMD[(int)PACKET_CONF.COMM_POS_VER] = (byte)PACKET_CONF.COMM_VERSION;
            CMD[(int)PACKET_CONF.COMM_POS_SRC] = src;
            CMD[(int)PACKET_CONF.COMM_POS_DEST] = dest;
            CMD[(int)PACKET_CONF.COMM_POS_SEQID] = 0x01;


            CMD[(int)PACKET_CONF.COMM_POS_LEN] = (byte)(payload.Length >> 8 & 0x00FF);
            CMD[(int)PACKET_CONF.COMM_POS_LEN + 1] = (byte)(payload.Length);

            //payload is at least 2 bytes - command
            for (int i = 0; i < payload.Length; i++)
                CMD[(int)PACKET_CONF.COMM_POS_PAYLOAD + i] = payload[i];

            //Calcurate CRC
            UInt16 calcurateCRC = UpdateCRC16(CMD, (uint)totalPackageLength - (int)PACKET_CONF.COMM_AFTER_PAYLOAD_OVERHEAD);

            CMD[totalPackageLength - 3] = (byte)((calcurateCRC >> 8) & 0x00FF);
            CMD[totalPackageLength - 2] = (byte)((calcurateCRC) & 0x00FF);
            CMD[totalPackageLength - 1] = (byte)PACKET_CONF.COMM_EOF;

            ret = ERROR_LIST.ERROR_NONE;

            return CMD;
        }


        public ERROR_LIST validateRXPacket(byte[] data)
        {
            if (data == null)
                return ERROR_LIST.ERROR_INPUT_DATA_NONE;

            if (data.Length < (int)PACKET_CONF.COMM_LEN_OVERHEAD)
                return ERROR_LIST.ERROR_RECIVE_DATA_WRONG;
            try
            {
                int payloadLen = data[(int)PACKET_CONF.COMM_POS_LEN] * 256 + data[(int)PACKET_CONF.COMM_POS_LEN + 1];
                int totalLen = payloadLen + (int)PACKET_CONF.COMM_LEN_OVERHEAD;

                /*
                 *Check Start Mark
                 */
                if (data[(int)PACKET_CONF.COMM_POS_SOF] != (byte)PACKET_CONF.COMM_SOF)
                    return ERROR_LIST.ERROR_SOF_WRONG;

                /*
                 * Check End Mark
                 */
                if (data[totalLen - 1] != (byte)PACKET_CONF.COMM_EOF)
                    return ERROR_LIST.ERROR_EOF_WRONG;

                /*
                *  Check CRC16
                */
                ushort updateCRC = UpdateCRC16(data, (uint)totalLen - 3);
                int calCRC = data[totalLen - 3] * 256 + data[totalLen - 2];

                if (updateCRC != calCRC)
                    return ERROR_LIST.ERROR_CRC_WRONG;
                /*
                 *  Check ACK bit
                */
                if (data[(int)PACKET_CONF.COMM_POS_PAYLOAD + 2] != (byte)PACKET_CONF.COMM_RESULT_OK)
                    return ERROR_LIST.ERROR_NCK;

                return ERROR_LIST.ERROR_NONE;
            }
            catch
            {
                return ERROR_LIST.ERROR_EXCEPTION;
            }

        }

        public static byte[] commandToByteArray(INNCOM_COMMAND_LIST com)
        {
            int intValue = (int)com;
            byte[] intBytes = BitConverter.GetBytes(intValue);
            byte[] result = new byte[2];

            Array.Copy(intBytes, result, 2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
        public static byte[] commandToByteArray(COMM_COMMAND_LIST com)
        {
            int intValue = (int)com;
            byte[] intBytes = BitConverter.GetBytes(intValue);
            byte[] result = new byte[2];

            Array.Copy(intBytes, result, 2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
        public static byte[] commandToByteArray(INNCOM_CONF com)
        {
            int intValue = (int)com;
            byte[] intBytes = BitConverter.GetBytes(intValue);
            byte[] result = new byte[2];

            Array.Copy(intBytes, result, 2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
        public static byte[] getResponseValueData(byte[] data)
        {
            int datalen = data[(int)PACKET_CONF.COMM_POS_LEN] * 256 + data[(int)PACKET_CONF.COMM_POS_LEN + 1] - (int)PACKET_CONF.COMM_RESPONSE_SZ;
            if (datalen == 0)
            {               
                byte[] err = { 0x00 };
                return err;
            }
            byte[] value = new byte[datalen];
            Array.Copy(data, (int)PACKET_CONF.COMM_POS_PAYLOAD +(int)PACKET_CONF.COMM_RESPONSE_SZ, value, 0, datalen);

            return value;
        }
 
    }
}