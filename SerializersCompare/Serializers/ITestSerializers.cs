using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSerializers
{
    public interface ITestSerializers
    {
        string GetName();
        bool IsBinary();
        dynamic Serialize(object thisObj);
        T Deserialize<T>(dynamic serInput);
        //Results TestSerializer(object originalObject, object testObjectAsJson);
    }
}
