using System;
using System.Collections.Generic;
using SerializersCompare.Entities;

namespace SerializersCompare
{
    class Program
    {
        public static List<Results> Results;
        public static Test Test = new Test();

        static void Main(string[] args)
        {
            bool stillWorking = true;
            char menuOption = 'T';

            while (stillWorking)
            {
                switch (menuOption)
                {
                    case 'T':
                        RunTests();
                        break;
                    case 'E':
                        stillWorking = false;
                        break;
                    case 'D':
                        Console.WriteLine("Type the name of the serializer to print:");
                        string serName = Console.ReadLine();
                        PrintTestObject(serName);
                        break;
                    case 'R':
                        Test.PrintResultTable(Results);
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
                    menuOption = GetUserSelection();
                }
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("Options: (T)est, (R)esults, (D)eserializer output, (E)xit");
        }

        static char GetUserSelection()
        {
            char menuOption = '~'; // some char not in selection 

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
            Results resultsThisSer = Results.Find(a => a.serName == serName);
            if (resultsThisSer != null)
            {
                Console.WriteLine(resultsThisSer.serializedFormObject);
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
            //var testObject = new SimpleEntity();
            var testObject = new InheritedEntity();
            
            Results = Test.RunTests(originalObject, testObject);
            
            Test.PrintResultTable(Results);
        }
    }
}
