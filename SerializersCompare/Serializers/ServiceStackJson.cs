using ServiceStack.Text;

namespace SerializersCompare.Serializers
{
    public class ServiceStackJson : ITestSerializers
    {
        public string GetName()
        {
            return "ServiceStackJson";
        }

        public bool IsBinary()
        {
            return false;
        }

        public dynamic Serialize<T>(object thisObj)
        {
            return JsonSerializer.SerializeToString(thisObj);
        }

        public T Deserialize<T>(dynamic json)
        {
            return JsonSerializer.DeserializeFromString<T>((string)json);
        }
    }
}
