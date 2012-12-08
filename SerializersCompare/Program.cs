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
        public static List<Results> results;
        public static Test test = new Test();


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
                        printTestObject(serName);
                        break;
                    case 'R':
                        test.PrintResultTable(results);
                        break;
                    default:
                        Console.WriteLine("Unknown input!");
                        break;
                }

                if (stillWorking)
                {
                    printMenu();
                    menuOption = getUserSelection();
                }
            }
        }

        static void printMenu()
        {
            Console.WriteLine("Options: (T)est, (R)esults, (D)eserializer output, (E)xit");
        }

        static char getUserSelection()
        {
            char menuOption = 'T';

            //ConsoleKeyInfo cki = Console.ReadKey();
            //string inputStr = cki.KeyChar.ToString().ToUpper().Replace(" ", string.Empty);
            string inputStr = Console.ReadLine();
            inputStr = inputStr.ToUpper().Replace(" ", string.Empty);
            if (inputStr.Length > 0)
                menuOption = inputStr[0];
            else
                menuOption = '~'; // char not in selection 

            return menuOption;
        }

        static void printTestObject(string serName)
        {
            Results resultsThisSer = results.Find(a => a.serName == serName);
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
            SimpleEntity originalObject = new SimpleEntity();
            originalObject.FillDummyData();
            SimpleEntity testObject = new SimpleEntity();
            
            results = test.RunTests<SimpleEntity>(originalObject, testObject);
            
            test.PrintResultTable(results);
        }
    }
}
