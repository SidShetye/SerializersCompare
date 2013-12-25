using ServiceStack.Text;

namespace SerializersCompare.Serializers
{
    public class ServiceStackJsv : ITestSerializers
    {
        public string GetName()
        {
            return "ServiceStackJSV";
        }

        public bool IsBinary()
        {
            return false;
        }

        public dynamic Serialize<T>(T thisObj)
        {
            return TypeSerializer.SerializeToString(thisObj);
        }

        public T Deserialize<T>(dynamic jsv)
        {
            return TypeSerializer.DeserializeFromString<T>((string)jsv);
        }
    }
}
