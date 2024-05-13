using IECS_CRUD.Helpers;
using IECS_CRUD.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IECS_CRUD.Repository
{
    public class PersonRepo
    {
        private readonly string filePath;

        public PersonRepo(string filePath)
        {
            this.filePath = filePath;
        }

        public void AddPerson(Person person)
        {
            try
            {
                // Deserialize existing data from the JSON file
                List<Person> people = GetAllPeople();



                if (!IsDuplicateName(person.FirstName, person.LastName))
                {
                    // Generate unique ID
                    string lastId = GetLastId(people);
                    string newId = GenerateNextId(lastId);

                    // Assign the generated ID to the person
                    person.ID = newId;

                    // Add person to the list of people
                    people.Add(person);

                    // Save updated data to the file
                    SavePeopleToFile(people);
                    // SaveAddressAndPersonData(data);
                }
                else
                {
                    throw new ArgumentException("A person with the same first and last name already exists.");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }


        public void UpdatePerson(Person updatedPerson)
        {
            try
            {
                
                string existingJson = File.Exists(filePath) ? File.ReadAllText(filePath) : "";
                var existingPeopleData = JsonConvert.DeserializeObject<AddressAndPerson>(existingJson) ?? new AddressAndPerson();

                
                Person personToUpdate = existingPeopleData.Person.FirstOrDefault(p => p.ID.Equals(updatedPerson.ID, StringComparison.OrdinalIgnoreCase));

                
                if (personToUpdate == null)
                {
                    string unquotedId = updatedPerson.ID.Trim('"');
                    personToUpdate = existingPeopleData.Person.FirstOrDefault(p => p.ID.Equals(unquotedId, StringComparison.OrdinalIgnoreCase));
                }

                // Check for duplicate names
                if (!IsDuplicateName(updatedPerson.FirstName, updatedPerson.LastName))
                {
                   
                    int index = existingPeopleData.Person.FindIndex(p => p.ID.Equals(updatedPerson.ID, StringComparison.OrdinalIgnoreCase));


                    if (index != -1)
                    {
                        
                        if (updatedPerson.FirstName != null)
                            existingPeopleData.Person[index].FirstName = updatedPerson.FirstName;
                        if (updatedPerson.LastName != null)
                            existingPeopleData.Person[index].LastName = updatedPerson.LastName;
                        if (updatedPerson.DateOfBirth != null)
                            existingPeopleData.Person[index].DateOfBirth = updatedPerson.DateOfBirth;
                        if (updatedPerson.Nickname != null)
                            existingPeopleData.Person[index].Nickname = updatedPerson.Nickname;


                        
                        string json = JsonConvert.SerializeObject(existingPeopleData, Formatting.Indented);

                        
                        File.WriteAllText(filePath, json);

                    }
                    else
                    {
                        throw new ArgumentException($"Person with ID {updatedPerson.ID} not found.");
                    }
                }
                else
                {
                    throw new ArgumentException("A person with the same first and last name already exists.");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public void DeletePerson(string id)
        {
            try
            {
                string existingJson = File.Exists(filePath) ? File.ReadAllText(filePath) : "";


                var existingPeopleData = JsonConvert.DeserializeObject<AddressAndPerson>(existingJson) ?? new AddressAndPerson();



                // Find the person with the specified ID within the People list
                Person personToDelete = existingPeopleData.Person.FirstOrDefault(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (personToDelete == null)
                {
                    string unquotedId = id.Trim('"');
                    personToDelete = existingPeopleData.Person.FirstOrDefault(p => p.ID.Equals(unquotedId, StringComparison.OrdinalIgnoreCase));
                }

                if (personToDelete != null)
                {
                    existingPeopleData.Person.Remove(personToDelete);

                    // Serialize the updated data to JSON format
                    string json = JsonConvert.SerializeObject(existingPeopleData, Formatting.Indented);

                    // Write the JSON data back to the file
                    File.WriteAllText(filePath, json);
                }
                else
                {
                    throw new ArgumentException($"Person with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public List<Person> SearchPeople(string searchTerm)
        {
            try
            {
                if (searchTerm == null)
                {
                    throw new ArgumentNullException(nameof(searchTerm), "Search term cannot be null.");
                }
                // Get all people from the JSON file
                List<Person> people = GetAllPeople();


                // Search for people whose first name or last name contains the search term (case-insensitive)
                List<Person> searchResults = people
                            .Where(p => p.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                        p.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                            .ToList();

                return searchResults;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public List<Person> GetAllPeople()
        {
            List<Person> people = new List<Person>();

            try
            {
                if (File.Exists(filePath))
                {
                    // Deserialize the JSON file into a PeopleData object
                    string json = File.ReadAllText(filePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var existingPeopleData = JsonConvert.DeserializeObject<AddressAndPerson>(json) ?? new AddressAndPerson();

                        if (existingPeopleData != null)
                        {
                            people = existingPeopleData.Person;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }

            return people;
        }

        public Person GetPersonById(string id)
        {
            try
            {

                List<Person> people = GetAllPeople();

                // Find the person with the specified ID within the Person list
                var peopleData = JsonConvert.DeserializeObject<AddressAndPerson>(File.ReadAllText(filePath));

                Person person = peopleData.Person.FirstOrDefault(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (person == null)
                {
                    string unquotedId = id.Trim('"');
                    person = peopleData.Person.FirstOrDefault(p => p.ID.Equals(unquotedId, StringComparison.OrdinalIgnoreCase));
                }

                return person;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        private void SavePeopleToFile(List<Person> people)
        {
            try
            {

                string existingJson = File.Exists(filePath) ? File.ReadAllText(filePath) : "";


                var existingPeopleData = JsonConvert.DeserializeObject<AddressAndPerson>(existingJson) ?? new AddressAndPerson();

                // Check if Person list exists in the existing data
                if (existingPeopleData.Person == null)
                {
                    existingPeopleData.Person = new List<Person>();
                }

                // Add new people to the existing list
                foreach (var peopleid in people)
                {
                    if (existingPeopleData.Person.Where(x => x.ID == peopleid.ID).Count() == 0)
                    {
                        existingPeopleData.Person.Add(peopleid);
                    }
                }
                // Serialize the updated data to JSON format
                string json = JsonConvert.SerializeObject(existingPeopleData, Formatting.Indented);

                // Write the JSON data to the file
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }


        private bool IsDuplicateName(string firstName, string lastName)
        {
            try
            {
                List<Person> people = GetAllPeople();

                if (people != null)
                {
                    return people.Exists(p => p.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) && p.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                return false;
            }

        }

        private string GetLastId(List<Person> people)
        {

            if (people == null || people.Count == 0)
            {
                return "0"; // Initial ID when  Text

            }
            return people.Last().ID;
        }

        private string GenerateNextId(string lastId)
        {
            int numericPart = int.Parse(lastId); // Parse the last ID as an integer
            numericPart++; // Increment the numeric part
            return numericPart.ToString(); // Convert back to string for the new ID
        }
        public List<PersonWithAddress> Viewpeoplewithaddress(string peopleID)
        {
            List<PersonWithAddress> peoplewithaddress = new List<PersonWithAddress>();

            string existingJson = File.Exists(filePath) ? File.ReadAllText(filePath) : "";
            try
            {
                var existingPeopleData = JsonConvert.DeserializeObject<AddressAndPerson>(existingJson) ?? new AddressAndPerson();

                Person person = existingPeopleData.Person.FirstOrDefault(p => p.ID.Equals(peopleID, StringComparison.OrdinalIgnoreCase));
                if (person == null)
                {
                    string unquotedId = peopleID.Trim('"');
                    person = existingPeopleData.Person.FirstOrDefault(p => p.ID.Equals(unquotedId, StringComparison.OrdinalIgnoreCase));
                }

                if (person != null)
                {
                    AddressRepo addressRepo = new AddressRepo(filePath);

                    // Get addresses associated with the person ID
                    List<Address> personAddresses = addressRepo.GetAddressesListByPersonId(person.ID);

                    foreach (var address in personAddresses)
                    {
                        // Create PersonWithAddress object from Person and Address
                        PersonWithAddress personWithAddress = new PersonWithAddress
                        {
                            ID = person.ID,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            DateOfBirth = person.DateOfBirth,
                            Nickname = person.Nickname,
                            // Set address properties based on the retrieved address
                            Line1 = address.Line1,
                            Line2 = address.Line2,
                            Country = address.Country,
                            Postcode = address.Postcode
                        };

                        // Add the person with address to the list
                        peoplewithaddress.Add(personWithAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }

            return peoplewithaddress;
        }

    }
}
