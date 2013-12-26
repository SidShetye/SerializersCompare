using System.IO;
using MsgPack.Serialization;

namespace SerializersCompare.Serializers
{
    public class MessagePack<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "MessagePack";
        }

        public bool IsBinary()
        {
            return true;
        }

        public void Init()
        {

        }

        public dynamic Serialize(object thisObj)
        {
            var serializer = MessagePackSerializer.Create<T>();

            using (var byteStream = new MemoryStream())
            {
                serializer.Pack(byteStream, (T)thisObj);
                return byteStream.ToArray();
            }
        }

        public T Deserialize(dynamic bytes)
        {
            var serializer = MessagePackSerializer.Create<T>();
            using (var byteStream = new MemoryStream(bytes))
            {
                return serializer.Unpack(byteStream);
            }
        }
    }
}
