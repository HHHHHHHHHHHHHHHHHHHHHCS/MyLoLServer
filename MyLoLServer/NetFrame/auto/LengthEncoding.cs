using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame.auto
{
    public class LengthEncoding
    {
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="buff">将要传出的数据</param>
        /// <returns>编码后的数据</returns>
        public static byte[] Encode(byte[] buff)
        {
            MemoryStream ms = new MemoryStream();//创建内存流对象
            BinaryWriter bw = new BinaryWriter(ms);//写入二进制对象流
            //写入消息长度
            bw.Write(buff.Length);
            //写入消息体
            bw.Write(buff);
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            bw.Close();
            bw.Dispose();
            ms.Close();
            ms.Dispose();
            return result;
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="cache">缓存对象</param>
        /// <returns>解码后的数据</returns>
        public static byte[] Decode(ref List<byte> cache)
        {
            if(cache.Count<4)
            {//如果长度过小，则不符合规则 直接返回
                return null;
            }

            MemoryStream ms = new MemoryStream(cache.ToArray());//创建内存流对象，并将缓存数据流写入
            BinaryReader br = new BinaryReader(ms);//二进制读取流
            int length = br.ReadInt32();//从缓存中读取int型消息长度
            //如果消息体长度大于 消息长度-消息位置 则说明消息还没有读取完
            if (length>ms.Length-ms.Position)
            {
                br.Close();
                br.Dispose();
                ms.Close();
                ms.Dispose();
                return null;
            }
            //读取正确长度的数据
            byte[] result = br.ReadBytes(length);
            //清空缓存
            cache.Clear();
            //将读取后的剩余数据写入缓存
            cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
            br.Close();
            br.Dispose();
            ms.Close();
            ms.Dispose();
            return result;
        }
    }
}
