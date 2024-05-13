using IECS_CRUD.Helpers;
using IECS_CRUD.Models;
using IECS_CRUD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.BAL
{
    public class AddressBAL
    {
       
        private readonly AddressRepo addressRepo;

        public AddressBAL(string addressFilePath)
        {
            this.addressRepo = new AddressRepo(addressFilePath);
        }


        public void AddAddressToPerson(Address address)
        {

            // Validate constraints

            ValidationHelper.ValidateField(address.Line1, "Line 1", isRequired: true);
            ValidationHelper.ValidateField(address.Line2, "Line 2", isRequired: false, canContainNumbers: false);
            ValidationHelper.ValidateField(address.Country, "Country");
            ValidationHelper.ValidateField(address.Postcode, "Post code");

            // Add the address to the person
            addressRepo.AddAddressToPerson(address);
        }
        public void UpdateAddress(Address address)
        {

            // Validate constraints
            ValidationHelper.ValidateField(address.Line1, "Line 1", isRequired: true);
            ValidationHelper.ValidateField(address.Line2, "Line 2", isRequired: false, canContainNumbers: false);
            ValidationHelper.ValidateField(address.Country, "Country");
            ValidationHelper.ValidateField(address.Postcode, "Post code");

            // Update address
            addressRepo.UpdateAddress(address);
        }

        public Address GetAddressByPersonId(Address address)
        {
            try
            {
                // Call the GetAddressByPersonId method of AddressRepo for get the address with personId
                List<Address> addresses = addressRepo.GetAddressesByPersonId(address);
                // If there are addresses associated with the personId, return the first one
                if (addresses != null && addresses.Count > 0)
                {
                    return addresses[0];
                }
                else
                {
                    // Return null if no addresses are found
                    return null; 
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
                // Call the DeleteAddress method of AddressRepo to delete the Address
                addressRepo.DeleteAddress(id);
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
                // Call the GetAllAddress method of PersonRepo to retrieve all people
                return addressRepo.GetAddressById(id);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

    }
}
