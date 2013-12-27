using System;
using System.Diagnostics;
using System.IO;
using Org.BouncyCastle.Asn1;
using SerializersCompare.Entities;

namespace SerializersCompare.Experiments
{
    public class Asn1Experiments
    {
        public void RunExpt1()
        {
            // Quick copy-cat experiment trying to DER encode the InheritedEntity object
            // but by hand here
            var e = new InheritedEntity();
            e.FillDummyData();

            const int iterations = 1000;
            var asn1DerBytes= new byte[0];

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterations; i++)
                asn1DerBytes = SerializeAsn1(e);
            sw.Stop();

            // x2 since we're only serializing here, (big) assumption
            // that a ser->deser route will be double, but ok for coarse measures
            var avgMs = sw.Elapsed.TotalMilliseconds*2/iterations;

            File.WriteAllBytes("Asn1Expt.der", asn1DerBytes);
            Console.WriteLine("Serialized via ASN.1 DER encoding to {0} bytes in {1,4:n4} ms: {2}", 
                asn1DerBytes.Length,
                avgMs,
                BitConverter.ToString(asn1DerBytes));
        }

        private static byte[] SerializeAsn1(InheritedEntity e)
        {
            using (var ms = new MemoryStream())
            {
                var s = new DerSequenceGenerator(ms, 1, false);
                s.AddObject(new DerPrintableString(e.Message));
                s.AddObject(new DerPrintableString(e.FunctionCall));
                s.AddObject(new DerPrintableString(e.Parameters));
                s.AddObject(new DerPrintableString(e.Name));
                s.AddObject(new DerInteger(e.EmployeeId));

                var floatBytes = BitConverter.GetBytes(e.RaiseRate);
                    // unable to find API to write floats, hacking away :/
                s.AddObject(new DerOctetString(floatBytes));

                s.AddObject(new DerPrintableString(e.AddressLine1));
                s.AddObject(new DerPrintableString(e.AddressLine2));
                s.AddObject(new DerOctetString(e.Icon));
                s.AddObject(new DerOctetString(e.LargeIcon));
                s.Close();

                return ms.ToArray();
            }            
        }
    }
}
