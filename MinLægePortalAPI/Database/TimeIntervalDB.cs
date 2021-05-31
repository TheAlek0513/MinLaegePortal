using Dapper;
using MinLægePortalModels.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Transactions;
namespace MinLægePortalAPI.Database
{
    public class TimeIntervalDB
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public object InsertTimeIntervalToDatabase(TimeInterval entity)
        {
            object o = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO TimeTable(dateTime, employeeId, duration, consultationId, reserved) VALUES (@dateTime,@employeeId,@duration,@consultationId,@reserved)";
                        cmd.Parameters.AddWithValue("dateTime", entity.Date);
                        cmd.Parameters.AddWithValue("employeeId", entity.EmployeeId);
                        cmd.Parameters.AddWithValue("duration", entity.Duration);
                        cmd.Parameters.AddWithValue("consultationId", entity.ConsultationId);
                        cmd.Parameters.AddWithValue("reserved", entity.Reserved);
                        o = cmd.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            return o;
        }

        //CRUD
        public static TimeInterval CreateObject(SqlDataReader reader, bool singleRead)
        {
            if (singleRead)
            {
                reader.Read();
            }
            int timeIntervalId = reader.GetInt32(reader.GetOrdinal("timeTableId"));
            DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("dateTime"));
            string employeeId = reader.GetString(reader.GetOrdinal("employeeId"));
            int duration = reader.GetInt32(reader.GetOrdinal("duration"));
            int consultationId = reader.GetInt32(reader.GetOrdinal("consultationId"));
            bool reserved = reader.GetBoolean(reader.GetOrdinal("reserved"));

            TimeInterval time = new TimeInterval(timeIntervalId, dateTime, employeeId, duration, consultationId, reserved);
            return time;
        }

        public TimeInterval GetTimeIntervalByID(object var)
        {
            TimeInterval time = null;
            if (var is int)
            {
                int value = (int)var;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "SELECT * FROM TimeTable WHERE timeIntervalId = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            time = CreateObject(reader, true);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                    }
                }
            }
            return time;
        }

        public TimeInterval GetTimeIntervalByDateTimeAndEmployee(DateTime date, string employee)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return GetTimeIntervalByDateTimeAndEmployee(date, employee, conn);
            }
        }
        public TimeInterval GetTimeIntervalByDateTimeAndEmployee(DateTime date, string employee, SqlConnection connection)
        {
            string sqlString = "SELECT * FROM TimeTable WHERE employeeId = @employeeId AND dateTime = @dateTime";
            TimeInterval result = connection.Query<TimeInterval>(sqlString, new { employeeId = employee, dateTime = date }).FirstOrDefault();
            return result;
        }

        //public bool BookTimeInterval(Patient entity)
        //{
        //    bool result = false;
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = con.CreateCommand())
        //            try
        //            {
        //                //cmd.CommandText = "UPDATE Consultation SET dateTime = @dateTime, description = @description, CPR = @CPR, employeeId = @employeeId WHERE consultationId = @consultationId";
        //                //cmd.Parameters.AddWithValue("dateTime", entity.Date);
        //                //cmd.Parameters.AddWithValue("description", entity.Description);
        //                //cmd.Parameters.AddWithValue("CPR", entity.PatientID);
        //                //cmd.Parameters.AddWithValue("employeeId", entity.EmployeeID);
        //                //cmd.Parameters.AddWithValue("consultationId", entity.ConsultationID);
        //                //cmd.ExecuteNonQuery();
        //                result = true;
        //            }
        //            catch (Exception e)
        //            {
        //                result = false;
        //                throw e;
        //            }
        //    }
        //    return result;
        //}


        //public bool DeletePatient(Patient entity)
        //{
        //    Patient patient = entity;
        //    bool result = false;
        //    if (patient.CPR is string)
        //    {
        //        string value = (string)patient.CPR;
        //        using (SqlConnection con = new SqlConnection(_connectionString))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = con.CreateCommand())
        //            {
        //                try
        //                {
        //                    cmd.CommandText = "DELETE * FROM Patient WHERE CPR = @value";
        //                    cmd.Parameters.AddWithValue("value", value);
        //                    var reader = cmd.ExecuteNonQuery();
        //                    result = true;
        //                }
        //                catch (Exception e)
        //                {
        //                    throw e;
        //                }

        //            }
        //        }
        //    }
        //    return result;

        //}
    }
}