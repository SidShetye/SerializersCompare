using System.Collections.Generic;
using ServiceStack.Text;

namespace SerializersCompare.Serializers
{
    public class ServiceStackJson<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "ServiceStackJson";
        }

        public bool IsBinary()
        {
            return false;
        }

        public dynamic Serialize(object thisObj)
        {
            return JsonSerializer.SerializeToString(thisObj);
        }

        public T Deserialize(dynamic json)
        {
            return JsonSerializer.DeserializeFromString<T>((string)json);
        }
    }
}
