﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        
        NUM_USER_NAME = 16,
        NUM_MOBILE_SN = 15,
        NUM_APPLICATION_ID = 32,

        SZ_PARAM_INDEX = 2,

        NUM_CONFIG_PARAM = 111,
        NUM_CYLINDER_SN = 20,

        SZ_EEP_MEMORY = 4096,
        SZ_MAX_MEMORY_BLOCK = 128,
        MEM_TYPE_EEPROM = 1,
        MEM_TYPE_FLASH = 0,

        CERT_NOT_PRESENT = 0,
        CERT_VALID = 1,
        CERT_INVALID = 2,
        CERT_EXPIRED = 3,
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
    public enum SENSOR_TPYE
    {
        ECC = 0,
        FL_CAT = 1,
        IR = 2,
        PID = 3,
        MOS = 4,
    }
    public enum GAS_TYPE
    {
        FLAMMABLE = 0,
        TOXIC = 1,
        O2 = 2,
        VOC = 3,
    }

    public enum CELL_ID
    {
        O2 = 1,
        CO = 2,
        H2S_Low = 3,
        H2S_High = 4,
        H2 = 5,
        SO2 = 6,
        NO2 = 7,
        NH3_Low = 8,
        NH3_High = 9,
        CL2 = 10,
        O3 = 11,
        CH4_IR_LEL =12,
        CO2_IR_vol =13,
        CH4_CAT_LEL =14,
        CO2_IR_LEL =15,
        C3H8_IR = 16,
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
        public int totalSize;

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

            totalSize = tModbusCfg.getSize() + tRelayCfg.getSize() + tmAOutputCfg.getSize() + tGeneralCfg.getSize() + tCircuitCalCfg.getSize() + 26 +
              tDeviceInfo.getSize() + tSecurityCfg.getSize() + tNtcCfg.getSize() + tUL2075Cfg.getSize();
        }
        public byte[] getDataToByteArray()
        {
            byte[] returnData;
            byte[] temp = tModbusCfg.Serialize();
            returnData = temp;

            temp = tRelayCfg.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            temp = tmAOutputCfg.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            temp = tGeneralCfg.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            temp = tCircuitCalCfg.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            returnData = Utility.mergeByteArray(returnData, u8Reserved);

            temp = tDeviceInfo.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            temp = tSecurityCfg.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            temp = tNtcCfg.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            temp = tUL2075Cfg.Serialize();
            returnData = Utility.mergeByteArray(returnData, temp);

            return returnData;
        }

        public bool setDataFromByteArray(byte[] data)
        {
            if (data.Length < totalSize)
                return false;

            int start = 0;
            tModbusCfg.DeSerialize(data, start);
            start += tModbusCfg.getSize();

            tRelayCfg.DeSerialize(data, start);
            start += tRelayCfg.getSize();

            tmAOutputCfg.DeSerialize(data, start);
            start += tmAOutputCfg.getSize();

            tGeneralCfg.DeSerialize(data, start);
            start += tGeneralCfg.getSize();

            tCircuitCalCfg.DeSerialize(data, start);
            start += tCircuitCalCfg.getSize();

            start += 26;

            tDeviceInfo.DeSerialize(data, start);
            start += tDeviceInfo.getSize();

            tSecurityCfg.DeSerialize(data, start);
            start += tSecurityCfg.getSize();

            tNtcCfg.DeSerialize(data, start);
            start += tNtcCfg.getSize();

            tUL2075Cfg.DeSerialize(data, start);

            return true;
        }
    }
    public class CONFIG_MODBUS_STRUCT
    {
        public ushort u16_crc ;
        public byte u8_SlaveId;
        public byte u8_Baudrate;
        public byte u8_Parity;
        public byte u8_FlowCtrl;
        public byte u8_Databits;
        public byte u8_Stopbits;

        private int size = 8;
        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(u8_SlaveId);
                    writer.Write(u8_Baudrate);
                    writer.Write(u8_Parity);
                    writer.Write(u8_FlowCtrl);
                    writer.Write(u8_Databits);
                    writer.Write(u8_Stopbits);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data , int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            u8_SlaveId = data[start + 2];
            u8_Baudrate = data[start + 3];
            u8_Parity = data[start + 4];
            u8_FlowCtrl = data[start + 5];
            u8_Databits = data[start + 6];
            u8_Stopbits = data[start + 7];
        }

        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + 6;
            return size;
        }
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

        private int size ;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(u8_TriggerEvent1);
                    writer.Write(u8_TriggerEvent2);
                    writer.Write(u8_InitialState1);
                    writer.Write(u8_InitialState2);
                    writer.Write(u16_OnDelayTime);
                    writer.Write(u16_OffDelayTime);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            u8_TriggerEvent1 = data[start + 2];
            u8_TriggerEvent2 = data[start + 3];
            u8_InitialState1 = data[start + 4];
            u8_InitialState2 = data[start + 5];
            u16_OnDelayTime = BitConverter.ToUInt16(data, start + 6);
            u16_OffDelayTime = BitConverter.ToUInt16(data, start + 8);
        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + 8;
            return size;
        }
    }

    public class CONFIG_MA_OUPUT_STRUCT
    {
        public ushort u16_crc;
        public byte u8_FaultCurrent;
        public byte u8_WarningCurrent;
        public byte u8_OverrangeCurrent;
        public ushort u16_InhibitTimeout;

        private int size ;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(u8_FaultCurrent);
                    writer.Write(u8_WarningCurrent);
                    writer.Write(u8_OverrangeCurrent);
                    writer.Write(u16_InhibitTimeout);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            u8_FaultCurrent = data[start + 2];
            u8_WarningCurrent = data[start + 3];
            u8_OverrangeCurrent = data[start + 4];
            u16_InhibitTimeout = BitConverter.ToUInt16(data, start + 5);
        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + 5;
            return size;
        }
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

        private int size;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(u8_LocationTag);
                    writer.Write(u8_LEDCtrl);
                    writer.Write(u8_AlarmMode);
                    writer.Write(u8_SafeMode);
                    writer.Write(u8_CalOverDueOption);
                    writer.Write(u8_passcode);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            for (int i = 0; i< (int)INNCOM_CONF.NUM_LOCATION_TAG; i++)
            {
                u8_LocationTag[i] = data[start + 2 + i];
            }
            start += (int)INNCOM_CONF.NUM_LOCATION_TAG;
            u8_LEDCtrl = data[start + 2];
            u8_AlarmMode = data[start + 3];
            u8_SafeMode = data[start + 4];
            u8_CalOverDueOption = data[start + 5];
            for (int i = 0; i < 4; i++)
            {
                u8_passcode[i] = data[start + 6 + i];
            }
        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + 8 + (int)INNCOM_CONF.NUM_LOCATION_TAG;
            return size;
        }
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

        private int size;
        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(f32_mAOutputSinkOffset);
                    writer.Write(f32_mAOutputSourceOffset);
                    writer.Write(f32_mAOutputSinkSpan);
                    writer.Write(f32_mAOutputSourceSpan);
                    writer.Write(f32_mALoopbackSinkOffset);
                    writer.Write(f32_mALoopbackSourceOffset);
                    writer.Write(f32_mALoopbackSinkSpan);
                    writer.Write(f32_mALoopbackSourceSpan);
                    writer.Write(f32_VoltageOutputOffset);
                    writer.Write(f32_VoltageOutputSpan);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            f32_mAOutputSinkOffset = BitConverter.ToSingle(data, start + 2);
            f32_mAOutputSourceOffset = BitConverter.ToSingle(data, start + 6);
            f32_mAOutputSinkSpan = BitConverter.ToSingle(data, start + 10);
            f32_mAOutputSourceSpan = BitConverter.ToSingle(data, start + 14);
            f32_mALoopbackSinkOffset = BitConverter.ToSingle(data, start + 18);
            f32_mALoopbackSourceOffset = BitConverter.ToSingle(data, start + 22);
            f32_mALoopbackSinkSpan = BitConverter.ToSingle(data, start + 26);
            f32_mALoopbackSourceSpan = BitConverter.ToSingle(data, start + 30);
            f32_VoltageOutputOffset = BitConverter.ToSingle(data, start + 34);
            f32_VoltageOutputSpan = BitConverter.ToSingle(data, start + 38);

        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + 40;
            return size;
        }

    }
    public class CONFIG_DEVICE_INFO_STRUCT
    {
        public ushort u16_crc;
        public byte[] u8_DeviceSerialNum = new byte[(int)INNCOM_CONF.NUM_DEVICE_SN];
        public byte[] u8_BoardSerialNum = new byte[(int)INNCOM_CONF.NUM_BOARD_SN];

        private int size;
        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(u8_DeviceSerialNum);
                    writer.Write(u8_BoardSerialNum);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            for (int i = 0; i < (int)INNCOM_CONF.NUM_DEVICE_SN; i++)
            {
                u8_DeviceSerialNum[i] = data[start + 2 + i];
            }

            start += (int)INNCOM_CONF.NUM_DEVICE_SN;

            for (int i = 0; i < (int)INNCOM_CONF.NUM_BOARD_SN; i++)
            {
                u8_BoardSerialNum[i] = data[start + 2 + i];
            }
        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + (int)INNCOM_CONF.NUM_DEVICE_SN + (int)INNCOM_CONF.NUM_BOARD_SN;
            return size;
        }
    }

    public class CONFIG_SECURITY_STRUCT
    {
        public ushort u16_crc;
        public byte u8_num_login_retry;
        public byte u8_login_locktime;
        public byte u8_otp_limits;
        public byte u8_reserved;
        public byte[] u8_otp_key = new byte[(int)INNCOM_CONF.NUM_OTP_KEY];

        private int size = 6 + (int)INNCOM_CONF.NUM_OTP_KEY;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(u8_num_login_retry);
                    writer.Write(u8_login_locktime);
                    writer.Write(u8_otp_limits);
                    writer.Write(u8_reserved);
                    writer.Write(u8_otp_key);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            u8_num_login_retry = data[start + 2];
            u8_login_locktime = data[start + 3];
            u8_otp_limits = data[start + 4];
            u8_reserved = data[start + 5];
            for (int i = 0; i < (int)INNCOM_CONF.NUM_OTP_KEY; i++)
            {
                u8_otp_key[i] = data[start + 6 + i];
            }
        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + 4 + (int)INNCOM_CONF.NUM_OTP_KEY;
            return size;
        }
    }
    public class CONFIG_NTC_COMP_STRUCT
    {
        public ushort u16_crc;
        public sbyte[] s8_NtcTempComp = new sbyte[(int)INNCOM_CONF.NUM_NTC_COMP];
        private int size = 2 + (int)INNCOM_CONF.NUM_NTC_COMP;

        public byte[] Serialize()
        {
            byte[] ntc = (byte[])(object)s8_NtcTempComp;
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(ntc);
                }
                return m.ToArray();
            }
        }
        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            for (int i = 0; i < (int)INNCOM_CONF.NUM_NTC_COMP; i++)
            {
                s8_NtcTempComp[i] = (sbyte)data[start + 2 + i];
            }
        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + (int)INNCOM_CONF.NUM_NTC_COMP;
            return size;
        }
    }

    public class CONFIG_UL2075_HISTOGRAM_STRUCT
    {
        public ushort u16_crc;
        public byte[] u8_Histogram = new byte[(int)INNCOM_CONF.NUM_HISTOGRAM];
        private int size = 2 + (int)INNCOM_CONF.NUM_HISTOGRAM;

        public byte[] Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(u16_crc);
                    writer.Write(u8_Histogram);
                }
                return m.ToArray();
            }
        }

        public void DeSerialize(byte[] data, int start)
        {
            u16_crc = BitConverter.ToUInt16(data, start);
            for (int i = 0; i < (int)INNCOM_CONF.NUM_HISTOGRAM; i++)
            {
                u8_Histogram[i] = data[start + 2 + i];
            }
        }
        public int getSize()
        {
            // class member size
            size = (int)PACKET_CONF.COMM_CRC_SZ + (int)INNCOM_CONF.NUM_HISTOGRAM;
            return size;
        }
    }

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
