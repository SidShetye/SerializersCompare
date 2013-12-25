using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SerializersCompare.Serializers
{
    public class XmlDotNet : ITestSerializers
    {
        public string GetName()
        {
            return "Xml .NET";
        }

        public bool IsBinary()
        {
            return false;
        }

        public dynamic Serialize<T>(T thisObj)
        {
            MemoryStream ms = new MemoryStream();
            XmlSerializer xmlSer = new XmlSerializer(thisObj.GetType());
            xmlSer.Serialize(ms, thisObj);
            ms.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(ms, Encoding.UTF8);
            string outString = reader.ReadToEnd();

            return outString;
        }

        public T Deserialize<T>(dynamic xml)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes((string)xml);
            MemoryStream ms = new MemoryStream(byteArray);
            ms.Seek(0, SeekOrigin.Begin);

            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            return (T)xmlSer.Deserialize(ms);
        }
    }
}
