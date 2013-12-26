using ProtoBuf;
using System.IO;

namespace SerializersCompare.Serializers
{
    public class ProtoBuf<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "ProtoBuf";
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
            using (var ms = new MemoryStream())
            {
                Serializer.NonGeneric.Serialize(ms, thisObj);
                return ms.ToArray();
            }
        }

        public T Deserialize(dynamic bytes)
        {

            using (var ms = new MemoryStream((byte[])bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
