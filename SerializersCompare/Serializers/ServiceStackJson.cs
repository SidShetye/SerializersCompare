using ServiceStack.Text;

namespace SerializersCompare.Serializers
{
    public class ServiceStackJson<T> : SerializerBase<T>
    {
        public ServiceStackJson()
        {
            SerName = "ServiceStackJson";
            IsBinarySerializer = false;
        }

        public override dynamic Serialize(object thisObj)
        {
            SerString = JsonSerializer.SerializeToString(thisObj);
            return SerString;
        }

        public override T Deserialize(dynamic json)
        {
            return JsonSerializer.DeserializeFromString<T>((string)json);
        }
    }
}
