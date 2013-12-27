using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SerializersCompare.Serializers
{
    public class XmlDotNet<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "Xml .NET";
        }

        public bool IsBinary()
        {
            return false;
        }

        public dynamic Serialize(object thisObj)
        {
            using (var ms = new MemoryStream())
            {
                var xmlSer = new XmlSerializer(thisObj.GetType());
                xmlSer.Serialize(ms, thisObj);
                ms.Seek(0, SeekOrigin.Begin);

                var reader = new StreamReader(ms, Encoding.UTF8);
                string outString = reader.ReadToEnd();

                return outString;
            }
        }

        public T Deserialize(dynamic xml)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes((string)xml);
            using (var ms = new MemoryStream(byteArray))
            {
                ms.Seek(0, SeekOrigin.Begin);
                var xmlSer = new XmlSerializer(typeof (T));
                return (T) xmlSer.Deserialize(ms);
            }
        }
    }
}
