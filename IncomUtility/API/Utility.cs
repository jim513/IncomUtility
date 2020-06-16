using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        public static bool isTimeCheck(int year, int month, int day, int hour, int minute, int second)
        {
            if (year < 999 || year > 10000)
            {
                return false;
            }

            if (month < 1 || month > 12)
            {
                return false;
            }

            if (day < 1 || day > 31)
            {
                return false;
            }

            if (hour < 0 || hour > 24)
            {
                return false;
            }

            if (minute < 0 || minute > 60)
            {
                return false;
            }

            if (second < 0 || second > 60)
            {
                return false;
            }

            return true;
        }
        public static bool isTimeCheck(int year, int month, int day)
        {
            if (year < 999 || year > 10000)
            {
                return false;
            }

            if (month < 1 || month > 12)
            {
                return false;
            }

            if (day < 1 || day > 31)
            {
                return false;
            }

            return true;
        }
        public static float getF32FromByteA(byte[] data, int offset)
        {

            if (data.Length < offset + 4)
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
            if (data.Length < offset + 4)
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
            if (data.Length < offset + 4)
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
            if (data.Length < offset + 2)
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
            if (data.Length < offset + 2)
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

        public static byte[] getBytesFromU32(int data)
        {

            byte[] getBytes = BitConverter.GetBytes(data);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(getBytes);

            return getBytes;
        }
        public static byte[] getBytesFromF32(float data)
        {

            byte[] getBytes = BitConverter.GetBytes(data);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(getBytes);

            return getBytes;
        }
        public static byte[] getBytesFromU16(ushort data)
        {

            byte[] getBytes = BitConverter.GetBytes(data);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(getBytes);

            return getBytes;
        }
        public static bool checkBitPos(byte data, int bit_pos)
        {
            return (data & (1 << bit_pos)) != 0;
        }
        public static byte setBitPos(byte data , int bit_pos)
        {
            return data |= (byte)(0x01 << bit_pos);
        }
        public static T ByteToStruct<T>(byte[] buffer) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            if (size > buffer.Length)
            {
                throw new Exception();
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, 0, ptr, size);
            T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return obj;
        }
        public static byte[] StructToByte(object obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
      
    }
}
