
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Models
{
    public class Consultation
    {
        public int ConsultationId { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int PatientId { get; set; }
        public string EmployeeId { get; set; }
        public string RoomId { get; set; }

        //Empty constructor for a Consultation
        public Consultation()
        {

        }

        //Constructor for a new Consultation
        public Consultation(DateTime dateTime, string description, int patientId, string employeeId)
        {
            DateTime = dateTime;
            Description = description;
            PatientId = patientId;
            EmployeeId = employeeId;
        }

        //Constructor for an already existing Consultation
        public Consultation(int id, DateTime dateTime, string description, int patientId, string employeeId)
        {
            ConsultationId = id;
            DateTime = dateTime;
            Description = description;
            PatientId = patientId;
            EmployeeId = employeeId;
        }
    }
}
