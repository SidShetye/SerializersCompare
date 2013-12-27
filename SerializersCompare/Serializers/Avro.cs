using System.IO;
using Avro.IO;
using Avro.Specific;

namespace SerializersCompare.Serializers
{
    public class Avro<T> : CodeGenSersBase<T, InheritedEntityAvro> where T : new()
    {
        public Avro(bool cheating = false): base(cheating)
        {
            IsBinarySerializer = true;
            SerName = "Avro";
        }

        public override dynamic Serialize(object thisObj)
        {
            DoCopyIfNotCheating((T)thisObj);

            using (var ms = new MemoryStream())
            {
                var enc = new BinaryEncoder(ms);
                var writer = new SpecificDefaultWriter(InheritedEntityAvro._SCHEMA); // Schema comes from pre-compiled, code-gen phase
                writer.Write(CodeGenObjSer, enc);

                SerBytes = ms.ToArray();
                return SerBytes;
            }
        }

        public override T Deserialize(dynamic bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var dec = new BinaryDecoder(ms);
                var reader = new SpecificDefaultReader(InheritedEntityAvro._SCHEMA, InheritedEntityAvro._SCHEMA);
                reader.Read(ReuseDeserObj, dec);
                DoCopyIfNotCheating(ReuseDeserObj);

                return RegenAppObj;
            }
        }
    }
}
