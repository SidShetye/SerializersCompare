/*
 * To build the code-gen .cs files run
 * <proj root>\packages\Thrift.0.9.0.0\tools\thrift-0.9.0.exe -gen csharp SimpleAndInheritedEntity.thrift
 * If tools are missing, restore the NuGet packages in this project
 */

/* This is manually "inherited" to fit the 
 * rest of the serializers compared in this project
 *
 * See https://git-wip-us.apache.org/repos/asf?p=thrift.git;a=blob_plain;f=tutorial/tutorial.thrift
 * for basics on how to create a .thrift file like this one
 */
struct InheritedThriftEntity {
  10:  string   Message,
  20:  string   FunctionCall,
  30:  string   Parameters,
  40:  string   Name,
  50:  i32      EmployeeId,
  60:  double   RaiseRate,
  70:  string   AddressLine1,
  80:  string   AddressLine2,
  90:  binary   Icon,
  100: binary   LargeIcon,
}