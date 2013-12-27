SerializersCompare
==================
Showcasing a few C# text and binary serializers for performance and size. Feel free to modify and improve.

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
The `cheating` tags above simply mean that Unlike most serializers, Thrift and Avro code-gen their own data classes and those, not the application's existing classes are used in transport and RPC. So most real world projects will certainly bear some extra processing time taken to copy values from their internal app logic classes into the Thrift auto-gen'd classes. In other serializers one can directly use the app logic classes, bypassing the extra copy needed by Thrift. So "Cheating" simply means whether or not we should exclude the data-copy times from Thrift's timings. Real world performance should be with cheating disabled.

Issues
------
Need to see why Avro MSFT is larger than the standard Avro. Avro MSFT uses the inherited C# objects and doesn't have any pre-compilation, code generation phase i.e. it likely computes the schema at run time. Avro standard on the other hand, uses separately created (by hand) non-inherited Avro schema and the tool does code-generation off that schema. So it's very likely that the MSFT generated schema has the hierarchy actually seen in the inherited object and that takes more bytes since it likely encapsulates structures within structures. It should be easy to match the lower byte count if one manually wrote a class without inheritence to match the hand-written schema.