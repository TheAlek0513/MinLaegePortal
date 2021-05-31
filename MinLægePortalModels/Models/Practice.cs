using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinLægePortalModels.Models
{
    public class Practice
    {
        public string CVR { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        
        public Practice()
        {

        }
        //Constructor for a Practice
        public Practice(string cvr, string name, string phone, string address, string zipCode, TimeSpan openTime, TimeSpan closeTime)
        {
            CVR = cvr;
            Name = name;
            PhoneNumber = phone;
            Address = address;
            ZipCode = zipCode;
            OpenTime = openTime;
            CloseTime = closeTime;
        }
    }
}
