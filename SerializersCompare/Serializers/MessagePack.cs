using System.IO;
using System.Runtime.CompilerServices;
using MsgPack.Serialization;
using Newtonsoft.Json;
using MsgPack;
using System;

namespace SerializersCompare.Serializers
{
    public class MessagePack : ITestSerializers
    {
        public string GetName()
        {
            return "MessagePack";
        }

        public bool IsBinary()
        {
            return true;
        }

        public dynamic Serialize<T>(object thisObj)
        {
            var serializer = MessagePackSerializer.Create<T>();

            using (var byteStream = new MemoryStream())
            {
                serializer.Pack(byteStream, (T)thisObj);
                return byteStream.ToArray();
            }
        }

        public T Deserialize<T>(dynamic bytes)
        {
            var serializer = MessagePackSerializer.Create<T>();
            using (var byteStream = new MemoryStream(bytes))
            {
                return serializer.Unpack(byteStream);
            }
        }
    }
}
