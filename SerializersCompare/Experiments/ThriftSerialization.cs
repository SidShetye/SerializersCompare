using System;
using System.IO;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using SerializersCompare.Entities;
using Thrift.Protocol;
using Thrift.Transport;

namespace SerializersCompare.Experiments
{
    class ThriftSerialization
    {
        public void RunExpt1()
        {
            // Prepare input entity, we reuse existing init code 
            // and just Inject (via Value Injecter library)
            var origMsg = new InheritedEntity();
            origMsg.FillDummyData();
            var origMsgJson = JsonConvert.SerializeObject(origMsg);
            Console.WriteLine("Original object is {0}", origMsgJson);


            // Serialization
            var tmsg = new InheritedThriftEntity();
            tmsg.InjectFrom(origMsg);
            var ms = new MemoryStream();
            var tproto = new TCompactProtocol(new TStreamTransport(ms, ms));
            tmsg.Write(tproto);
            var tbytes = ms.ToArray();

            Console.WriteLine("Serialized to {0} bytes: {1}", tbytes.Length, BitConverter.ToString(tbytes));

            // Deserialization
            var ms2 = new MemoryStream(tbytes);
            var tproto2 = new TCompactProtocol(new TStreamTransport(ms2, ms2));
            var regenTMsg = new InheritedThriftEntity();
            regenTMsg.Read(tproto2);
            var regenMsg = new InheritedEntity();
            regenMsg.InjectFrom(regenTMsg);

            var regenMsgJson = JsonConvert.SerializeObject(regenMsg);
            Console.WriteLine("Regenerated object is {0}", regenMsgJson);
        }
    }
}
