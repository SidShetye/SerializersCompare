@echo off
echo Running Avro code generation tool ...
..\lib\avro-csharp-1.7.5\codegen\Release\avrogen.exe -s Serializers\Avro\InheritedEntity.avsc ..
move Serializers\InheritedEntityAvro.cs Serializers\Avro

echo Running Thrift code generation tool ...
..\lib\thrift-0.9.1\thrift-0.9.1.exe -out Serializers\Thrift\ -gen csharp Serializers\Thrift\InheritedEntityThrift.thrift
