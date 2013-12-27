using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SerializersCompare.Serializers
{
    public class XmlDotNet<T> : SerializerBase<T>
    {
        public XmlDotNet()
        {
            SerName = "XmlSerializer";
            IsBinarySerializer = false;
        }

        public override dynamic Serialize(object thisObj)
        {
            using (var ms = new MemoryStream())
            {
                var xmlSer = new XmlSerializer(thisObj.GetType());
                xmlSer.Serialize(ms, thisObj);
                ms.Seek(0, SeekOrigin.Begin);

                var reader = new StreamReader(ms, Encoding.UTF8);
                SerString = reader.ReadToEnd();
                return SerString;
            }
        }

        public override T Deserialize(dynamic xml)
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
