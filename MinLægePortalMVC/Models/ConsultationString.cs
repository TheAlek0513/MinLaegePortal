using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Models
{
    public class ConsultationString
    {
        public Consultation Consultation { get; set; }
        public string DateString { get; set; }
        public string PatientString { get; set; }
        public string EmployeeString { get; set; }
        public string Description { get; set; }

        //Empty Contructor
        public ConsultationString()
        {
            //Empty
        }

        //Constructor with parameters
        public ConsultationString(Consultation consultation, string patientString, string employeeString, string description)
        {
            Consultation = consultation;
            if(Consultation != null)
            {
                DateString = _getDateTimeString(Consultation.DateTime);
            }
            PatientString = patientString;
            EmployeeString = employeeString;
            Description = description;
        }

        //Creates the string of date for the Consultation
        private string _getDateTimeString(DateTime dateTime)
        {
            string dayString = "";
            string dateString = "";
            string timeString = "";

            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dayString = "Mandag";
                    break;

                case DayOfWeek.Tuesday:
                    dayString = "Tirsdag";
                    break;

                case DayOfWeek.Wednesday:
                    dayString = "Onsdag";
                    break;

                case DayOfWeek.Thursday:
                    dayString = "Torsdag";
                    break;

                case DayOfWeek.Friday:
                    dayString = "Fredag";
                    break;

                case DayOfWeek.Saturday:
                    dayString = "Lørdag";
                    break;

                case DayOfWeek.Sunday:
                    dayString = "Søndag";
                    break;
            }

            List<string> months = new List<string>() {
                "januar", "februar", "marts", "april", "maj", "juni", "juli", "august", "september", "oktober", "november", "december"
            };

            dateString = dateTime.Day + ". " + months.ElementAt(dateTime.Month - 1) + " " + dateTime.Year;

            timeString = dateTime.Hour + ":";
            if(dateTime.Minute == 0)
            {
                timeString += "00";
            }
            else
            {
                timeString += dateTime.Minute;
            }

            string dateTimeString = dayString + " den " + dateString + " kl. " + timeString;

            return dateTimeString;
        }
    }
}
