using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.Models
{
    public class AddressAndPerson
    {
        public List<Person> Person { get; set; }
        public List<Address> Address { get; set; }
    
    }
    public class PersonWithAddress
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nickname { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
    }
}
