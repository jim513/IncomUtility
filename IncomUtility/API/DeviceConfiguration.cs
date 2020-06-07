using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomUtility
{
    public enum INNCOM_CONF_LIST
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
    }
    public enum PARAMETER_TYPE
    {
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
    }
    public enum INNCOM_CONF
    {
        NUM_HISTOGRAM = 40,
        NUM_NTC_COMP = 12,
        NUM_LOCATION_TAG = 25,
        NUM_BOARD_SN = 16,
        NUM_DEVICE_SN = 16,
        NUM_GAS_NAME = 15,
        NUM_OTP_KEY = 32,

        SZ_PARAM_INDEX = 2,

        NUM_CONFIG_PARAM = 111,
        NUM_CYLINDER_SN = 20,
    }
    public enum LOG_TABLE_TYPE
    {
        LOG_TABLE_TYPE_ALARM = 1,
        LOG_TABLE_TYPE_WARNING = 2,
        LOG_TABLE_TYPE_FAULT = 3,
        LOG_TABLE_TYPE_INFO = 4,
        LOG_TABLE_TYPE_CALIBRATION = 5,
        LOG_TABLE_TYPE_REFLEX = 6,
    }
    public class INCOM_DEVICE_CONFIG_STRUCT
    {
        public CONFIG_MODBUS_STRUCT tModbusCfg;
        public CONFIG_RELAY_STRUCT tRelayCfg;
        public CONFIG_MA_OUPUT_STRUCT tmAOutputCfg;
        public CONFIG_GENERAL_SETTINGS_STRUCT tGeneralCfg;
        public CONFIG_CIRCUIT_CALIBRATION_STRUCT tCircuitCalCfg;
        public byte[] u8Reserved = new byte[26];
        public CONFIG_DEVICE_INFO_STRUCT tDeviceInfo;
        public CONFIG_SECURITY_STRUCT tSecurityCfg;
        public CONFIG_NTC_COMP_STRUCT tNtcCfg;
        public CONFIG_UL2075_HISTOGRAM_STRUCT tUL2075Cfg;

        public INCOM_DEVICE_CONFIG_STRUCT()
        {
            tModbusCfg = new CONFIG_MODBUS_STRUCT();
            tRelayCfg = new CONFIG_RELAY_STRUCT();
            tmAOutputCfg = new CONFIG_MA_OUPUT_STRUCT();
            tGeneralCfg = new CONFIG_GENERAL_SETTINGS_STRUCT();
            tCircuitCalCfg = new CONFIG_CIRCUIT_CALIBRATION_STRUCT();
            tDeviceInfo = new CONFIG_DEVICE_INFO_STRUCT();
            tSecurityCfg = new CONFIG_SECURITY_STRUCT();
            tNtcCfg = new CONFIG_NTC_COMP_STRUCT();
            tUL2075Cfg = new CONFIG_UL2075_HISTOGRAM_STRUCT();
        }
    }
    public class CONFIG_MODBUS_STRUCT
    {
        public ushort u16_crc = 0;
        public byte u8_SlaveId;
        public byte u8_Baudrate;
        public byte u8_Parity;
        public byte u8_FlowCtrl;
        public byte u8_Databits;
        public byte u8_Stopbits;
    }
    public class CONFIG_RELAY_STRUCT
    {
        public ushort u16_crc;
        public byte u8_TriggerEvent1;
        public byte u8_TriggerEvent2;
        public byte u8_InitialState1;
        public byte u8_InitialState2;
        public ushort u16_OnDelayTime;
        public ushort u16_OffDelayTime;
    }

    public class CONFIG_MA_OUPUT_STRUCT
    {
        public ushort u16_crc;
        public byte u8_FaultCurrent;
        public byte u8_WarningCurrent;
        public byte u8_OverrangeCurrent;
        public ushort u16_InhibitTimeout;
    }
    public class CONFIG_GENERAL_SETTINGS_STRUCT
    {
        public ushort u16_crc;
        public byte[] u8_LocationTag = new byte[(int)INNCOM_CONF.NUM_LOCATION_TAG];
        public byte u8_LEDCtrl;
        public byte u8_AlarmMode;
        public byte u8_SafeMode;
        public byte u8_CalOverDueOption;
        public byte[] u8_passcode = new byte[4];
    }

    public class CONFIG_CIRCUIT_CALIBRATION_STRUCT
    {
        public ushort u16_crc;
        public float f32_mAOutputSinkOffset;
        public float f32_mAOutputSourceOffset;
        public float f32_mAOutputSinkSpan;
        public float f32_mAOutputSourceSpan;
        public float f32_mALoopbackSinkOffset;
        public float f32_mALoopbackSourceOffset;
        public float f32_mALoopbackSinkSpan;
        public float f32_mALoopbackSourceSpan;
        public float f32_VoltageOutputOffset;
        public float f32_VoltageOutputSpan;
    }
    public class CONFIG_DEVICE_INFO_STRUCT
    {
        public ushort u16_crc;
        public byte[] u8_DeviceSerialNum = new byte[(int)INNCOM_CONF.NUM_DEVICE_SN];
        public byte[] u8_BoardSerialNum = new byte[(int)INNCOM_CONF.NUM_BOARD_SN];
    }

    public class CONFIG_SECURITY_STRUCT
    {
        public ushort u16_crc;
        public byte u8_num_login_retry;
        public byte u8_login_locktime;
        public byte u8_otp_limits;
        public byte u8_reserved;
        public byte[] u8_otp_key = new byte[(int)INNCOM_CONF.NUM_OTP_KEY];
    }
    public class CONFIG_NTC_COMP_STRUCT
    {
        public ushort u16_crc;
        public sbyte[] s8_NtcTempComp = new sbyte[(int)INNCOM_CONF.NUM_NTC_COMP];
    }

    public class CONFIG_UL2075_HISTOGRAM_STRUCT
    {
        public ushort u16_crc;
        public byte[] u8_Histogram = new byte[(int)INNCOM_CONF.NUM_HISTOGRAM];
    }
    public class CONFIG_PARAM_TABLE_STRUCT
    {
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
    public class SenParamTable
    {
        public List<CONFIG_PARAM_TABLE_STRUCT> SenParamTableList = new List<CONFIG_PARAM_TABLE_STRUCT>();

        public SenParamTable()
        {
            initTable();
        }
        private void initTable()
        {
            //Modbus Settings
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MODBUS_PARAM_SLAVE_ID, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MODBUS_PARAM_BAUDRATE, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MODBUS_PARAM_PARITY, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MODBUS_PARAM_FLOW_CONTROL, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MODBUS_PARAM_DATABITS, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MODBUS_PARAM_STOPBITS, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));

            //Relay Settings
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.RELAY_PARAM_TRIGGER1, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.RELAY_PARAM_TRIGGER2, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.RELAY_PARAM_INIT_STATE1, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.RELAY_PARAM_INIT_STATE2, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.RELAY_PARAM_ON_DELAY_TIME, (byte)PARAMETER_TYPE.PARAM_TYPE_U16, 2));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.RELAY_PARAM_OFF_DELAY_TIME, (byte)PARAMETER_TYPE.PARAM_TYPE_U16, 2));

            //mA Output Settings
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MA_PARAM_FAULT_CURRENT, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MA_PARAM_WARNING_CURRENT, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MA_PARAM_OVERRANGE_CURRENT, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.MA_PARAM_INHIBIT_TIMEOUT, (byte)PARAMETER_TYPE.PARAM_TYPE_U16, 2));

            //General,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GENERAL_PARAM_LOCATION_TAG, (byte)PARAMETER_TYPE.PARAM_TYPE_STR, 25));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GENERAL_PARAM_LED_CONTROL, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GENERAL_PARAM_ALARM_OP_MODE, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GENERAL_PARAM_OP_MODE, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GENERAL_PARAM_CAL_OVERDUE, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GENERAL_PARAM_PASSCODE, (byte)PARAMETER_TYPE.PARAM_TYPE_U8A, 4));


            //Gas Alarm Settings,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD1, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD2, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.ALARM_PARAM_THRESHOLD3, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER1, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER2, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.ALARM_PARAM_TRIGGER3, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.ALARM_PARAM_LATCHING, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));

            //Gas Calibration Settings,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CAL_PARAM_CAL_INTERVAL, (byte)PARAMETER_TYPE.PARAM_TYPE_U16, 2));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CAL_PARAM_CAL_CONC, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CAL_PARAM_LAST_CAL_DATE, (byte)PARAMETER_TYPE.PARAM_TYPE_U32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CAL_PARAM_DAYS_SINCE_LAST_CAL, (byte)PARAMETER_TYPE.PARAM_TYPE_U16, 2));


            //Gas Settings,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_USER_GAS_NAME, (byte)PARAMETER_TYPE.PARAM_TYPE_STR, 15));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_MEASURING_RANGE, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_CORRECTION_FACTOR, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_MEASUREMENT_UNITS, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_UNIT_CONVERSION, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_DEADBAND, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_GAS_TYPE, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_TARGET_CHANNEL, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_DECIMAL_POINT, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.GAS_PARAM_DISPLAY_RESOLUTION, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));


            //Circuit Calibration,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SINK, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_OFFSET_SOURCE, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SINK, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_SPAN_SOURCE, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SINK, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_OFFSET_SOURCE, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SINK, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_420MA_LOOP_SPAN_SOURCE, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_OFFSET, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.CIRCUIT_PARAM_VOLTAGE_OUT_SPAN, (byte)PARAMETER_TYPE.PARAM_TYPE_F32, 4));

            //Device Information,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.DEV_INFO_PARAM_DEVICE_SN, (byte)PARAMETER_TYPE.PARAM_TYPE_STR, (byte)INNCOM_CONF.NUM_DEVICE_SN));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.DEV_INFO_PARAM_BOARD_SN, (byte)PARAMETER_TYPE.PARAM_TYPE_STR, 16));

            //Security,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.SEC_PARAM_NUM_RETRY, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.SEC_PARAM_LOGIN_LOCK_TIME, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.SEC_PARAM_OTP_CONNECTION, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.SEC_PARAM_OTP_KEY, (byte)PARAMETER_TYPE.PARAM_TYPE_STR, 32));

            //NTC Compensation Table,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP1, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP2, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP3, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP4, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP5, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP6, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP7, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP8, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP9, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP10, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP11, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.NTC_PARAM_TEMP_COMP12, (byte)PARAMETER_TYPE.PARAM_TYPE_S8, 1));

            //UL2075 histogram,
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD1, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD2, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD3, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD4, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD5, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD6, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD7, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD8, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD9, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD10, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD11, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD12, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD13, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD14, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD15, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD16, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD17, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD18, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD19, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD20, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD21, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD22, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD23, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD24, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD25, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD26, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD27, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD28, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD29, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD30, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD31, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD32, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD33, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD34, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD35, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD36, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD37, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD38, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD39, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
            SenParamTableList.Add(new CONFIG_PARAM_TABLE_STRUCT((ushort)INNCOM_CONF_LIST.UL2075_ALARM_THRESHOLD40, (byte)PARAMETER_TYPE.PARAM_TYPE_U8, 1));
        }
        public bool searchParmCfg(ushort u16_param, ref CONFIG_PARAM_TABLE_STRUCT data)
        {
            bool b_found = false;

            for (int u16_index = 0; u16_index < (int)INNCOM_CONF.NUM_CONFIG_PARAM; u16_index++)
            {
                if (SenParamTableList[u16_index].u16_parm_index == u16_param)
                {

                    b_found = true;
                    data = SenParamTableList[u16_index];
                    break;
                }
            }

            return b_found;

        }
    };
    
    class DeviceConfiguration
    {
        public static byte[] configurationToByteArray(INNCOM_CONF com)
        {
            int intValue = (int)com;
            byte[] intBytes = BitConverter.GetBytes(intValue);
            byte[] result = new byte[2];

            Array.Copy(intBytes, result, 2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
        public static byte[] configurationToByteArray(INNCOM_CONF_LIST com)
        {
            int intValue = (int)com;
            byte[] intBytes = BitConverter.GetBytes(intValue);
            byte[] result = new byte[2];

            Array.Copy(intBytes, result, 2);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
    }
}
