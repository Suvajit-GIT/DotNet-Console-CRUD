using IECS_CRUD.BAL;
using IECS_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.Helpers
{
    public class CommandHandler
    {
        private readonly PersonBAL _personBAL;

        public CommandHandler(PersonBAL personBAL)
        {
            _personBAL = personBAL;
        }

        #region Person
        public static void AddPerson(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("Invalid person add command. Usage: person add -firstname [FirstName] -lastname [LastName] -dob [DateOfBirth] -nickname [Nickname]");
                return;
            }
            // Extract the command-line arguments
            string firstName = null;
            string lastName = null;
            string dob = null;
            string nickname = null;

            try
            {


                for (int i = 2; i < args.Length; i += 2)
                {
                    switch (args[i])
                    {
                        case "-firstname":
                            firstName = args[i + 1];
                            break;
                        case "-lastname":
                            lastName = args[i + 1];
                            break;
                        case "-dob":
                            dob = args[i + 1];
                            break;
                        case "-nickname":
                            nickname = args[i + 1];
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {args[i]}");
                            return;
                    }
                }


                // Instantiate PersonBAL and pass DataLayer instance
                var personBAL = new PersonBAL(AppConfig.FilePath);

                // Create a new Person object
                var person = CreatePerson(firstName, lastName, dob, nickname);

                // Call the AddPerson method of PersonBAL to add the person
                personBAL.AddPerson(person);
                Console.WriteLine("Person added successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                //LoggerHelper.LogError(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add person: " + ex.Message);
                LoggerHelper.LogError(ex);
            }
        }

        public static void ViewPeople(string[] args)
        {
            string id = null;

            try
            {
                // Instantiate PersonBAL and pass DataLayer instance
                var personBAL = new PersonBAL(AppConfig.FilePath);

                if(args.Length > 2)
                {
                    for (int i = 2; i < args.Length; i += 2)
                    {
                        switch (args[i])
                        {
                            case "-id":
                                id = args[i + 1];
                                break;
                            default:
                                Console.WriteLine($"Unknown argument: {args[i]}");
                                return;
                        }
                    }

                    var peoplewithaddress = personBAL.Viewpeoplewithaddress(id);

                    if (peoplewithaddress == null || peoplewithaddress.Count == 0)
                    {
                        Console.WriteLine("No records found.");
                    }
                    else
                    {
                        Console.WriteLine("+-----+-------------+-------------+--------------+----------+-------------+-------------+---------+--------+");
                        Console.WriteLine("| ID  | First Name  |  Last Name  |Date of Birth | Nickname |     Line1   |   Line2     | Country |PostCode|");
                        Console.WriteLine("+-----+-------------+-------------+--------------+----------+-------------+-------------+---------+--------+");

                        foreach (var personAddress in peoplewithaddress)
                        {
                            string[] lines = personAddress.Line1.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            string line1 = lines.Length > 0 ? lines[0] : "";
                            string line2 = lines.Length > 1 ? lines[1] : "";

                            // Ensure proper formatting with appropriate spacing
                            Console.WriteLine($"|{personAddress.ID,-5}|{personAddress.FirstName,-13}|{personAddress.LastName,-13}|{personAddress.DateOfBirth.ToShortDateString(),-14}|{personAddress.Nickname,-10}|{line1,-13}|{line2,-13}|{personAddress.Country,-9}|{personAddress.Postcode,-8}|");
                            Console.WriteLine("+-----+-------------+-------------+--------------+----------+-------------+-------------+---------+--------+");
                        }


                    }
                }
                else
                {
                    var people = personBAL.ViewPerson();

                    if (people == null || people.Count == 0)
                    {
                        Console.WriteLine("No records found.");
                    }
                    else
                    {
                        Console.WriteLine("+------------+---------------+---------------+--------------------+-------------+");
                        Console.WriteLine("|     ID     |  First Name   |   Last Name   |   Date of Birth    |  Nickname   |");
                        Console.WriteLine("+------------+---------------+---------------+--------------------+-------------+");

                        foreach (var person in people)
                        {
                            Console.WriteLine($"|{person.ID,-12}|{person.FirstName,-15}|{person.LastName,-15}|{person.DateOfBirth.ToShortDateString(),-20}|{person.Nickname,-13}|");
                            Console.WriteLine("+------------+---------------+---------------+--------------------+-------------+");
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                LoggerHelper.LogError(ex);
            }
        }
        public static void EditPerson(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("Invalid person edit command. Usage: person edit -id [ID] -firstname [FirstName] -lastname [LastName] -dob [DateOfBirth] -nickname [Nickname]");
                return;
            }

             
            string id = null;
            string firstName = null;
            string lastName = null;
            string dateOfBirth = null;
            string nickname = null;


            try
            {
                for (int i = 2; i < args.Length; i += 2)
                {
                    switch (args[i])
                    {
                        case "-id":
                            id = args[i + 1];
                            break;
                        case "-firstname":
                            firstName = args[i + 1];
                            break;
                        case "-lastname":
                            lastName = args[i + 1];
                            break;
                        case "-dob":
                            dateOfBirth = args[i + 1];
                            break;
                        case "-nickname":
                            nickname = args[i + 1];
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {args[i]}");
                            return;
                    }
                }


                 
                var personBAL = new PersonBAL(AppConfig.FilePath);

                //Get the person with given id
                Person existingPerson = personBAL.GetPersonById(id);

                if (existingPerson == null)
                {
                    Console.WriteLine($"Person with ID {id} not found.");
                    return;
                }

                
                if (!string.IsNullOrEmpty(firstName))
                {
                    existingPerson.FirstName = firstName;
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    existingPerson.LastName = lastName;
                }
                if (!string.IsNullOrEmpty(dateOfBirth))
                {
                    if (DateTime.TryParse(dateOfBirth, out DateTime parsedDateOfBirth))
                    {
                        existingPerson.DateOfBirth = parsedDateOfBirth;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date of birth format.");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(nickname))
                {
                    existingPerson.Nickname = nickname;
                }

                // Call the UpdatePerson method of PersonBAL to update the person
                personBAL.UpdatePerson(existingPerson);

                Console.WriteLine("Person updated successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                //LoggerHelper.LogError(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to edit person: {ex.Message}");
                LoggerHelper.LogError(ex);
            }
        }

        public static void DeletePerson(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid person delete command. Usage: person delete -id [ID]");
                return;
            }

            string id = null;

            try
            {
                for (int i = 2; i < args.Length; i += 2)
                {
                    switch (args[i])
                    {
                        case "-id":
                            id = args[i + 1];
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {args[i]}");
                            return;
                    }
                }

                 
                var personBAL = new PersonBAL(AppConfig.FilePath);

                // Call the DeletePerson method of PersonBAL to delete the person
                personBAL.DeletePerson(id);

                Console.WriteLine("Person deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete person: {ex.Message}");
                LoggerHelper.LogError(ex);
            }
        }

        public static void SearchPeople(string[] args)
        {
            string searchTerm = "";

            if (args.Length < 3)
            {
                Console.WriteLine("Invalid search command. Usage: search [searchTerm]");
                return;
            }
            else
            {
                searchTerm = string.Join(" ", args.Skip(2)).Trim('"');
                 
            }
            try
            {
                var personBAL = new PersonBAL(AppConfig.FilePath);

                var searchResults = personBAL.SearchPeople(searchTerm);

                if (searchResults.Count == 0)
                {
                    Console.WriteLine("No matching results found.");
                }
                else
                {
                    Console.WriteLine("+------------+---------------+---------------+--------------------+-------------+");
                    Console.WriteLine("|     ID     |  First Name   |   Last Name   |   Date of Birth    |  Nickname   |");
                    Console.WriteLine("+------------+---------------+---------------+--------------------+-------------+");

                    foreach (var person in searchResults)
                    {
                        Console.WriteLine($"|{person.ID,-12}|{person.FirstName,-15}|{person.LastName,-15}|{person.DateOfBirth.ToShortDateString(),-20}|{person.Nickname,-13}|");
                        Console.WriteLine("+------------+---------------+---------------+--------------------+-------------+");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to search people: {ex.Message}");
                LoggerHelper.LogError(ex);
            }
        }
        private static Person CreatePerson(string firstName, string lastName, string dob, string nickname)
        {
            try
            {
                // Parse the date of birth string to a DateTime object
                DateTime dateOfBirth = DateTime.Parse(dob);

                // Create a new Person object with the parsed data
                return new Person(null, firstName, lastName, dateOfBirth, nickname);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Invalid date format for Date of Birth.", ex);
                
            }
        }

        #endregion

        #region Address

        public static void AddAddress(string[] args)
        {
            if (args.Length < 8)
            {
                Console.WriteLine("Invalid address add command. Usage: address add -personId [PersonId] -line1 [Line1] -country [Country]");
                return;
            }
            
            string personId = null;
            string line1 = null;
            string line2 = null;
            string country = null;
            string postcode = null;
            try
            {


                for (int i = 2; i < args.Length; i += 2)
                {
                    switch (args[i].ToLower())
                    {
                        case "-personid":
                            personId = args[i + 1];
                            break;
                        case "-line1":
                            line1 = args[i + 1];
                            break;
                        case "-line2":
                            line2 = args[i + 1];
                            break;
                        case "country":
                            country = args[i + 1];
                            break;
                        case "-postcode":
                            postcode = args[i + 1];
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {args[i]}");
                            return;
                    }
                }
 
                // Instantiate AddressBAL
                var addressBAL = new AddressBAL(AppConfig.FilePath);
                var address = new Address(null, personId, line1, line2, country, postcode);

                // Get the Address with the same Person ID
                Address existingAddressWithPerson = addressBAL.GetAddressByPersonId(address);


                if (existingAddressWithPerson != null)
                {
                    //Already exist for same person
                    Console.WriteLine($"Address With Person ID {personId} Already added");
                    return;
                }
                else
                {

                    //Add Address with Person Id
                    addressBAL.AddAddressToPerson(address);
                    Console.WriteLine("Address with PersonId added successfully.");
                }

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                //LoggerHelper.LogError(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add Address: " + ex.Message);
                LoggerHelper.LogError(ex);
            }
        }

        public static void EditAddress(string[] args)
        {
            if (args.Length < 6)
            {
                Console.WriteLine("Invalid Address edit command. Usage: Address edit -id [ID] -line1 [Line1] -line2 [Line2] -country [Country] -postcode [Postcode]");
                return;
            }


            string id = null;
            string line1 = null;
            string line2 = null;
            string country = null;
            string postcode = null;


            try
            {
                for (int i = 2; i < args.Length; i += 2)
                {
                    switch (args[i])
                    {
                        case "-id":
                            id = args[i + 1];
                            break;
                        case "-line1":
                            line1 = args[i + 1];
                            break;
                        case "-line2":
                            line2 = args[i + 1];
                            break;
                        case "-country":
                            country = args[i + 1];
                            break;
                        case "-postcode":
                            postcode = args[i + 1];
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {args[i]}");
                            return;
                    }
                }



                var AddressBAL = new AddressBAL(AppConfig.FilePath);

                // Get the Address with the same Person ID
                Address existingAddress = AddressBAL.GetAddressById(id);

                if (existingAddress == null)
                {
                    Console.WriteLine($"Address with ID {id} not found.");
                    return;
                }


                if (!string.IsNullOrEmpty(line1))
                {
                    existingAddress.Line1 = line1;
                }
                if (!string.IsNullOrEmpty(line2))
                {
                    existingAddress.Line2 = line2;
                }
                if (!string.IsNullOrEmpty(country))
                {
                    existingAddress.Country = country;
                }
                if (!string.IsNullOrEmpty(postcode))
                {
                    existingAddress.Postcode = postcode;
                }

                // Call the UpdateAddress method of AddressBAL to update the Address
                AddressBAL.UpdateAddress(existingAddress);

                Console.WriteLine("Address updated successfully.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
               // LoggerHelper.LogError(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to edit Address: {ex.Message}");
                LoggerHelper.LogError(ex);
            }
        }

        public static void DeleteAddress(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid Address delete command. Usage: Address delete -id [ID]");
                return;
            }
            string id = null;

            try
            {
                for (int i = 2; i < args.Length; i += 2)
                {
                    switch (args[i])
                    {
                        case "-id":
                            id = args[i + 1];
                            break;
                        default:
                            Console.WriteLine($"Unknown argument: {args[i]}");
                            return;
                    }
                }


                var addressBAL = new AddressBAL(AppConfig.FilePath);

                // Call the DeleteAddress method of addressBAL to delete the Address
                addressBAL.DeleteAddress(id);

                Console.WriteLine("Address deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete Address: {ex.Message}");
                LoggerHelper.LogError(ex);
            }
        }

        #endregion
    }
}
