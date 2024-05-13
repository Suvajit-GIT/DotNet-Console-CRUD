 
using IECS_CRUD.Helpers; 
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace IECS_CRUD
{
    class Program
    {

       
        static void Main(string[] args)
        {
            
            // Parse the command and call the appropriate method
            while (true)
            {
                Console.WriteLine("Enter a command:");
                string input = Console.ReadLine().ToLower();

                string[] commandArgs = SplitArguments(input);

                if (commandArgs.Length > 0)
                {
                    string command = commandArgs[0];
                   

                    switch (command)
                    {
                        case "person":
                            ProcessPersonCommand(commandArgs);
                            break;
                        case "address":
                            ProcessAddressCommand(commandArgs);
                            break;
                        default:
                            Console.WriteLine("Invalid command.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("No command provided.");
                }

                Console.WriteLine("Press Enter to continue or type 'exit' to quit or type 'clear' to clear the console.");
                string continueInput = Console.ReadLine().ToLower();
                if (continueInput == "exit")
                {
                    break;
                }
                else if (continueInput == "clear")
                {
                    Console.Clear();
                }

            }
        }

        #region Person
        static void ProcessPersonCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid person command.");
                return;
            }

            string subCommand = args[1].ToLower();

            switch (subCommand)
            {
                //maintan person create-read-update-delete case here
                case "view":
                    CommandHandler.ViewPeople(args);
                    break;
                case "add":
                    CommandHandler.AddPerson(args);
                    break;
                case "edit":
                    CommandHandler.EditPerson(args);
                    break;
                case "delete":
                    CommandHandler.DeletePerson(args);
                    break;
                case "search":
                    CommandHandler.SearchPeople(args);
                    break;
                default:
                    Console.WriteLine($"Unknown command: {subCommand}");
                    break;
            }
        }
        #endregion

        #region Address
        static void ProcessAddressCommand(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid Address command.");
                return;
            }

            string subCommand = args[1].ToLower();

            switch (subCommand)
            {
                //maintan address create with person-update-delete case here
               
                case "add":
                    CommandHandler.AddAddress(args);
                    break;
                case "edit":
                    CommandHandler.EditAddress(args);
                    break;
                case "delete":
                    CommandHandler.DeleteAddress(args);
                    break;
                default:
                    Console.WriteLine($"Unknown command: {subCommand}");
                    break;
            }
        }
        #endregion

        static string[] SplitArguments(string input)
        {
            // Split by space outside of double quotes
            MatchCollection matches = Regex.Matches(input, @"[\""].+?[\""]|[^ ]+");
            string[] args = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                args[i] = matches[i].Value.Replace("\"", "");
            }
            return args;
        }

    }
}
