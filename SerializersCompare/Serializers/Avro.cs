using System.Collections.Generic;
using System.IO;
using Microsoft.Hadoop.Avro;

namespace SerializersCompare.Serializers
{
    public class Avro<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "Avro";
        }

        public bool IsBinary()
        {
            return true;
        }

        public void Init(IEnumerable<object> args)
        {
            //StreamWriter schemafs = new StreamWriter(File.Create(@"SampleData.avsc"));
            //schemafs.Write(serializer.Schema.ToString());
            //schemafs.Close(); 
        }

        // Note, this can be made faster if we generate the schema file (.avsc)
        // first and then use schema files at runtime.
        // We're skipping that step here (code gen) since the rest of the project
        // doesn't support an init() or codegen() phase (yet!)
        public dynamic Serialize(object thisObj)
        {
            var serializer = new AvroSerializer(thisObj.GetType());

            using (var byteStream = new MemoryStream())
            {
                serializer.Serialize(thisObj, byteStream);
                return byteStream.ToArray();
            }
        }

        public T Deserialize(dynamic bytes)
        {
            var serializer = new AvroSerializer(typeof(T));

            using (var ms = new MemoryStream((byte[])bytes))
            {
                return serializer.Deserialize<T>(ms);
            }
        }
    }
}
