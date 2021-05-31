using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dependencies;
using MinLægePortalModels.Models;
using MinLægePortalAPI.Database;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Transactions;

namespace MinLægePortalAPI.Controllers
{
    [RoutePrefix("api/TimeInterval")]
    public class TimeIntervalController : ApiController
    {
        private TimeIntervalDB _timeIntervalDB = new TimeIntervalDB();
        private PracticeController _practiceCtrl = new PracticeController();
        private DoctorController _doctorCtrl = new DoctorController();

        [HttpPost]
        [Route("api/TimeInterval/timeInterval")]
        public IHttpActionResult Post([FromBody] TimeInterval timeInterval)
        {
            IHttpActionResult result;
            try
            {
                TransactionOptions options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.Serializable,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    TimeInterval addedTimeInterval = _timeIntervalDB.InsertTimeIntervalToDatabase(timeInterval);
                    scope.Complete();

                    result = Ok(addedTimeInterval);
                }
            }
            catch
            {
                result = Content(HttpStatusCode.InternalServerError, "Could not insert TimeTable into Database");
            }
            return result;
        }

        [HttpPost]
        //[Route("api/TimeInterval/employeeId")]
        public IHttpActionResult AutoPost([FromBody] Doctor doc)
        {
            IHttpActionResult result;
            try
            {
                Doctor doctor = _doctorCtrl.GetDoctorByEmployeeId(doc.EmployeeId);
                Practice practice = _practiceCtrl.GetPracticeByPersonCVR(doctor.CVR);
                TimeSpan openTime = practice.OpenTime;
                TimeSpan closeTime = practice.CloseTime;
                DateTime todaysDate = DateTime.Now;
                todaysDate = todaysDate.AddDays(1.00);
                DateTime date = new DateTime(
                    todaysDate.Year,
                    todaysDate.Month,
                    todaysDate.Day,
                    openTime.Hours,
                    openTime.Minutes,
                    0
                    );

                int duration = 30;

                List<TimeInterval> timeIntervals = new List<TimeInterval>();

                //Creates the date we want to end up on
                //Hardcoded to 7 days from todaysdate
                DateTime upToDate = new DateTime(
                    date.Year,
                    date.Month,
                    date.Day + 4,
                    closeTime.Hours,
                    closeTime.Minutes,
                    0
                    );
                int i = 0;

                //Starts a while loop that runs until we reach upToDate
                while (!(date >= upToDate))
                {
                    if (date.Hour >= closeTime.Hours)
                    {
                        string dayInString = date.DayOfWeek.ToString();

                        switch (date.DayOfWeek)
                        {
                            case DayOfWeek.Friday:
                                date = date.AddDays(2);
                                break;

                            case DayOfWeek.Saturday:
                                date = date.AddDays(1);
                                break;
                        }

                        date = date.AddDays(1);
                        date = date.Date + openTime;

                    }
                    else
                    {
                        TimeInterval timeToAdd = new TimeInterval(date, duration, doctor.EmployeeId, null, false);
                        timeIntervals.Add(timeToAdd);
                        date = date.AddMinutes(duration);
                        i++;
                        Console.WriteLine("Created no. " + i);
                    }


                }

                TransactionOptions options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.Serializable,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    bool status = _timeIntervalDB.CreateTimetables(timeIntervals);
                    scope.Complete();

                    result = Ok(status);
                }
            }
            catch
            {
                result = Content(HttpStatusCode.InternalServerError, "Could not insert TimeTables into Database");
            }
            return result;
        }
        
        [HttpGet]
        //[Route("employeeId")]
        public IHttpActionResult GetTimeIntervalByEmployeeId(string employeeId)
        {
            IHttpActionResult result;
            try
            {
                List<TimeInterval> timeIntervals = _timeIntervalDB.GetTimeIntervalsByEmployeeId(employeeId);
                result = Ok(timeIntervals);
            }
            catch(ArgumentException)
            {
                result = NotFound();
            }
            return result;
        }

        [HttpGet]
        [Route("DateTime/{datetime}/EmployeeId{EmployeeId}")]
        public IHttpActionResult GetTimeIntervalByEmployeeIdAndDateTime([FromBody] string employeeId, DateTime dateTime)
        {
            IHttpActionResult result;
            try
            {
                TimeInterval timeInterval = _timeIntervalDB.GetTimeIntervalByDateTimeAndEmployeeId(dateTime, employeeId);
                result = Ok(timeInterval);
            }
            catch (ArgumentException)
            {
                result = NotFound();
            }
            return result;
        }

        //Update TimeInterval
        //Is called from ConsultationConsultation, inside a transaction
        public void UpdateTimeInterval(int consultationId, string employeeId, DateTime dateTime)
        {
            try
            {
                _timeIntervalDB.UpdateTimeInterval(consultationId, employeeId, dateTime);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }
    }
}
