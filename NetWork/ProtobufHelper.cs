using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Protobuf解析类
    /// </summary>
    public static class ProtobufHelper
    {
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="msgBase"></param>
        /// <returns></returns>
        public static byte[] Encode(ProtoBuf.IExtensible msgBase)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(memory, msgBase);
                return memory.ToArray();
            }
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="protoName"></param>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static ProtoBuf.IExtensible Decode(string protoName, byte[] bytes, int offset, int count)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream(bytes, offset, count))
            {
                //命名空间信息  proto.+类名
                Type t = Type.GetType("proto."+protoName);
                return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t, memory);
            }
        }
        /// <summary>
        /// 编码协议名
        /// </summary>
        /// <param name="msgBase"></param>
        /// <returns></returns>
        public static byte[] EncodeName(ProtoBuf.IExtensible msgBase)
        {
            //去掉命名空间信息  proto.
            string str = msgBase.ToString().Replace("proto.", "");
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(str.ToString());
            Int16 len = (Int16)nameBytes.Length;
            //申请bytes数组
            byte[] bytes = new byte[2 + len];
            //组装2字节的长度信息
            bytes[0] = (byte)(len % 256);
            bytes[1] = (byte)(len / 256);
            //组装名字
            Array.Copy(nameBytes, 0, bytes, 2, len);
            return bytes;
        }
        /// <summary>
        /// 解析协议名
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string DecodeName(byte[] bytes, int offset, out int count)
        {
            count = 0;
            //必须大于两字节
            if (offset + 2 > bytes.Length) return "";
            //读取长度
            Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
            //检查长度
            if (len <= 0) return "";
            //检查长度
            if (offset + 2 + len > bytes.Length) return "";
            //解析
            count = 2 + len;
            string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
            return name;
        }
    }
}