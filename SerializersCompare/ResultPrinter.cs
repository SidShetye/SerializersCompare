using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializersCompare
{
    public static class ResultPrinter
    {
        public static bool BySize { get; set; }
        public static void Print(List<Results> resultTable)
        {
            PrintResultTableVertical(resultTable);
        }

        static ResultPrinter()
        {
            BySize = true;
        }

        /// <summary>
        /// This is useful when printing serializers, times across multiple
        /// iterations. Yields a rather wide table that looks ugly in a console
        /// </summary>
        /// <param name="resultTable"></param>
        public static void PrintResultTableHorizontal(List<Results> resultTable)
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
        public static void PrintResultTableVertical(List<Results> resultTable)
        {
            // SORT
            if (BySize)
                resultTable.Sort(Results.BySize);
            else
                resultTable.Sort(Results.ByTime);

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
            Console.WriteLine("{0} iterations per serializer, average times listed", numOfObjects);
            PrintCurrentSortOrder();

            // PRINT HEADER
            const string fmtString = "{0,-18} {1,6} {2,10}";
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

        public static void PrintCurrentSortOrder()
        {
            if (BySize)
                Console.WriteLine("Sorting result by size");
            else
                Console.WriteLine("Sorting result by time");
        }
    }
}
