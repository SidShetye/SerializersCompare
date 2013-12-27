using System.Collections.Generic;
using System.IO;
using Omu.ValueInjecter;
using SerializersCompare.Serializers.Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace SerializersCompare.Serializers
{
    public class Thrift<T> : ITestSerializers<T> where T : new()
    {
        private InheritedThriftEntity _tmsgSer;
        private T _regenMsg;

        private const bool ThriftCheating = true;

        public string GetName()
        {
            return "Thrift";
        }

        public bool IsBinary()
        {
            return true;
        }

        public void Init(IEnumerable<object> args)
        {
        }

        public dynamic Serialize(object thisObj)
        {
            // We do this just once, so that the impact of injecting values from our app object
            // to thrift objects is minimized to just the very first time we do this. 
            // Could be considered 'cheating' since in real apps, it's likely this sort of 
            // object injection/projection might be unavoidable due to thrift design
            if (_tmsgSer == null && ThriftCheating)
                _tmsgSer = ToThriftObject((T)thisObj);

            using (var ms = new MemoryStream())
            {
                var tproto = new TCompactProtocol(new TStreamTransport(ms, ms));
                _tmsgSer.Write(tproto);
                return ms.ToArray();
            }
        }

        public T Deserialize(dynamic bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var tproto = new TCompactProtocol(new TStreamTransport(ms, ms));
                var regenTMsg = new InheritedThriftEntity();
                regenTMsg.Read(tproto);

                if (_regenMsg == null && ThriftCheating)
                    _regenMsg = FromThriftObject(regenTMsg);

                return _regenMsg;
            }
        }

        private T FromThriftObject(InheritedThriftEntity regenTMsg)
        {
            _regenMsg = new T();
            _regenMsg.InjectFrom(regenTMsg) // inject most values
                .InjectFrom<DoubleToFloat>(regenTMsg); // special call to inject the double (thrift) to float (C#)

            return _regenMsg;
        }

        private InheritedThriftEntity ToThriftObject(T thisObj)
        {
            _tmsgSer = new InheritedThriftEntity();
            // Thrift needs to use it's own code-gen proxy objects
            // so no elegant way here :(
            _tmsgSer.InjectFrom(thisObj). // inject all values
                InjectFrom<FloatToDouble>(thisObj);
            // special call to inject the float (C#) into the double (thrift)

            return _tmsgSer;
        }
    }
}
