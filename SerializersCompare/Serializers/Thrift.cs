using System.IO;
using Omu.ValueInjecter;
using SerializersCompare.Serializers.Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace SerializersCompare.Serializers
{
    public class Thrift<T> : CodeGenSersBase<T, InheritedEntityThrift> where T : new()
    {
        public Thrift(bool cheating = false)
            : base(cheating)
        {
            IsBinarySerializer = true;
            SerName = "Thrift";
        }

        public override dynamic Serialize(object thisObj)
        {
            DoCopyIfNotCheating((T)thisObj);

            using (var ms = new MemoryStream())
            {
                var tproto = new TCompactProtocol(new TStreamTransport(ms, ms));
                CodeGenObjSer.Write(tproto);
                SerBytes = ms.ToArray();
            }

            return SerBytes;
        }

        public override T Deserialize(dynamic bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var tproto = new TCompactProtocol(new TStreamTransport(ms, ms));
                ReuseDeserObj.Read(tproto);
                DoCopyIfNotCheating(ReuseDeserObj);
            }

            return RegenAppObj;
        }

        protected override T FromSerObject(InheritedEntityThrift regenTMsg)
        {
            RegenAppObj = base.FromSerObject(regenTMsg);            
            RegenAppObj.InjectFrom<DoubleToFloat>(regenTMsg); // special call to inject the double (thrift) to float (C#)
            return RegenAppObj;
        }

        protected override InheritedEntityThrift ToSerObject(T thisObj)
        {
            CodeGenObjSer = base.ToSerObject(thisObj);
            CodeGenObjSer.InjectFrom<FloatToDouble>(thisObj); // special call to inject the float (C#) into the double (thrift)
            return CodeGenObjSer;
        }
    }
}
