 
using IECS_CRUD.Helpers;
using IECS_CRUD.Models;
using IECS_CRUD.Repository;
using System;
using System.Collections.Generic;

namespace IECS_CRUD.BAL
{
    public class PersonBAL
    {
        private readonly PersonRepo personRepo; 

        public PersonBAL(string filePath) 
        {
            personRepo = new PersonRepo(filePath); 
        }

        public void AddPerson(Person person)
        {
            try
            {
                // Validate constraints
                ValidationHelper.ValidateField(person.FirstName, "First Name", isRequired: true, canContainNumbers: false);
                ValidationHelper.ValidateField(person.LastName, "Last Name", isRequired: true, canContainNumbers: false);
                ValidationHelper.ValidateField(person.DateOfBirth.ToString(), "Date of Birth", isRequired: true);
                ValidationHelper.ValidateField(person.Nickname, "Nickname", canContainNumbers: false);
                // Call the AddPerson method of PersonRepo to add the person
                personRepo.AddPerson(person);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public List<Person> ViewPerson()
        {
            try
            {
                // Call the GetAllPeople method of PersonRepo to retrieve all people
                return personRepo.GetAllPeople();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public List<PersonWithAddress> Viewpeoplewithaddress(string id)
        {
            try
            {
                // Call the GetAllPeople method of PersonRepo to retrieve all people
                return personRepo.Viewpeoplewithaddress(id);
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
                // Call the Get People method of PersonRepo to retrieve Search People
                return personRepo.SearchPeople(searchTerm);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }
        public Person GetPersonById(string id)
        {
            try
            {
                // Call the GetAllPeople method of PersonRepo to retrieve all people
                return personRepo.GetPersonById(id);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

        public void UpdatePerson(Person person)
        {
            try
            {
                // Validate constraints
                ValidationHelper.ValidateField(person.FirstName, "First Name", isRequired: true, canContainNumbers: false);
                ValidationHelper.ValidateField(person.LastName, "Last Name", isRequired: true, canContainNumbers: false);
                ValidationHelper.ValidateField(person.DateOfBirth.ToString(), "Date of Birth", isRequired: true);
                ValidationHelper.ValidateField(person.Nickname, "Nickname", canContainNumbers: false);
                // Call the AddPerson method of PersonRepo to add the person
                personRepo.UpdatePerson(person);
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
                // Call the DeletePerson method of PersonRepo to delete the person
                personRepo.DeletePerson(id);
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex);
                throw;
            }
        }

    }
}
