using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame.auto
{
    public class MessageEncoding
    {
        /// <summary>
        /// 消息体序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Encode(object value)
        {
            SocketModel model = (SocketModel)value;
            ByteArray ba = new ByteArray();
            ba.Write(model.type);
            ba.Write(model.area);
            ba.Write(model.command);
            //判断消息体是否为空 不为空则序列化后写入
            if (model.message != null)
            {
                ba.Write(SerializeUtil.Encode(model.message));
            }
            byte[] result = ba.GetBuff();
            ba.Close();
            return result;
        }

        /// <summary>
        /// 消息体反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Decode(byte[] value)
        {
            ByteArray ba = new ByteArray(value);
            byte _type;
            int _area;
            int _command;
            //从数据中读取 三层协议  读取数据流顺序必须和写入顺序保持一只
            ba.Read(out _type);
            ba.Read(out _area);
            ba.Read(out _command);
            SocketModel model = new SocketModel(_type, _area, _command, null);
            //判断读取完后 是否还有数据需要读取 是则说明有消息体 进行消息体读取
            if (ba.Readnable)
            {
                byte[] _message;
                //讲剩余数据全部读取出来
                ba.Read(out _message, ba.Length - ba.Position);
                //反序列化剩余数据为消息体
                model.message = SerializeUtil.Decode(_message);
            }
            ba.Close();
            return model;
        }
    }
}
