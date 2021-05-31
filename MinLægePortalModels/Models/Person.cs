
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Models
{
    public abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string CVR { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }

        //Empty constructor for a Person
        public Person()
        {
            
        }

        //Constructor for a Person
        public Person(string firstName, string lastName, string phoneNumber, string cvr, string address, string zipCode)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            CVR = cvr;
            Address = address;
            ZipCode = zipCode;
        }
    }
}
