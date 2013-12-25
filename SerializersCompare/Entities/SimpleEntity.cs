using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using ProtoBuf;

namespace SerializersCompare.Entities
{
    [Serializable]
    [DataContract]
    [ProtoInclude(100, typeof(InheritedEntity))]
    public class SimpleEntity
    {
        [DataMember(Order = 10)]
        public string Message { get; set; }

        [DataMember(Order = 20)]
        public string FunctionCall {get;set;}

        [DataMember(Order = 30)]
        public string Parameters { get; set; }

        [DataMember(Order = 40)]
        public string Name { get; set; }

        [DataMember(Order = 50)]
        public int EmployeeId { get; set; }

        [DataMember(Order = 60)]
        public float RaiseRate { get; set; }

        [DataMember(Order = 70)]
        public string AddressLine1 { get; set; }

        [DataMember(Order = 80)]
        public string AddressLine2 { get; set; }

        [DataMember(Order = 90)]
        public byte[] Icon { get; set; }    

        public void FillDummyData()
        {
            Message = "Hello World!";
            FunctionCall = "FunctionNameHere";
            Parameters = "x=1,y=2,z=3";
            Name = "Mickey Mouse";
            EmployeeId = 1;
            RaiseRate = 1.2F;
            AddressLine1 = "1 Disney Street";
            AddressLine2 = "Disneyland, CA";

            Icon = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(Icon);
        }
    }

}
