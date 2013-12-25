using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SerializersCompare.Serializers
{
    public class BinFormatter : ITestSerializers
    {
        public string GetName()
        {
            return "Binary Formatter";
        }

        public bool IsBinary()
        {
            return true;
        }

        public dynamic Serialize<T>(T thisObj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter(); 
                bf.Serialize(ms, thisObj);
                return ms.GetBuffer();
            }
        }

        public T Deserialize<T>(dynamic bytes)
        {
            using (MemoryStream ms = new MemoryStream((byte[])bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                ms.Read(bytes, 0, 0);
                return (T)bf.Deserialize(ms);
            }
        }
    }
}
