SerializersCompare
==================
Showcasing a few C# text and binary serializers for performance and size. Similar in concept (but not internal design) to https://github.com/eishay/jvm-serializers/wiki but for C# instead of Java. Feel free to improve and send your pull request to me.

Results
-------
    1000 iterations per serializer, average times listed
    Sorting result by size
    Name                Bytes  Time (ms)
    ------------------------------------
    Avro                  141     0.0527
    Avro (cheating)       141     0.0204
    Avro MSFT             149     0.0169
    Thrift                156     0.1495
    Thrift (cheating)     156     0.0075
    ProtoBuf              163     0.0076
    MessagePack           238     0.0306
    ServiceStackJSV       266     0.0162
    Json.NET BSON         294     0.0441
    Json.NET              298     0.0389
    ServiceStackJson      298     0.0171
    Xml .NET              579     0.1092
    Binary Formatter     1024     0.0434

    Options: (T)est, (R)esults, s(O)rt order, (S)erializer output, (D)eserializer output (in JSON form), (E)xit

Notes
-----
The `cheating` tags above simply mean that unlike most serializers, Thrift/Avro code-gen their own data classes and those, not the application's existing classes, are used in transport and RPC. Most real world projects will therefore see extra processing time to copy values from their internal app logic classes into the Thrift/Avro auto-gen'd classes. In contrast, most other serializers (including Avro MSFT) use the app logic classes directly, bypassing the extra copy into the code-gen'd classes. So "cheating" simply means discarding that extra copy time. For real world performance indicators, you should look at figures WITHOUT cheating.

Issues
------
Need to see why Avro MSFT is larger than the standard Avro. Avro MSFT uses the inherited C# objects and doesn't have any pre-compilation, code generation phase i.e. it likely computes the schema at run time. Avro standard on the other hand, uses separately created (by hand) non-inherited Avro schema and the tool does code-generation off that schema. So it's very likely that the MSFT generated schema has the hierarchy actually seen in the inherited object and that takes more bytes since it likely encapsulates structures within structures. It should be easy to match the lower byte count if one manually wrote a class without inheritence to match the hand-written schema.