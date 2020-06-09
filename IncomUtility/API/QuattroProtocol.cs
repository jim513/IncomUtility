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
    }

    class QuattroProtocol : CRC16
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