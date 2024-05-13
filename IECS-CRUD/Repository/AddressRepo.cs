using IECS_CRUD.Helpers;
using IECS_CRUD.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.Repository
{
    public class AddressRepo
    {
        private readonly string addressFilePath; 

        public AddressRepo(string addressFilePath)
        {
            this.addressFilePath = addressFilePath;
        }

        public void AddAddressToPerson(Address address)
        {
            try
            {
                
                List<Address> addresses = GetAllAddresses();

                
                PersonRepo personRepo = new PersonRepo(addressFilePath);

                
                Person existingPerson = personRepo.GetPersonById(address.PersonIDs);
                // Check if a person with the specified ID exists
                if (existingPerson == null)
                {
                    throw new ArgumentException($"Person with ID { address.PersonIDs } does not exist.");
                }

                // Check if addresses list is not null
                if (addresses != null)
                {
                    
                    // Check if the address already exists
                    bool addressExists = AddressExists(addresses, address);
                    if (addressExists)
                    {
                        
                        Address existingAddress = addresses.FirstOrDefault(a =>a.Line1 == address.Line1 && a.Country == address.Country);

                        existingAddress.PersonIDs += "," + address.PersonIDs;

                        UpdateAddress(existingAddress);
                        return;
                    }
                }

                // Generate unique ID for the new address
                string lastId = GetLastAddressId(addresses);
                string newId = GenerateNextId(lastId);

                // Assign the generated ID to the address
                address.ID = newId;

                // Add the address to the list of addresses
                addresses.Add(address);

                // Save the updated list of addresses back to the JSON file
                SaveAddressesToFile(addresses);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public void UpdateAddress(Address updatedaddress)
        {
            try
            {
                if (updatedaddress == null)
                {
                    throw new ArgumentNullException(nameof(updatedaddress), "Updated address cannot be null.");
                }

                
                string existingJson = File.Exists(addressFilePath) ? File.ReadAllText(addressFilePath) : "";
                var existingData = JsonConvert.DeserializeObject<AddressAndPerson>(existingJson) ?? new AddressAndPerson();

               
                Address AddressToUpdate = existingData.Address.FirstOrDefault(p => p.ID.Equals(updatedaddress.ID, StringComparison.OrdinalIgnoreCase));

                
                if (AddressToUpdate == null)
                {
                    string unquotedId = updatedaddress.ID.Trim('"');
                    AddressToUpdate = existingData.Address.FirstOrDefault(p => p.ID.Equals(unquotedId, StringComparison.OrdinalIgnoreCase));
                }

               
                if (AddressToUpdate != null)
                {
                    
                    int index = existingData.Address.FindIndex(p => p.ID.Equals(updatedaddress.ID, StringComparison.OrdinalIgnoreCase));

                    if (index != -1)
                    {
                        // Update the address fields with the new values, if they are not null
                        if (updatedaddress.Line1 != null)
                            existingData.Address[index].Line1 = updatedaddress.Line1;
                        if (updatedaddress.Line2 != null)
                            existingData.Address[index].Line2 = updatedaddress.Line2;
                        if (updatedaddress.Country != null)
                            existingData.Address[index].Country = updatedaddress.Country;
                        if (updatedaddress.Postcode != null)
                            existingData.Address[index].Postcode = updatedaddress.Postcode;
                        if (updatedaddress.PersonIDs != null)
                            existingData.Address[index].PersonIDs = updatedaddress.PersonIDs;

                        
                        string json = JsonConvert.SerializeObject(existingData, Formatting.Indented);

                        
                        File.WriteAllText(addressFilePath, json);
                    }
                    else
                    {
                        throw new ArgumentException($"Address with ID {updatedaddress.ID} not found.");
                    }
                }
                else
                {
                    throw new ArgumentException($"Address with ID {updatedaddress.ID} not found.");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public void DeleteAddress(string id)
        {
            try
            {
                string existingJson = File.Exists(addressFilePath) ? File.ReadAllText(addressFilePath) : "";


                var existingData = JsonConvert.DeserializeObject<AddressAndPerson>(existingJson) ?? new AddressAndPerson();



                
                Address AddressToDelete = existingData.Address.FirstOrDefault(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (AddressToDelete == null)
                {
                    string unquotedId = id.Trim('"');
                    AddressToDelete = existingData.Address.FirstOrDefault(p => p.ID.Equals(unquotedId, StringComparison.OrdinalIgnoreCase));
                }

                if (AddressToDelete != null)
                {
                    existingData.Address.Remove(AddressToDelete);

                    
                    string json = JsonConvert.SerializeObject(existingData, Formatting.Indented);

                    
                    File.WriteAllText(addressFilePath, json);
                }
                else
                {
                    throw new ArgumentException($"Address with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }
        public List<Address> GetAllAddresses()
        {
            List<Address> addresses = new List<Address>();

            try
            {
                if (File.Exists(addressFilePath))
                {
                    string json = File.ReadAllText(addressFilePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var existingData = JsonConvert.DeserializeObject<AddressAndPerson>(json) ?? new AddressAndPerson();
                        
                        if (existingData.Address !=null)
                        {
                            addresses = existingData.Address;
                        }
                        else
                        {
                            //Console.WriteLine("No address objects found in the JSON data.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The address file is empty.");
                    }
                }
                else
                {
                    Console.WriteLine("The address file does not exist.");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }

            return addresses;
        }
        private void SaveAddressesToFile(List<Address> addresses)
        {
            try
            {
                
                string existingJson = File.Exists(addressFilePath) ? File.ReadAllText(addressFilePath) : "";

                
                var existingPeopleData = JsonConvert.DeserializeObject<AddressAndPerson>(existingJson) ?? new AddressAndPerson();

                
                if (existingPeopleData.Address == null)
                {
                    existingPeopleData.Address = new List<Address>();
                }

                //Add Addresses in Json File
                foreach (var AddressID in addresses)
                {
                    if (existingPeopleData.Address.Where(x => x.ID == AddressID.ID).Count() == 0)
                    {

                        existingPeopleData.Address.Add(AddressID);
                    }
                }


                
                string json = JsonConvert.SerializeObject(existingPeopleData, Formatting.Indented);

                
                File.WriteAllText(addressFilePath, json);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        private bool AddressExists(List<Address> addresses, Address address)
        {
            // Check if any address in the list matches the provided address criteria
            return addresses.Any(a => a.Line1 == address.Line1 && a.Country == address.Country);
        }


        private string GetLastAddressId(List<Address> addresses)
        {
            if (addresses==null || addresses.Count == 0)
            {
                return "0"; // Initial ID when the list is empty
            }
            return addresses[addresses.Count - 1].ID;
        }

        private string GenerateNextId(string lastId)
        {
            int numericPart = int.Parse(lastId); // Parse the last ID as an integer
            numericPart++; // Increment the numeric part
            return numericPart.ToString(); // Convert back to string for the new ID
        }

        public List<Address> GetAddressesByPersonId(Address address)
        {
            try
            {
                List<Address> addresses = GetAllAddresses();

                if (addresses != null)
                {

                    
                    // Filter addresses by person ID, Line1, and Country
                    List<Address> personAddresses = addresses.Where(a =>
                        !string.IsNullOrEmpty(a.PersonIDs) &&
                        a.PersonIDs.Split(',').Contains(address.PersonIDs) &&
                        a.Line1 == address.Line1 &&
                        a.Country == address.Country)
                        .ToList();

                    return personAddresses;
                }
                else
                {
                    return new List<Address>(); 
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }

        }
        public List<Address> GetAddressesListByPersonId(string PersonID)
        {
            try
            {
                List<Address> addresses = GetAllAddresses();

                if (addresses != null)
                {


                    // Filter addresses by person ID, Line1, and Country
                    List<Address> personAddresses = addresses.Where(a =>
                        !string.IsNullOrEmpty(a.PersonIDs) &&
                        a.PersonIDs.Split(',').Contains(PersonID))
                        .ToList();

                    return personAddresses;
                }
                else
                {
                    return new List<Address>();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }

        }
        public Address GetAddressById(string id)
        {
            try
            {

                List<Address> addresses = GetAllAddresses();

                // Find the person with the specified ID within the Person list
                var addressdata = JsonConvert.DeserializeObject<AddressAndPerson>(File.ReadAllText(addressFilePath));

                Address address = addressdata.Address.FirstOrDefault(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase));

                if (address == null)
                {
                    string unquotedId = id.Trim('"');
                    address = addressdata.Address.FirstOrDefault(p => p.ID.Equals(unquotedId, StringComparison.OrdinalIgnoreCase));
                }

                return address;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

    }
}
