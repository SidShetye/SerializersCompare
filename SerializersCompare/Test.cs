using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSerializers
{
    class Test
    {
        private object originalObject;
        private object testObject;

        public List<Results> RunTests<T>(T originalObj, T testObj)
        {
            originalObject = originalObj;
            testObject = testObj;

            int loopLimit = 10000;
            List<Results> resultTable = new List<Results>();
            // Json.NET test
            resultTable.Add(TestSerializerInLoop<T>(new JsonNET(), loopLimit));

            // ServiceStackTextJsv test
            resultTable.Add(TestSerializerInLoop<T>(new ServiceStackJsv(), loopLimit));

            // ServiceStackTextJson test
            resultTable.Add(TestSerializerInLoop<T>(new ServiceStackJson(), loopLimit));

            // .NET XML serializer
            resultTable.Add(TestSerializerInLoop<T>(new XmlDotNet(), loopLimit));

            // BinaryFormatter test
            resultTable.Add(TestSerializerInLoop<T>(new BinFormatter(), loopLimit));

            // ProtoBuf test
            resultTable.Add(TestSerializerInLoop<T>(new ProtoBuf(), loopLimit));

            return resultTable;
        }

        public void PrintResultTable(List<Results> resultTable)
        {
            // Stores as lists top-down but console printing is left to right!
            // This is inherently ugly, can be fixed if so desired
            int col1 = 5;
            int colN = 20;

            // Header - line 1
            string formatString = String.Format("{{0,{0}}}", col1);
            string toPrint = "";
            System.Console.Write(formatString, toPrint);
            for (int i = 0; i < resultTable.Count; i++)
            {
                toPrint = String.Format("{0}", resultTable[i].serName);
                formatString = String.Format("{{0,{0}}}", colN );
                System.Console.Write(formatString, toPrint);
            }
            System.Console.Write(Environment.NewLine);

            // Header - line 2
            formatString = String.Format("{{0,{0}}}", col1);
            toPrint = "Loop";
            System.Console.Write(formatString, toPrint);
            for (int i = 0; i < resultTable.Count; i++)
            {
                toPrint = String.Format("Size:{0} bytes", resultTable[i].sizeBytes);
                formatString = String.Format("{{0,{0}}}", colN );
                System.Console.Write(formatString, toPrint);
            }
            System.Console.Write(Environment.NewLine);

            // Rest of Table
            //Assumes: all colums have same length and rows line-up for same loop count
            for (int row = 0; row < resultTable[0].resultColumn.Count; row++)
            {
                formatString = String.Format("{{0,{0}}}", col1);
                toPrint = resultTable[0].resultColumn[row].iteration.ToString();
                System.Console.Write(formatString, toPrint);

                for (int i = 0; i < resultTable.Count; i++)
                {
                    formatString = String.Format("{{0,{0}}}", colN );
                    toPrint = String.Format("{0,4:n4} ms", resultTable[i].resultColumn[row].time.TotalMilliseconds);
                    System.Console.Write(formatString, toPrint);
                }
                System.Console.Write(Environment.NewLine);
            }

            System.Console.WriteLine("If the above looks messy, please set your console width to over 200 and rerun this program");

        }

        private Results TestSerializerInLoop<T>(dynamic ser, int loopLimit)
        {
            int sizeInBytes;
            bool success;
            string testObjJson;
            int i = 1;
            Results result = new Results();
            List<object> warmUpObjects = new List<object>();
            
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
                byte[] binOutput = ser.Serialize<T>(originalObject);
                strOutput = BitConverter.ToString(binOutput).Replace("-", " ");
            }
            else
            {
                strOutput = ser.Serialize<T>(originalObject);
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
                    binOutput = ser.Serialize<T>(originalObject);
                    testObject = ser.Deserialize<T>(binOutput);
                }
                sw.Stop();
                // Find size outside loop to avoid timing hits
                binOutput = ser.Serialize<T>(originalObject);
                sizeInBytes = binOutput.Count();
            }
            // TEXT serializers
            // eg. JSON, XML etc
            else 
            {
                string strOutput;
                sw.Reset();
                sw.Start();
                for (int i = 0; i < iterations; i++)
                {
                    strOutput = ser.Serialize<T>(originalObject);
                    testObject = ser.Deserialize<T>(strOutput);
                }
                sw.Stop();

                // Find size outside loop to avoid timing hits
                // Size as bytes for UTF-8 as it's most common on internet
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                byte[] strInBytes = encoding.GetBytes(ser.Serialize<T>(originalObject));
                sizeInBytes = strInBytes.Count();
            }
            ResultColumnEntry entry = new ResultColumnEntry();
            entry.iteration = iterations;
            long avgTicks = sw.Elapsed.Ticks / iterations;
            if (avgTicks == 0)
            {
                // sometime when running windows inside a VM this is 0! Possible vm issue?
                //Debugger.Break();
            }
            entry.time = new TimeSpan(avgTicks);


            // Debug: To aid printing to screen, human debugging etc. Json used as best for console presentation
            JsonNET jsonSer = new JsonNET();

            string orignalObjectAsJson = JsonHelper.FormatJson(jsonSer.Serialize<T>(originalObject));
            
            testObjJson = JsonHelper.FormatJson(jsonSer.Serialize<T>(testObject));
            success = true;
            if (orignalObjectAsJson != testObjJson)
            {
                System.Console.WriteLine(">>>> {0} FAILED <<<<", ser.GetName());
                System.Console.WriteLine("\tOriginal and regenerated objects differ !!");
                System.Console.WriteLine("\tRegenerated objects is:");
                System.Console.WriteLine(testObjJson);
                success = false;
            }

            return entry;
        }
    }
}
