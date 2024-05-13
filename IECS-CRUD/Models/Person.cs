using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.Models
{
    public class Person
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nickname { get; set; }

        public Person(string id, string firstName, string lastName, DateTime dateOfBirth, string nickname)
        {

            ID = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Nickname = nickname;
        }
    }
    

}
