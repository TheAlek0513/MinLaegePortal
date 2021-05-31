using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MinLægePortalModels.Models
{
    public class Patient : Person
    {
        public int PatientId { get; set; }
        public string CPR { get; set; }

        //Empty constructor for a Patient
        public Patient()
        {

        }

        //Constructor for a new Patient
        public Patient(string cpr, string firstName, string lastName, string phone, string address, string zipCode, string cvr) : base(firstName, lastName, phone, cvr, address, zipCode)
        {
            CPR = cpr;
        }

        //Constructor for an already existing Patient
        public Patient(int patientId, string cpr, string firstName, string lastName, string phone, string address, string zipCode, string cvr) : base(firstName, lastName, phone, cvr, address, zipCode)
        {
            PatientId = patientId;
            CPR = cpr;
        }
        
    }
}
