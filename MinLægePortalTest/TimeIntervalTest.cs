using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinLægePortalAPI.Controllers;
using MinLægePortalModels.Models;
using System;
using System.Web.Http;
using System.Web.Http.Results;

namespace MinLægePortalTest
{
    /// <summary>
    /// Summary description for TimeIntervalTest
    /// </summary>
    [TestClass]
    public class TimeIntervalTest
    {
        private TimeIntervalController _TimeICtrl;
        private DoctorController _doctorCtrl;


        [TestInitialize]

        public void setup()
        {
            CleanUp.RemoveData();
            StartUp.InsertData();
            _TimeICtrl = new TimeIntervalController();
            _doctorCtrl = new DoctorController();
        }

        [TestMethod]
        public void TestCreateTimeTable()
        {
            //Arrange
            DateTime date = new DateTime(2077, 8, 10, 14, 30, 0);
            int duration = 15;
            int? consultationId = null;
            bool reserved = false;
            string EmployeeId = "235097";
            TimeInterval newTimeInterval = new TimeInterval(date, duration, EmployeeId, consultationId, reserved);

            //Act 
            IHttpActionResult addedTimeIntervalResult = _TimeICtrl.Post(newTimeInterval);

            //Assert
            Assert.IsInstanceOfType(addedTimeIntervalResult, typeof(OkNegotiatedContentResult<TimeInterval>));
        }

        [TestMethod]
        public void TestAutoCreateTimeTables()
        {
            //Arrange
            string employeeId = "235097";

            //Act 
            Console.WriteLine("Test af auto timetables");
            Doctor doctor = (Doctor)_doctorCtrl.GetDoctorByEmployeeId(employeeId);
            IHttpActionResult addedTimeIntervalResult = _TimeICtrl.AutoPost(doctor);

            //Assert
            Assert.IsInstanceOfType(addedTimeIntervalResult, typeof(OkNegotiatedContentResult<bool>));
        }
    }
}
