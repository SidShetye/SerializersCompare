using System;
using System.IO;
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
            Console.WriteLine("Experiment to determine binary/wire compatibility between Avro Apache serializer and Avro MSFT serializer." +
                              "The Microsoft serializer seems to always emit null-able (or optional) fields, so can break compatiblity if the " +
                              "Avro-Apache schema doesn't have optional fields in it's schema.\n");
            
            CompareSchemas();

            try
            {
                AvroMsftToAvro();
                AvroToAvroMsft();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAILED! UNABLE TO OBSERVE BINARY COMPATIBILITY\n");
                Console.WriteLine(ex);
            }
        }

        private void CompareSchemas()
        {
            var eCodeGenEntity = new InheritedEntityAvro();

            // Extra test for schema compare, save schema to compare later
            string avroSchema = eCodeGenEntity.Schema.ToString();

            // Extra test for schema compare, save schema to compare later
            var serializer = new AvroSerializer(typeof(InheritedEntity));

            string avroMsftSchema = serializer.Schema;

            Console.Write("\nThe Avro MSFT schema and the Avro Apache schema ");
            if (String.IsNullOrEmpty(avroSchema) || (avroSchema != avroMsftSchema))
            {
                Console.WriteLine("do NOT MATCH !!!\n");
                Console.WriteLine("Avro Apache schema: {0}", avroSchema);
                Console.WriteLine("Avro MSFT schema  : {0}", avroMsftSchema);
            }
            else
                Console.WriteLine("match.\n");

        }

        private void AvroToAvroMsft()
        {
            // avro ser => avro-msft deser
            
            var e = new InheritedEntity();
            e.FillDummyData();

            // Avro-Apache serialize
            var eCodeGenEntity = new InheritedEntityAvro();
            eCodeGenEntity.InjectFrom(e);
            var eBytes = SerializeAvro(eCodeGenEntity);

            // Avro MSFT deserialize
            var eAppEntityRegen = DeserializeAvroMsft(eBytes); 

            // Compare the object (in JSON, easiest to do so)
            var eJson = JsonConvert.SerializeObject(e);
            var eRegenJson = JsonConvert.SerializeObject(eAppEntityRegen);

            Console.Write("avro ser => avro-msft deser ");
            if (eJson != eRegenJson)
                Console.WriteLine("FAILED !!!");
            else
                Console.WriteLine("passed");

        }

        private void AvroMsftToAvro()
        {
            // avro-msft ser => avro deser

            var e = new InheritedEntity();
            e.FillDummyData();

            // Avro MSFT serialize
            var eBytes = SerializeAvroMsft(e);

            // Avro-Apache deserialize
            var eCodeGenEntity = DeserializeAvro(eBytes);
            var eAppEntityRegen = new InheritedEntity();
            eAppEntityRegen.InjectFrom(eCodeGenEntity);

            // Compare the object (in JSON, easiest to do so)
            var eJson = JsonConvert.SerializeObject(e);
            var eRegenJson = JsonConvert.SerializeObject(eAppEntityRegen);

            Console.Write("avro-msft ser => avro deser ");
            if (eJson != eRegenJson)
                Console.WriteLine("FAILED !!!");
            else
                Console.WriteLine("passed");
        }

        public byte[] SerializeAvroMsft(InheritedEntity thisObj)
        {
            var serializer = new AvroSerializer(thisObj.GetType());
            
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(thisObj, ms);
                return ms.ToArray();
            }
        }

        public InheritedEntity DeserializeAvroMsft(byte[] bytes)
        {
            var serializer = new AvroSerializer(typeof(InheritedEntity));

            using (var ms = new MemoryStream(bytes))
            {
                return serializer.Deserialize<InheritedEntity>(ms);
            }
        }

        public byte[] SerializeAvro(InheritedEntityAvro thisObj)
        {
            using (var ms = new MemoryStream())
            {
                var enc = new BinaryEncoder(ms);
                var writer = new SpecificDefaultWriter(InheritedEntityAvro._SCHEMA); // Schema comes from pre-compiled, code-gen phase
                writer.Write(thisObj, enc);

                return ms.ToArray();
            }
        }

        public InheritedEntityAvro DeserializeAvro(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var dec = new BinaryDecoder(ms);
                var reader = new SpecificDefaultReader(InheritedEntityAvro._SCHEMA, InheritedEntityAvro._SCHEMA);
                var regenAvroMsg = new InheritedEntityAvro();
                reader.Read(regenAvroMsg, dec);

                return regenAvroMsg;
            }
        }
    }
}
