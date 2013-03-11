using ProtoBuf;
using System.IO;

namespace SerializersCompare.Serializers
{
    public class ProtoBuf : ITestSerializers
    {
        public string GetName()
        {
            return "ProtoBuf";
        }

        public bool IsBinary()
        {
            return true;
        }

        public dynamic Serialize<T>(object thisObj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.NonGeneric.Serialize(ms, thisObj);
                return ms.ToArray();
            }
        }

        public T Deserialize<T>(dynamic bytes)
        {

            using (MemoryStream ms = new MemoryStream((byte[])bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
