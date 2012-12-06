using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ProtoBuf;

namespace TestSerializers
{
    [Serializable]
    [DataContract]
    class SimpleEntity
    {
        [DataMember(Order = 1)]
        public string message { get; set; }
        
        [DataMember(Order = 2)]
        public string functionCall {get;set;}

        [DataMember(Order = 3)]
        public string parameters { get; set; }

        [DataMember(Order = 4)]
        public string name { get; set; }

        [DataMember(Order = 5)]
        public int employeeId { get; set; }

        [DataMember(Order = 6)]
        public float raiseRate { get; set; }

        [DataMember(Order = 7)]
        public string addressLine1 { get; set; }

        [DataMember(Order = 8)]
        public string addressLine2 { get; set; }

        public SimpleEntity()
        {
        }

        public void FillDummyData()
        {
            message = "Hello World!";
            functionCall = "FunctionNameHere";
            parameters = "x=1,y=2,z=3";
            name = "Mickey Mouse";
            employeeId = 1;
            raiseRate = 1.2F;
            addressLine1 = "1 Disney Street";
            addressLine2 = "Disneyland, CA";
        }
    }

}
