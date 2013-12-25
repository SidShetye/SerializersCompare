using Newtonsoft.Json;

namespace SerializersCompare.Serializers
{
    public class JsonNET : ITestSerializers
    {
        public string GetName()
        {
            return "Json.NET";
        }

        public bool IsBinary()
        {
            return false;
        }

        public dynamic Serialize<T>(T thisObj)
        {
            return JsonConvert.SerializeObject(thisObj);
        }

        public T Deserialize<T>(dynamic json)
        {
            return JsonConvert.DeserializeObject<T>((string)json);
        }
    }
}
