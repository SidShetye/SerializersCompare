using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;

namespace TestSerializers
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

        public dynamic Serialize<T>(object thisObj)
        {
            return TypeSerializer.SerializeToString(thisObj);
        }

        public T Deserialize<T>(dynamic jsv)
        {
            return TypeSerializer.DeserializeFromString<T>((string)jsv);
        }
    }
}
