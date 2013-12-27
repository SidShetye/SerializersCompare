using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializersCompare.Serializers
{
    public class BinFormatter<T> : SerializerBase<T>
    {
        public BinFormatter()
        {
            SerName = "Binary Formatter";
            IsBinarySerializer = true;
        }

        public override dynamic Serialize(object thisObj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter(); 
                bf.Serialize(ms, thisObj);
                SerBytes = ms.ToArray();
                return SerBytes;
            }
        }

        public override T Deserialize(dynamic bytes)
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
