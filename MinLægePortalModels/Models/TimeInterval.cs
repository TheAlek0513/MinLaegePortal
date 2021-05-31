
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Models
{
    public class TimeInterval
    {
        public int TimeTableId { get; set; }
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public int? ConsultationId { get; set; }
        public bool Reserved { get; set; }
        public string EmployeeId { get; set; }

        //Empty Constructor for a new TimeInterval
        public TimeInterval()
        {

        }

        //Constructor for a new TimeInterval
        public TimeInterval(DateTime date, int duration, string employeeId, int? consultationId, bool reserved)
        {
            DateTime = date;
            Duration = duration;
            EmployeeId = employeeId;
            ConsultationId = consultationId;
            Reserved = reserved;
        }

        //Constrcutor for an already existing object
        public TimeInterval(int timeTableId, DateTime dateTime, string employeeId, int duration, int? consultationId, bool reserved)
        {
            TimeTableId = timeTableId;
            DateTime = dateTime;
            Duration = duration;
            EmployeeId = employeeId;
            ConsultationId = consultationId;
            Reserved = reserved;
        }
    }
}
