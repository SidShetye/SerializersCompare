using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;

namespace TestSerializers
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
