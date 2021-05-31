using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Models
{
    public class DataTransferObjects
    {
        public IEnumerable<Doctor> Doctors { get; set; }
        public Doctor Doctor { get; set; }
        public IEnumerable<Patient> Patients { get; set; }
        public Patient Patient { get; set; }
        public IEnumerable<Consultation> Consultations { get; set; }
        public Consultation Consultation { get; set; }
        public IEnumerable<ConsultationString> ConsultationStrings { get; set; }
        public ConsultationString ConsultationString { get; set; }
        public IEnumerable<TimeInterval> TimeIntervals { get; set; }
        public TimeInterval TimeInterval { get; set; }
        public IEnumerable<Practice> Practices { get; set; }
        public Practice Practice { get; set; }
        public bool IsPatient { get;  set; }
        public string FailMessage { get; set; }
    }
}
