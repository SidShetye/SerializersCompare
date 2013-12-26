using System.IO;
using Microsoft.Hadoop.Avro;
using Omu.ValueInjecter;
using Thrift.Protocol;
using Thrift.Transport;

namespace SerializersCompare.Serializers
{
    public class Thrift<T> : ITestSerializers<T> where T : new()
    {
        public string GetName()
        {
            return "Thrift";
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
            // Thrift needs to use it's own code-gen proxy objects
            // so no elegant way here :(
            var tmsg = new InheritedThriftEntity();
            tmsg.InjectFrom((T)thisObj);

            using (var ms = new MemoryStream())
            {
                var tproto = new TCompactProtocol(new TStreamTransport(ms, ms));
                tmsg.Write(tproto);
                return ms.ToArray();
            }
        }

        public T Deserialize(dynamic bytes)
        {
            using (var ms2 = new MemoryStream(bytes))
            {
                var tproto2 = new TCompactProtocol(new TStreamTransport(ms2, ms2));
                var regenTMsg = new InheritedThriftEntity();
                regenTMsg.Read(tproto2);
                var regenMsg = new T();
                regenMsg.InjectFrom(regenTMsg);

                return regenMsg;
            }
        }
    }
}
