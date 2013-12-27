using System.IO;
using MsgPack.Serialization;

namespace SerializersCompare.Serializers
{
    public class MessagePack<T> : SerializerBase<T>
    {
        public MessagePack()
        {
            SerName = "MessagePack";
            IsBinarySerializer = true;
        }

        public override dynamic Serialize(object thisObj)
        {
            var serializer = MessagePackSerializer.Create<T>();

            using (var ms = new MemoryStream())
            {
                serializer.Pack(ms, (T)thisObj);
                SerBytes = ms.ToArray();
                return SerBytes;
            }
        }

        public override T Deserialize(dynamic bytes)
        {
            var serializer = MessagePackSerializer.Create<T>();
            using (var byteStream = new MemoryStream(bytes))
            {
                return serializer.Unpack(byteStream);
            }
        }
    }
}
