using System.Collections.Generic;
using Newtonsoft.Json;

namespace SerializersCompare.Serializers
{
    public class JsonNet<T> : ITestSerializers<T>
    {
        public string GetName()
        {
            return "Json.NET";
        }

        public bool IsBinary()
        {
            return false;
        }

        public dynamic Serialize(object thisObj)
        {
            return JsonConvert.SerializeObject(thisObj);
        }

        public T Deserialize(dynamic json)
        {
            return JsonConvert.DeserializeObject<T>((string)json);
        }
    }
}
