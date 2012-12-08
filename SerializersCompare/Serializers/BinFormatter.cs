using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestSerializers
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

        public dynamic Serialize<T>(object thisObj)
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
