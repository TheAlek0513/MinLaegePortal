using Dapper;
using MinLægePortalModels.Models;
using MinLægePortalModels.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace MinLægePortalAPI.Database
{
    public class TimeIntervalDB
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public TimeInterval InsertTimeIntervalToDatabase(TimeInterval timeInterval)
        {
            
            TimeInterval addedTimeInterval = _sqlTimeInterval(timeInterval);
                
            return addedTimeInterval;
        }

        public bool CreateTimetables(List<TimeInterval> times)
        {
            bool status = false;
            foreach(TimeInterval time in times)
            {
                TimeInterval timeInterval = _sqlTimeInterval(time);
            }
            status = true;
            return status;
        }

        public TimeInterval GetTimeIntervalById(int timeTableId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM TimeTable WHERE timeTableId = @timeTableId";
                TimeInterval result = con.Query<TimeInterval>(sqlString, new { timeTableId }).FirstOrDefault();
                return result;
            }
        }

        public TimeInterval GetTimeIntervalByDateTimeAndEmployeeId(DateTime date, string employeeId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM TimeTable WHERE employeeId = @employeeId AND dateTime = @dateTime";
                TimeInterval result = con.Query<TimeInterval>(sqlString, new { employeeId, dateTime = date }).FirstOrDefault();
                return result;
            }
        }

        public List<TimeInterval> GetTimeIntervalsByEmployeeId(string employeeId)
        {
            using(SqlConnection con = new SqlConnection(_connectionString))
            {
                bool reserved = false;
                DateTime dateTime = DateTime.Now;
                string sqlString = "SELECT * FROM TimeTable WHERE employeeId = @employeeId AND reserved = @reserved AND dateTime > @dateTime";

                List<TimeInterval> timeIntervals = con.Query<TimeInterval>(sqlString, new { employeeId, reserved, dateTime }).ToList();
                return timeIntervals;
            }

        }

        public void UpdateTimeInterval(int consultationId, string employeeId, DateTime dateTime)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string queryString = "UPDATE TimeTable SET consultationId = @consultationId, reserved = @reserved WHERE employeeId = @employeeId AND dateTime = @dateTime; SELECT SCOPE_IDENTITY()";

                int reserved = 1;

                int id = con.ExecuteScalar<int>(queryString, new
                {
                    consultationId,
                    reserved,
                    employeeId,
                    dateTime
                });
            }
        }

        //CREATE-body
        private TimeInterval _sqlTimeInterval(TimeInterval timeInterval)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                DateTime dateTime = timeInterval.DateTime;
                string employeeId = timeInterval.EmployeeId;
                int duration = timeInterval.Duration;
                int? consultationId = timeInterval.ConsultationId;
                //SQL can't take bool, så 0 is instead of FALSE
                int reserved = 0;
                if (timeInterval.Reserved)
                {
                    reserved = 1;
                }


                string CheckString = "SELECT * FROM TimeTable WHERE (employeeId = @employeeId AND dateTime = @dateTime)";

                var sqlSelectReader = con.ExecuteReader(CheckString, new { employeeId, dateTime });

                if (!sqlSelectReader.Read())
                {
                    sqlSelectReader.Close();

                    string queryString = "INSERT INTO TimeTable(dateTime,employeeId,duration,consultationId,reserved) VALUES(@dateTime,@employeeId,@duration,@consultationId,@reserved); SELECT SCOPE_IDENTITY();";
                    int id = con.ExecuteScalar<int>(queryString, new
                    {
                        dateTime,
                        employeeId,
                        duration,
                        consultationId,
                        reserved
                    });
                    TimeInterval result = GetTimeIntervalById(id);

                    return result;
                }
                else
                {
                    throw new AlreadyExistsException("A timeInterval does already exist with that doctor at that time");
                }
            }
        }
    }
}
