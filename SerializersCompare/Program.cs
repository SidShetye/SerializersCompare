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

        private static char _menuOption = 'T';

        static void Main(string[] args)
        {
            bool stillWorking = true;

            while (stillWorking)
            {
                switch (_menuOption)
                {
                    case 'T':
                        RunTests();
                        break;
                    case 'E':
                        stillWorking = false;
                        break;
                    case 'S':
                        Console.WriteLine("Type the name of the serializer to print:");
                        string serName = Console.ReadLine();
                        PrintTestObject(serName);
                        break;
                    case 'R':
                        Test.PrintResultTableVertical(Results);
                        break;
                    case 'X':
                        //var expt = new Experiments.ThriftClientServerExpt();
                        var expt = new Experiments.ThriftSerialization();
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

        static void PrintMenu()
        {
            Console.WriteLine("Options: (T)est, (R)esults, (S)erializer output, (E)xit");
        }

        static char GetUserSelection()
        {
            char menuOption = _menuOption; 

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

        static void PrintTestObject(string serName)
        {
            Results resultsThisSer = Results.Find(a => a.SerName == serName);
            if (resultsThisSer != null)
            {
                Console.WriteLine(resultsThisSer.SerializedFormObject);
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

            Results = Test.RunTests(originalObject, NumOfObjects);
            Test.PrintResultTableVertical(Results);
        }
    }
}
