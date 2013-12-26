using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SerializersCompare.Serializers;
using SerializersCompare.Utils;

namespace SerializersCompare
{
    class Test
    {
        private object _originalObject;
        private object _testObject;

        public List<Results> RunTests<T>(T originalObj, T testObj) where T : new()
        {
            _originalObject = originalObj;
            _testObject = testObj;

            const int loopLimit = 10000;
            var resultTable = new List<Results>();

            // BINARY SERIALIZERS
            // ProtoBuf test
            resultTable.Add(TestSerializerInLoop<T>(new ProtoBuf<T>(), loopLimit));

            // Avro test
            resultTable.Add(TestSerializerInLoop<T>(new Avro<T>(), loopLimit));

            // Avro test
            resultTable.Add(TestSerializerInLoop<T>(new Thrift<T>(), loopLimit));

            //MessagePack
            resultTable.Add(TestSerializerInLoop<T>(new MessagePack<T>(), loopLimit));

            // Json.NET.BSON test
            resultTable.Add(TestSerializerInLoop<T>(new JsonNetBson<T>(), loopLimit));

            // BinaryFormatter test
            resultTable.Add(TestSerializerInLoop<T>(new BinFormatter<T>(), loopLimit));
            
            // TEXT SERIALIZERS
            // Json.NET test
            resultTable.Add(TestSerializerInLoop<T>(new JsonNet<T>(), loopLimit));

            // ServiceStackTextJson test
            resultTable.Add(TestSerializerInLoop<T>(new ServiceStackJson<T>(), loopLimit));

            // ServiceStackTextJsv test
            resultTable.Add(TestSerializerInLoop<T>(new ServiceStackJsv<T>(), loopLimit));

            // .NET XML serializer
            resultTable.Add(TestSerializerInLoop<T>(new XmlDotNet<T>(), loopLimit));

            return resultTable;
        }

        public void PrintResultTable(List<Results> resultTable)
        {
            // Stores as lists top-down but console printing is left to right!
            // This is inherently ugly, can be fixed if so desired
            const int col1 = 5;
            const int colN = 20;

            // Header - line 1
            string formatString = String.Format("{{0,{0}}}", col1);
            string toPrint = "";
            Console.Write(formatString, toPrint);
            for (int i = 0; i < resultTable.Count; i++)
            {
                toPrint = String.Format("{0}", resultTable[i].serName);
                formatString = String.Format("{{0,{0}}}", colN );
                Console.Write(formatString, toPrint);
            }
            Console.Write(Environment.NewLine);

            // Header - line 2
            formatString = String.Format("{{0,{0}}}", col1);
            toPrint = "Loop";
            Console.Write(formatString, toPrint);
            for (int i = 0; i < resultTable.Count; i++)
            {
                toPrint = String.Format("Size:{0} bytes", resultTable[i].sizeBytes);
                formatString = String.Format("{{0,{0}}}", colN );
                Console.Write(formatString, toPrint);
            }
            Console.Write(Environment.NewLine);

            // Rest of Table
            //Assumes: all colums have same length and rows line-up for same loop count
            for (int row = 0; row < resultTable[0].resultColumn.Count; row++)
            {
                formatString = String.Format("{{0,{0}}}", col1);
                toPrint = resultTable[0].resultColumn[row].iteration.ToString();
                Console.Write(formatString, toPrint);

                for (int i = 0; i < resultTable.Count; i++)
                {
                    formatString = String.Format("{{0,{0}}}", colN );
                    toPrint = String.Format("{0,4:n4} ms", resultTable[i].resultColumn[row].time.TotalMilliseconds);
                    Console.Write(formatString, toPrint);
                }
                Console.Write(Environment.NewLine);
            }

            Console.WriteLine("If the above looks messy, please set your console width to over 200 and rerun this program");

        }

        private Results TestSerializerInLoop<T>(dynamic ser, int loopLimit)
        {
            int sizeInBytes;
            bool success;
            string testObjJson;
            int i = 1;
            var result = new Results();
            var warmUpObjects = new List<object>();
            
            result.serName = ser.GetName();
            result.resultColumn = new List<ResultColumnEntry>();
            do
            {
                // test at this loop count
                ResultColumnEntry resultEntry = TestSerializer<T>(ser, i, out sizeInBytes, out success, out testObjJson);
                result.resultColumn.Add(resultEntry);
                i = i * 2;    // geometrically scale loop at x2
            } while (i <= loopLimit);

            result.sizeBytes = sizeInBytes;
            result.success = success;
            result.testObjectAsJson = testObjJson; // for debug
            result.serializedFormObject = PrintSerializedOutput<T>(ser); // for debug

            return result;
        }

        private string PrintSerializedOutput<T>(dynamic ser)
        {
            string strOutput;
            if (ser.IsBinary())
            {
                byte[] binOutput = ser.Serialize(_originalObject);
                strOutput = BitConverter.ToString(binOutput).Replace("-", " ");
            }
            else
            {
                strOutput = ser.Serialize(_originalObject);
            }
            return strOutput;
        }

        private ResultColumnEntry TestSerializer<T>(dynamic ser, int iterations, out int sizeInBytes, out bool success, out string testObjJson)
        {
            Stopwatch sw = new Stopwatch();
            
            // BINARY serializers 
            // eg: ProtoBufs, Bin Formatter etc
            if (ser.IsBinary()) 
            {
                byte[] binOutput;
                sw.Reset();
                sw.Start();
                for (int i = 0; i < iterations; i++)
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
                for (int i = 0; i < iterations; i++)
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
            entry.iteration = iterations;
            long avgTicks = sw.Elapsed.Ticks / iterations;
            if (avgTicks == 0)
            {
                // sometime when running windows inside a VM this is 0! Possible vm issue?
                //Debugger.Break();
            }
            entry.time = new TimeSpan(avgTicks);


            // Debug: To aid printing to screen, human debugging etc. Json used as best for console presentation
            var jsonSer = new JsonNet<T>();

            string orignalObjectAsJson = JsonHelper.FormatJson(jsonSer.Serialize(_originalObject));
            
            testObjJson = JsonHelper.FormatJson(jsonSer.Serialize(_testObject));
            success = true;
            if (orignalObjectAsJson != testObjJson)
            {
                Console.WriteLine(">>>> {0} FAILED <<<<", ser.GetName());
                Console.WriteLine("\tOriginal and regenerated objects differ !!");
                Console.WriteLine("\tRegenerated objects is:");
                Console.WriteLine(testObjJson);
                success = false;
            }

            return entry;
        }
    }
}
