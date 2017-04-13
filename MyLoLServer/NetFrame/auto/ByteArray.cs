using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetFrame.auto
{
    /// <summary>
    /// 将数据写入成二进制
    /// </summary>
    public class ByteArray
    {
        MemoryStream ms = new MemoryStream();

        BinaryWriter bw;
        BinaryReader br;
        public void Close()
        {
            bw.Close();
            bw.Dispose();
            br.Close();
            br.Dispose();
            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        /// 支持传入初始数据的构造
        /// </summary>
        /// <param name="buff"></param>
        public ByteArray(byte[] buff)
        {
            ms = new MemoryStream(buff);
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }

        /// <summary>
        /// 获取当前数据 读取到的下标位置
        /// </summary>
        public int Position
        {
            get { return (int)ms.Position; }
        }

        /// <summary>
        /// 获取当前数据长度
        /// </summary>
        public int Length
        {
            get { return (int)ms.Length; }
        }
        /// <summary>
        /// 当前是否还有数据可以读取
        /// </summary>
        public bool Readnable
        {
            get { return ms.Length > ms.Position; }
        }

        /// <summary>
        /// 默认构造
        /// </summary>
        public ByteArray()
        {
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }

        public void Write(int value)
        {
            bw.Write(value);
        }
        public void Write(byte value)
        {
            bw.Write(value);
        }
        public void Write(bool value)
        {
            bw.Write(value);
        }
        public void Write(string value)
        {
            bw.Write(value);
        }
        public void Write(byte[] value)
        {
            bw.Write(value);
        }

        public void Write(double value)
        {
            bw.Write(value);
        }
        public void Write(float value)
        {
            bw.Write(value);
        }
        public void Write(long value)
        {
            bw.Write(value);
        }


        public void Read(out int value)
        {
            value = br.ReadInt32();
        }
        public void Read(out byte value)
        {
            value = br.ReadByte();
        }
        public void Read(out bool value)
        {
            value = br.ReadBoolean();
        }
        public void Read(out string value)
        {
            value = br.ReadString();
        }
        public void Read(out byte[] value, int length)
        {
            value = br.ReadBytes(length);
        }

        public void Read(out double value)
        {
            value = br.ReadDouble();
        }
        public void Read(out float value)
        {
            value = br.ReadSingle();
        }
        public void Read(out long value)
        {
            value = br.ReadInt64();
        }

        public void RePosition()
        {
            ms.Position = 0;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuff()
        {
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            return result;
        }
    }
}
