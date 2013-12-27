using System.Collections.Generic;
using ServiceStack.Text;

namespace SerializersCompare.Serializers
{
    public class ServiceStackJsv<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "ServiceStackJSV";
        }

        public bool IsBinary()
        {
            return false;
        }

        public void Init(IEnumerable<object> args)
        {

        }


        public dynamic Serialize(object thisObj)
        {
            return TypeSerializer.SerializeToString(thisObj);
        }

        public T Deserialize(dynamic jsv)
        {
            return TypeSerializer.DeserializeFromString<T>((string)jsv);
        }
    }
}
