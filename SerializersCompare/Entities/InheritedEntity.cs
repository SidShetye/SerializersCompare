using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace SerializersCompare.Entities
{
    [Serializable]
    [DataContract]
    public class InheritedEntity : SimpleEntity
    {
        [DataMember(Order = 100)]
        public byte[] LargeIcon { get; set; }

        public new void FillDummyData()
        {
            base.FillDummyData();
            LargeIcon = new byte[32];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(LargeIcon);
        }
    }
}
