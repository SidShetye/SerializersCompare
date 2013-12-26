/**
 * To build the code-gen .cs files run
 * <proj root>\packages\Thrift.0.9.0.0\tools\thrift-0.9.0.exe -gen csharp MultiplicationService.thrift
 * If tools are missing, restore the NuGet packages in this project
 */
 
service MultiplicationService
{
        i32 multiply(1:i32 n1, 2:i32 n2),
}