using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace SerializersCompare.Serializers
{
    public class JsonNetBson<T> : SerializerBase<T>
    {
        public JsonNetBson()
        {
            IsBinarySerializer = true;
            SerName = "Json.NET BSON";
        }

        public override dynamic Serialize(object thisObj)
        {
            using (var ms = new MemoryStream())
            using (var writer = new BsonWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, thisObj);
                SerBytes = ms.ToArray();
                return SerBytes;
            }
        }

        public override T Deserialize(dynamic bson)
        {
            using (var ms = new MemoryStream(bson))
            using (var reader = new BsonReader(ms))
            {
              var serializer = new JsonSerializer();            
              return serializer.Deserialize<T>(reader);
            }            
        }
    }
}
