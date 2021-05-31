using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinLægePortalMVC.Models
{
    public class SelectDoctorViewModel
    {
        [DisplayName("Doctors")]
        public int SelectedId { get; set; }

        public IEnumerable<SelectListItem> Doctors { get; set; }
    }

    public class ConsultationViewModel
    {
        public string PatientCPR { get; set; }
        public string PatientName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }

        public ConsultationViewModel(string patientCPR, string patientName, string employeeId, string employeeName, DateTime dateTime, string description)
        {
            PatientCPR = patientCPR;
            PatientName = patientName;
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            DateTime = dateTime;
            Description = description;
        }
    }
}