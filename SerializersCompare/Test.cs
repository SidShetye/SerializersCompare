using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SerializersCompare.Serializers;
using SerializersCompare.Utils;

namespace SerializersCompare
{
    class Test<T> where T : new()
    {
        private object _originalObject;
        private T _testObject;

        public List<Results> RunTests(T originalObj, int numOfObjects)
        {
            _originalObject = originalObj;            

            var resultTable = new List<Results>();

            // BINARY SERIALIZERS
            // ProtoBuf test
            resultTable.Add(TestSerializerInLoop<T>(new ProtoBuf<T>(), numOfObjects));

            // Avro test
            resultTable.Add(TestSerializerInLoop<T>(new AvroMsft<T>(), numOfObjects));

            // Avro test
            resultTable.Add(TestSerializerInLoop<T>(new Avro<T>(), numOfObjects));

            // Avro test
            resultTable.Add(TestSerializerInLoop<T>(new Avro<T>(true), numOfObjects));

            // Thrift test
            resultTable.Add(TestSerializerInLoop<T>(new Thrift<T>(), numOfObjects));

            // Thrift test
            resultTable.Add(TestSerializerInLoop<T>(new Thrift<T>(true), numOfObjects));

            //MessagePack
            resultTable.Add(TestSerializerInLoop<T>(new MessagePack<T>(), numOfObjects));

            // Json.NET.BSON test
            resultTable.Add(TestSerializerInLoop<T>(new JsonNetBson<T>(), numOfObjects));

            // BinaryFormatter test
            resultTable.Add(TestSerializerInLoop<T>(new BinFormatter<T>(), numOfObjects));
            
            // TEXT SERIALIZERS
            // Json.NET test
            resultTable.Add(TestSerializerInLoop<T>(new JsonNet<T>(), numOfObjects));

            // ServiceStackTextJson test
            resultTable.Add(TestSerializerInLoop<T>(new ServiceStackJson<T>(), numOfObjects));

            // ServiceStackTextJsv test
            resultTable.Add(TestSerializerInLoop<T>(new ServiceStackJsv<T>(), numOfObjects));

            // .NET XML serializer
            resultTable.Add(TestSerializerInLoop<T>(new XmlDotNet<T>(), numOfObjects));

            return resultTable;
        }

        private Results TestSerializerInLoop<T>(dynamic ser, int numOfObjects)
        {
            int sizeInBytes;
            bool success;
            string regeneratedObjAsJson;
            var result = new Results();
            
            // Warmup
            var warmup = TestSerializer<T>(ser, 1, out sizeInBytes, out success, out regeneratedObjAsJson);

            // Actual test loop
            result.SerName = ser.GetName();
            result.ResultColumn = new List<ResultColumnEntry>();

            // Serialize => Deserialize, "numOfObjects" times to average per object times
            ResultColumnEntry resultEntry = TestSerializer<T>(ser, numOfObjects, out sizeInBytes, out success, out regeneratedObjAsJson);
            result.ResultColumn.Add(resultEntry);

            result.SizeBytes = sizeInBytes;
            result.Success = success;
            result.RegeneratedObjectAsJson = regeneratedObjAsJson; // for debug
            result.SerializedFormObject = PrintSerializedOutput(ser); // for debug

            return result;
        }

        private string PrintSerializedOutput(dynamic ser)
        {
            string strOutput;
            if (ser.IsBinary())
            {
                byte[] binOutput = ser.Serialize(_originalObject);
                strOutput = BitConverter.ToString(binOutput);
            }
            else
            {
                strOutput = ser.Serialize(_originalObject);
            }
            return strOutput;
        }

        private ResultColumnEntry TestSerializer<T>(dynamic ser, int numOfObjects, out int sizeInBytes, out bool success, out string regeneratedObjAsJson)
        {
            var sw = new Stopwatch();
            
            // BINARY serializers 
            // eg: ProtoBufs, Bin Formatter etc
            if (ser.IsBinary()) 
            {
                byte[] binOutput;
                sw.Reset();
                sw.Start();
                for (int i = 0; i < numOfObjects; i++)
                {
                    binOutput = ser.Serialize(_originalObject);
                    _testObject = ser.Deserialize(binOutput);
                }
                sw.Stop();
                // Find size outside loop to avoid timing hits
                binOutput = ser.Serialize(_originalObject);
                sizeInBytes = binOutput.Count();
            }
            // TEXT serializers
            // eg. JSON, XML etc
            else 
            {
                sw.Reset();
                sw.Start();
                for (int i = 0; i < numOfObjects; i++)
                {
                    string strOutput = ser.Serialize(_originalObject);
                    _testObject = ser.Deserialize(strOutput);
                }
                sw.Stop();

                // Find size outside loop to avoid timing hits
                // Size as bytes for UTF-8 as it's most common on internet
                var encoding = new System.Text.UTF8Encoding();
                byte[] strInBytes = encoding.GetBytes(ser.Serialize(_originalObject));
                sizeInBytes = strInBytes.Count();
            }
            var entry = new ResultColumnEntry();
            entry.Iteration = numOfObjects;
            long avgTicks = sw.Elapsed.Ticks / numOfObjects;
            if (avgTicks == 0)
            {
                // sometime when running windows inside a VM this is 0! Possible vm issue?
                //Debugger.Break();
            }
            entry.Time = new TimeSpan(avgTicks);

            // Debug: To aid printing to screen, human debugging etc. Json used as best for console presentation
            var jsonSer = new JsonNet<T>();

            string orignalObjectAsJson = JsonHelper.FormatJson(jsonSer.Serialize(_originalObject));
            
            regeneratedObjAsJson = JsonHelper.FormatJson(jsonSer.Serialize(_testObject));
            success = true;
            if (orignalObjectAsJson != regeneratedObjAsJson)
            {
                Console.WriteLine(">>>> {0} FAILED <<<<", ser.GetName());
                Console.WriteLine("\tOriginal and regenerated objects differ !!");
                Console.WriteLine("\tRegenerated objects is:");
                Console.WriteLine(regeneratedObjAsJson);
                success = false;
            }

            return entry;
        }
    }
}
