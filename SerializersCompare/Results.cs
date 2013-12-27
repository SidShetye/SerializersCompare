using System;
using System.Collections.Generic;
using System.Linq;

namespace SerializersCompare
{
    public class Results
    {
        public string SerName { get; set; }
        public int SizeBytes { get; set; }
        public bool Success { get; set; }
        public List<ResultColumnEntry> ResultColumn { get; set; }

        // debug
        public string SerializedFormObject { get; set; }
        public string OrignalObjectAsJson { get; set; } // stored as JSON for human visualization
        //This is the original object => serialized => deserialized. Should again match original object if all went well!
        public string TestObjectAsJson { get; set; } // stored as JSON for human visualization

        public static int ByName(Results lhsResult, Results rhsResult)
        {
            return String.Compare(lhsResult.SerName, rhsResult.SerName, StringComparison.Ordinal);
        }
        public static int ByTime(Results lhsResult, Results rhsResult)
        {
            var resultColumnEntry = lhsResult.ResultColumn.FirstOrDefault();
            if (resultColumnEntry != null)
            {
                var firstOrDefault = rhsResult.ResultColumn.FirstOrDefault();
                if (firstOrDefault != null)
                    return resultColumnEntry.Time.CompareTo(firstOrDefault.Time);
                // rhsResult is missing it's result => infinity or very large 
                // => LHS smaller => -1
                return -1;
            }

            // lhsResult is missing it's result => missing => infinity or very large
            // => LHS bigger => 1
            return 1;
        }

        public static int BySize(Results lhsResult, Results rhsResult)
        {
            return lhsResult.SizeBytes.CompareTo(rhsResult.SizeBytes);
        }
    }

    public class ResultColumnEntry
    {
        public TimeSpan Time { get; set; }
        public int Iteration { get; set; }
    }
}
