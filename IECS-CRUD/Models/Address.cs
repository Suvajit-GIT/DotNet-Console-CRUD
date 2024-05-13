using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.Models
{
    public class Address
    {
        public string ID { get; set; }
        public string PersonIDs { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }


        public Address(string id,string personids, string line1, string line2, string country, string postcode)
        {

            ID = id;
            PersonIDs = personids;
            Line1 = line1;
            Line2 = line2;
            Country = country;
            Postcode = postcode;
        }
    }
   
}
