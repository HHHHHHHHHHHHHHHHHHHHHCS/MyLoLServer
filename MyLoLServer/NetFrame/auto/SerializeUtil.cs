using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame.auto
{
    public class SerializeUtil
    {
        /// <summary>
        /// 讲对象序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Encode(object value)
        {
            MemoryStream ms = new MemoryStream();//创建编码解码的内存流对象
            BinaryFormatter bf = new BinaryFormatter();//二进制序列化对象
            bf.Serialize(ms, value);//将obj对象序列化成二进制数据 写入到内存流
            byte[] result = new byte[ms.Length];
            //讲流数据拷贝到结果数组
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
            ms.Close();
            ms.Dispose();
            return result;
        }

        public static object Decode(byte[] value)
        {
            MemoryStream ms = new MemoryStream(value);//创建编码解码的内存流对象 并讲需要反序列化的数据写入其中
            BinaryFormatter bf = new BinaryFormatter();//二进制序列化对象
            //讲流数据反序列话为obj对象
            object result = bf.Deserialize(ms);//将obj对象序列化成二进制数据 写入到内存流
            ms.Close();
            ms.Dispose();
            return result;
        }
    }
}
