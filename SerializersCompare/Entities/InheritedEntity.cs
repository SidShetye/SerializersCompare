using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace SerializersCompare.Entities
{
    [XmlInclude(typeof(InheritedEntity))]
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
