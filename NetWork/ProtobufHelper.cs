using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Protobuf������
    /// </summary>
    public static class ProtobufHelper
    {
        /// <summary>
        /// ����
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
        /// ����
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
                //�����ռ���Ϣ  proto.+����
                Type t = Type.GetType("proto."+protoName);
                return (ProtoBuf.IExtensible)ProtoBuf.Serializer.NonGeneric.Deserialize(t, memory);
            }
        }
        /// <summary>
        /// ����Э����
        /// </summary>
        /// <param name="msgBase"></param>
        /// <returns></returns>
        public static byte[] EncodeName(ProtoBuf.IExtensible msgBase)
        {
            //ȥ�������ռ���Ϣ  proto.
            string str = msgBase.ToString().Replace("proto.", "");
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(str.ToString());
            Int16 len = (Int16)nameBytes.Length;
            //����bytes����
            byte[] bytes = new byte[2 + len];
            //��װ2�ֽڵĳ�����Ϣ
            bytes[0] = (byte)(len % 256);
            bytes[1] = (byte)(len / 256);
            //��װ����
            Array.Copy(nameBytes, 0, bytes, 2, len);
            return bytes;
        }
        /// <summary>
        /// ����Э����
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string DecodeName(byte[] bytes, int offset, out int count)
        {
            count = 0;
            //����������ֽ�
            if (offset + 2 > bytes.Length) return "";
            //��ȡ����
            Int16 len = (Int16)((bytes[offset + 1] << 8) | bytes[offset]);
            //��鳤��
            if (len <= 0) return "";
            //��鳤��
            if (offset + 2 + len > bytes.Length) return "";
            //����
            count = 2 + len;
            string name = System.Text.Encoding.UTF8.GetString(bytes, offset + 2, len);
            return name;
        }
    }
}