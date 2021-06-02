using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Models
{
    public class Doctor : Person
    {
        public string EmployeeId { get; set; }

        
        //Empty constructor for a Doctor
        public Doctor()
        {
            
        }

        //Constructor for a Doctor
        public Doctor(string firstName, string lastName, string phoneNumber, string address, string zipCode, string employeeID, string CVR) : base(firstName, lastName, phoneNumber, CVR, address, zipCode)
        {
            EmployeeId = employeeID;
        }
    }
}
