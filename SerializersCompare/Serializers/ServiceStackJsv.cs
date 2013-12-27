using ServiceStack.Text;

namespace SerializersCompare.Serializers
{
    public class ServiceStackJsv<T> : SerializerBase<T>
    {
        public ServiceStackJsv()
        {
            SerName = "ServiceStackJSV";
            IsBinarySerializer = false;
        }

        
        public override dynamic Serialize(object thisObj)
        {
            SerString = TypeSerializer.SerializeToString(thisObj);
            return SerString;
        }

        public override T Deserialize(dynamic jsv)
        {
            return TypeSerializer.DeserializeFromString<T>((string)jsv);
        }
    }
}
