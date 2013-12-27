using System.Collections.Generic;

namespace SerializersCompare.Serializers
{
    public interface ITestSerializers<T>
    {
        string GetName();
        bool IsBinary();

        void Init(IEnumerable<object> args);

        dynamic Serialize(object thisObj); // Most serializers don't need the <T> generic
                                              // but few DO (eg .NET XML).
        T Deserialize(dynamic serInput);
    }
}
