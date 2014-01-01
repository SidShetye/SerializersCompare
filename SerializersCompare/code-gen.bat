@echo off
echo Running Avro code generation tool ...
REM ..\lib\avro-1.7.5\codegen\Release\avrogen.exe -s Serializers\Avro\InheritedEntityAvroWithNulls.avsc ..
..\lib\avro-1.7.5\codegen\Release\avrogen.exe -s Serializers\Avro\InheritedEntityAvro.avsc ..
..\lib\avro-1.7.5\codegen\Release\avrogen.exe -s Serializers\Avro\SimpleEntityAvro.avsc ..
move Serializers\InheritedEntityAvro.cs Serializers\Avro
move Serializers\SimpleEntityAvro.cs Serializers\Avro

echo Running Thrift code generation tool ...
..\lib\thrift-0.9.1\thrift-0.9.1.exe -out Serializers\Thrift\ -gen csharp Serializers\Thrift\InheritedEntityThrift.thrift
