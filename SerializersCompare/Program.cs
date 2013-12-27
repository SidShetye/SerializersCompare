using System;
using System.Collections.Generic;
using SerializersCompare.Entities;

namespace SerializersCompare
{
    class Program
    {
        public static List<Results> Results;
        public static dynamic Test;
        private const int NumOfObjects = 1000;

        private static char _menuOption = GetDefaultOption();

        private static char GetDefaultOption()
        {
            return 'T';
        }

        static void Main(string[] args)
        {
            bool stillWorking = true;

            while (stillWorking)
            {
                switch (_menuOption)
                {
                    case 'T':
                        RunTests();
                        PrintResults();
                        break;
                    case 'R':
                        PrintResults();
                        break;
                    case 'E':
                        stillWorking = false;
                        break;
                    case 'S':
                        PrintSerOrDeserObject(GetSerName(), serObj:true);
                        break;
                    case 'D':
                        PrintSerOrDeserObject(GetSerName(), serObj: false);
                        break;
                    case 'O':
                        ResultPrinter.BySize = !ResultPrinter.BySize;
                        PrintResults();
                        break;
                    case 'X':
                        //var expt = new Experiments.ThriftClientServerExpt();
                        var expt = new Experiments.Asn1Experiments();
                        expt.RunExpt1();
                        break;
                    default:
                        Console.WriteLine("Unknown input!");
                        break;
                }

                if (stillWorking)
                {
                    PrintMenu();
                    _menuOption = GetUserSelection();
                }
            }
        }

        private static void PrintResults()
        {
            ResultPrinter.Print(Results);
        }

        private static string GetSerName()
        {
            Console.WriteLine("Type the name of the serializer to print:");
            return Console.ReadLine();
            
        }

        static void PrintMenu()
        {
            Console.WriteLine("Options: (T)est, (R)esults, s(O)rt order, (S)erializer output, (D)eserializer output (in JSON form), (E)xit");
        }

        static char GetUserSelection()
        {
            char menuOption = GetDefaultOption(); 

            //ConsoleKeyInfo cki = Console.ReadKey();
            //string inputStr = cki.KeyChar.ToString().ToUpper().Replace(" ", string.Empty);
            string inputStr = Console.ReadLine();
            if (inputStr != null)
            {
                inputStr = inputStr.ToUpper().Replace(" ", string.Empty);
                if (inputStr.Length > 0)
                    menuOption = inputStr[0];                    
            }
            return menuOption;
        }

        static void PrintSerOrDeserObject(string serName, bool serObj)
        {
            Results resultsThisSer = Results.Find(a => a.SerName == serName);
            if (resultsThisSer != null)
            {
                if (serObj)
                    Console.WriteLine(resultsThisSer.SerializedFormObject);
                else
                    Console.WriteLine(resultsThisSer.RegeneratedObjectAsJson);

            }
            else
            {
                Console.WriteLine("Incorrect serializer name!");
            }
        }       

        static void RunTests()
        {
            // Pick an entity type
            //var originalObject = new SimpleEntity();
            var originalObject = new InheritedEntity();
            originalObject.FillDummyData();
            Test = new Test<InheritedEntity>();
            //Test = new Test<SimpleEntity>();

            Results = Test.RunTests(originalObject, NumOfObjects);
        }
    }
}
