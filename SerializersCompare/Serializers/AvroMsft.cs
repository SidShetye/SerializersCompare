using System.IO;
using System.Text;
using Microsoft.Hadoop.Avro;

namespace SerializersCompare.Serializers
{
    public class AvroMsft<T> : SerializerBase<T>
    {
        private bool _schemaSaved = false;

        public AvroMsft()
        {
            // This is the Mcirosoft API into Avro, makes more sense within the 
            // C# world (uses DataContract decorators)
            SerName = "Avro MSFT";
            IsBinarySerializer = true;
        }

        // Note, this can be made faster if we generate the schema file (.avsc)
        // first and then use schema files at runtime.
        // We're skipping that step here (code gen) since the rest of the project
        // doesn't support an init() or codegen() phase (yet!)
        public override dynamic Serialize(object thisObj)
        {
            var serializer = new AvroSerializer(thisObj.GetType());

            SaveSchema(serializer);

            using (var ms = new MemoryStream())
            {
                serializer.Serialize(thisObj, ms);
                SerBytes = ms.ToArray();
                return SerBytes;
            }
        }

        public override T Deserialize(dynamic bytes)
        {
            var serializer = new AvroSerializer(typeof(T));

            using (var ms = new MemoryStream((byte[])bytes))
            {
                return serializer.Deserialize<T>(ms);
            }
        }

        private void SaveSchema(AvroSerializer serializer)
        {
            if (_schemaSaved)
                return;

            using (var fs = File.OpenWrite(IODir + "\\" + Name() + ".avsc"))
            {
                var bytes = Encoding.UTF8.GetBytes(serializer.Schema);
                fs.Write(bytes, 0, bytes.Length);
            }

            _schemaSaved = true;
        }
    }
}
