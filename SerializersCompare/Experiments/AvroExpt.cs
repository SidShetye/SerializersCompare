using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avro.IO;
using Avro.Specific;
using Microsoft.Hadoop.Avro;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using SerializersCompare.Entities;
using SerializersCompare.Serializers;

namespace SerializersCompare.Experiments
{
    public class AvroExpt
    {
        public void RunExpt1()
        {
            AvroMsftToAvro();
            AvroToAvroMsft();
        }

        private void AvroToAvroMsft()
        {
            // avro ser => avro-msft deser
            var e = new SimpleEntity();
            e.FillDummyData();
            var eJson = JsonConvert.SerializeObject(e);

            var eCodeGenEntity = new SimpleEntityAvro();
            eCodeGenEntity.InjectFrom(e);

            var eBytes = SerializeAvro(eCodeGenEntity);
            var eAppEntityRegen = DeserializeAvroMsft(eBytes);

            var eRegenJson = JsonConvert.SerializeObject(eAppEntityRegen);

            if (eJson != eRegenJson)
                Console.WriteLine("avro ser => avro-msft deser FAILED !!!");
            else
                Console.WriteLine("passed");
        }

        private void AvroMsftToAvro()
        {
            // avro-msft ser => avro deser
            var e = new SimpleEntity();
            e.FillDummyData();
            var eJson = JsonConvert.SerializeObject(e);

            // ser
            var eBytes = SerializeAvroMsft(e);

            //deser
            var eCodeGenEntity = DeserializeAvro(eBytes);
            var eAppEntityRegen = new SimpleEntity();
            eAppEntityRegen.InjectFrom(eCodeGenEntity);

            var eRegenJson = JsonConvert.SerializeObject(eAppEntityRegen);

            if (eJson != eRegenJson)
                Console.WriteLine("avro-msft ser => avro deser FAILED !!!");
            else
                Console.WriteLine("passed");
        }

        public byte[] SerializeAvroMsft(SimpleEntity thisObj)
        {
            var serializer = new AvroSerializer(thisObj.GetType());

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(thisObj, ms);
                return ms.ToArray();
            }
        }

        public SimpleEntity DeserializeAvroMsft(byte[] bytes)
        {
            var serializer = new AvroSerializer(typeof(SimpleEntity));

            using (var ms = new MemoryStream(bytes))
            {
                return serializer.Deserialize<SimpleEntity>(ms);
            }
        }

        public byte[] SerializeAvro(SimpleEntityAvro thisObj)
        {
            using (var ms = new MemoryStream())
            {
                var enc = new BinaryEncoder(ms);
                var writer = new SpecificDefaultWriter(SimpleEntityAvro._SCHEMA); // Schema comes from pre-compiled, code-gen phase
                writer.Write(thisObj, enc);

                return ms.ToArray();
            }
        }

        public SimpleEntityAvro DeserializeAvro(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var dec = new BinaryDecoder(ms);
                var reader = new SpecificDefaultReader(SimpleEntityAvro._SCHEMA, SimpleEntityAvro._SCHEMA);
                var regenAvroMsg = new SimpleEntityAvro();
                reader.Read(regenAvroMsg, dec);

                return regenAvroMsg;
            }
        }
    }
}
