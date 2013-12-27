using System.Collections.Generic;
using System.IO;
using Avro.IO;
using Avro.Specific;
using Omu.ValueInjecter;

namespace SerializersCompare.Serializers
{
    public class Avro<T> : CodeGenSersBase<T, InheritedEntityAvro> where T : new()
    {
        public Avro(bool enableCheating = false): base(enableCheating)
        {

        }

        public override string GetName()
        {
            Name = "Avro";
            return base.GetName();
        }

        public override dynamic Serialize(object thisObj)
        {
            if (CodeGenObjSer == null || !EnableCheating)
                CodeGenObjSer = ToSerObject((T)thisObj);

            using (var ms = new MemoryStream())
            {
                var enc = new BinaryEncoder(ms);
                var writer = new SpecificDefaultWriter(InheritedEntityAvro._SCHEMA); // Schema comes from pre-compiled, code-gen phase
                writer.Write(CodeGenObjSer, enc);

                return ms.ToArray();
            }
        }

        public override T Deserialize(dynamic bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var dec = new BinaryDecoder(ms);
                var reader = new SpecificDefaultReader(InheritedEntityAvro._SCHEMA, InheritedEntityAvro._SCHEMA);
                var regenAvroMsg = new InheritedEntityAvro();
                reader.Read(regenAvroMsg, dec); //TODO: reuse the regenAvroMsg object?

                if (RegenAppObj == null || !EnableCheating)
                    RegenAppObj = FromSerObject(regenAvroMsg);

                return RegenAppObj;
            }
        }

        //private T FromAvroObject(InheritedEntityAvro regenTMsg)
        //{
        //    _regenAppObj = new T();
        //    _regenAppObj.InjectFrom(regenTMsg); // inject most values                
        //    return _regenAppObj;
        //}

        //private InheritedEntityAvro ToAvroObject(T thisObj)
        //{
        //    _codeGenObjSer = new InheritedEntityAvro();
        //    _codeGenObjSer.InjectFrom(thisObj); // inject all values
        //    return _codeGenObjSer;
        //}
    }
}
