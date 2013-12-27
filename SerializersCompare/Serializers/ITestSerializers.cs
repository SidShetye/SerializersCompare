namespace SerializersCompare.Serializers
{
    public interface ITestSerializers<T>
    {
        string Name();

        bool IsBinary();

        dynamic Serialize(object thisObj); 

        T Deserialize(dynamic serInput);
    }
}
