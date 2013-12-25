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

        // API usage per http://irisclasson.com/2012/12/17/serializing-and-deserializing-packingunpacking-to-a-file-andor-memorystream-with-messagepack-in-c/

        public dynamic Serialize<T>(T thisObj)
        {
            var serializer = MessagePackSerializer.Create<T>();

            using (var byteStream = new MemoryStream())
            {
                serializer.Pack(byteStream, thisObj);
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
