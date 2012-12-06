using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace TestSerializers
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test();
            test.LoadEntityObject();
            test.RunTests();

            System.Console.Write(Environment.NewLine);
            System.Console.WriteLine("Press any key to exit ...");
            System.Console.ReadLine();
        }
    }
}
