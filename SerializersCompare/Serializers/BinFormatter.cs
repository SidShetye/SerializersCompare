using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializersCompare.Serializers
{
    public class BinFormatter<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "Binary Formatter";
        }

        public bool IsBinary()
        {
            return true;
        }

        public dynamic Serialize(object thisObj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter(); 
                bf.Serialize(ms, thisObj);
                return ms.GetBuffer();
            }
        }

        public T Deserialize(dynamic bytes)
        {
            using (var ms = new MemoryStream((byte[])bytes))
            {
                var bf = new BinaryFormatter();
                ms.Read(bytes, 0, 0);
                return (T)bf.Deserialize(ms);
            }
        }
    }
}
