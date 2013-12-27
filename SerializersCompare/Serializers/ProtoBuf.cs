using ProtoBuf;
using System.IO;

namespace SerializersCompare.Serializers
{
    public class ProtoBuf<T> : SerializerBase<T>
    {
        public ProtoBuf()
        {
            SerName = "ProtoBuf";
            IsBinarySerializer = true;
        }

        public override dynamic Serialize(object thisObj)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.NonGeneric.Serialize(ms, thisObj);
                SerBytes = ms.ToArray();
                return SerBytes;
            }
        }

        public override T Deserialize(dynamic bytes)
        {

            using (var ms = new MemoryStream((byte[])bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
