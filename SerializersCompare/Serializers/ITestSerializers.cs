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
        dynamic Serialize<T>(object thisObj); // Most serializers don't need the <T> generic
                                              // but few DO (eg .NET XML).
        T Deserialize<T>(dynamic serInput);
    }
}
