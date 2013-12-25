using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace SerializersCompare.Serializers
{
    public class JsonNETBSON : ITestSerializers
    {
        public string GetName()
        {
            return "Json.NET BSON";
        }

        public bool IsBinary()
        {
            return true;
        }

        public dynamic Serialize<T>(T thisObj)
        {
            var ms = new MemoryStream();
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, thisObj);
            }

            return ms.ToArray();
        }

        public T Deserialize<T>(dynamic bson)
        {
            var ms = new MemoryStream(bson);
            using (var reader = new BsonReader(ms))
            {
              var serializer = new JsonSerializer();            
              return serializer.Deserialize<T>(reader);
            }            
        }
    }
}
