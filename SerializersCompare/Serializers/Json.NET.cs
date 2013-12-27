using Newtonsoft.Json;

namespace SerializersCompare.Serializers
{
    public class JsonNet<T> : SerializerBase<T>
    {
        public JsonNet()
        {
            IsBinarySerializer = false;
            SerName = "Json.NET";
        }

        public override dynamic Serialize(object thisObj)
        {
            SerString = JsonConvert.SerializeObject(thisObj);
            return SerString;
        }

        public override T Deserialize(dynamic json)
        {
            return JsonConvert.DeserializeObject<T>((string)json);
        }
    }
}
