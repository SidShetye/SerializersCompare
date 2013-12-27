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
            resultTable.Add(TestSerializerInLoop<T>(new Avro<T>(), numOfObjects));

            // Thrift test
            resultTable.Add(TestSerializerInLoop<T>(new Thrift<T>(), numOfObjects));

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

        /// <summary>
        /// This is useful when printing serializers, times across multiple
        /// iterations. Yields a rather wide table that looks ugly in a console
        /// </summary>
        /// <param name="resultTable"></param>
        public void PrintResultTableHorizontal(List<Results> resultTable)
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
                toPrint = String.Format("{0}", resultTable[i].SerName);
                formatString = String.Format("{{0,{0}}}", colN);
                Console.Write(formatString, toPrint);
            }
            Console.Write(Environment.NewLine);

            // Header - line 2
            formatString = String.Format("{{0,{0}}}", col1);
            toPrint = "Loop";
            Console.Write(formatString, toPrint);
            for (int i = 0; i < resultTable.Count; i++)
            {
                toPrint = String.Format("Size:{0} bytes", resultTable[i].SizeBytes);
                formatString = String.Format("{{0,{0}}}", colN);
                Console.Write(formatString, toPrint);
            }
            Console.Write(Environment.NewLine);

            // Rest of Table
            //Assumes: all colums have same length and rows line-up for same loop count
            for (int row = 0; row < resultTable[0].ResultColumn.Count; row++)
            {
                formatString = String.Format("{{0,{0}}}", col1);
                toPrint = resultTable[0].ResultColumn[row].Iteration.ToString();
                Console.Write(formatString, toPrint);

                for (int i = 0; i < resultTable.Count; i++)
                {
                    formatString = String.Format("{{0,{0}}}", colN);
                    toPrint = String.Format("{0,4:n4} ms", resultTable[i].ResultColumn[row].Time.TotalMilliseconds);
                    Console.Write(formatString, toPrint);
                }
                Console.Write(Environment.NewLine);
            }

            Console.WriteLine("If the above looks messy, please set your console width to over 200 and rerun this program");

        }
        
        /// <summary>
        /// This is to print a single list of serializers vs time. Basically vertical
        /// representation of a single slice of the horizontal table
        /// </summary>
        /// <param name="resultTable"></param>
        public void PrintResultTableVertical(List<Results> resultTable)
        {
            // SORT
            resultTable.Sort(Results.BySize);

            // PRINT TEST INFO
            int numOfObjects = 0;
            var firstOrDefault = resultTable.FirstOrDefault();
            if (firstOrDefault != null)
            {
                var resultColumnEntry = firstOrDefault.ResultColumn.FirstOrDefault();
                if (resultColumnEntry != null)
                {
                    numOfObjects = resultColumnEntry.Iteration;
                }
            }
            Console.WriteLine("{0} cycles of object serialization->deserialization", numOfObjects);
            Console.WriteLine("Printing results, sorted by smallest payload size first ... \n");

            // PRINT HEADER
            const string fmtString = "{0,-16} {1,6} {2,10}";
            var hdr = String.Format(fmtString, "Name", "Bytes", "Time (ms)");
            Console.WriteLine(hdr);
            for (int i = 0; i < hdr.Length; i++)            
                Console.Write("-");            
            Console.WriteLine();

            // PRINT ACTUAL RESULTS
            foreach (var result in resultTable)
            {
                var resultColumnEntry = result.ResultColumn.FirstOrDefault();
                var timeString = (resultColumnEntry != null) ? 
                    String.Format("{0,4:n4}", resultColumnEntry.Time.TotalMilliseconds) : 
                    "Error";

                Console.WriteLine(fmtString, result.SerName, result.SizeBytes, timeString);
            }
            Console.WriteLine();
        }

        private Results TestSerializerInLoop<T>(dynamic ser, int numOfObjects)
        {
            int sizeInBytes;
            bool success;
            string testObjJson;
            var result = new Results();
            
            // Init
            var initArgs = new List<T>();
            initArgs.Add((T) _originalObject);
            ser.Init(initArgs);

            // Warmup
            var warmup = TestSerializer<T>(ser, 1, out sizeInBytes, out success, out testObjJson);

            // Actual test loop
            result.SerName = ser.GetName();
            result.ResultColumn = new List<ResultColumnEntry>();

            // Serialize => Deserialize, "numOfObjects" times to average per object times
            ResultColumnEntry resultEntry = TestSerializer<T>(ser, numOfObjects, out sizeInBytes, out success, out testObjJson);
            result.ResultColumn.Add(resultEntry);

            result.SizeBytes = sizeInBytes;
            result.Success = success;
            result.TestObjectAsJson = testObjJson; // for debug
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

        private ResultColumnEntry TestSerializer<T>(dynamic ser, int numOfObjects, out int sizeInBytes, out bool success, out string testObjJson)
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
