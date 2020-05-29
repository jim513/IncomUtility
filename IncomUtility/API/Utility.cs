using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomUtility
{
    public static class Utility
    {
        public static byte[] hexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static byte[] mergeByteArray(byte[] a, byte[] b)
        {
            return a.Concat(b).ToArray();
        }
        public static float getF32FromByteA(byte[] data, int offset)
        {

            if (data.Length < 4)
                return -1;
            if (offset > data.Length || offset < 0)
                return -1;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
                offset = data.Length - offset - 4;
                float ret =  BitConverter.ToSingle(data, offset);
                Array.Reverse(data);
                return ret;
            }
            else
                return BitConverter.ToSingle(data, offset);

        }
        public static int getS32FromByteA(byte[] data, int offset)
        {
            if (data.Length < 4)
                return -1;
            if (offset > data.Length || offset < 0)
                return -1;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
                offset = data.Length - offset - 4;
                int ret = BitConverter.ToInt32(data, offset);
                Array.Reverse(data);
                return ret;
            }
            else
                return BitConverter.ToInt32(data, offset);


        }
        public static uint getU32FromByteA(byte[] data, int offset)
        {
            if (data.Length < 4)
                return 0;
            if (offset > data.Length || offset < 0)
                return 0;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
                offset = data.Length - offset - 4;
                uint ret = BitConverter.ToUInt32(data, offset);
                Array.Reverse(data);
                return ret;
            }
            else
                return BitConverter.ToUInt32(data, offset);
        }

        public static short getS16FromByteA(byte[] data, int offset)
        {
            if (data.Length < 2)
                return -1;
            if (offset > data.Length || offset < 0)
                return -1;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
                offset = data.Length - offset - 2;
                short ret =  BitConverter.ToInt16(data, offset);
                Array.Reverse(data);
                return ret;
            }
            else
                return BitConverter.ToInt16(data, offset);
        }
        public static ushort getU16FromByteA(byte[] data, int offset)
        {
            if (data.Length < 2)
                return 0;
            if (offset > data.Length || offset < 0)
                return 0;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
                offset = data.Length - offset - 2;
                ushort ret= BitConverter.ToUInt16(data, offset);
                Array.Reverse(data);
                return ret;
            }
            else
                return BitConverter.ToUInt16(data, offset);
        }
    }
}
