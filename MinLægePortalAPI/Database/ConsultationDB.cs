using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using MinLægePortalModels.Models;
using System.Transactions;
using Dapper;
using System.Web.Http;

namespace MinLægePortalAPI.Database
{
    public class ConsultationDB
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private TimeIntervalDB _timeIntervalDB = new TimeIntervalDB();

        [HttpPost]
        public object InsertConsultationToDatabase(Consultation entity)
        {
            Consultation addedConsultation = null;
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead,
                Timeout = TimeSpan.FromSeconds(60) //<-- Timeout to prevent gridlocks, or any other type of blockage.
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    TimeInterval result = _timeIntervalDB.GetTimeIntervalByDateTimeAndEmployee(entity.Date, entity.EmployeeID);
                    if(!result.IsBooked())
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            try
                            {
                                cmd.CommandText = "UPDATE TimeTable SET reserved = @reserved, consultationId = @consultationId WHERE id = @id";
                                cmd.Parameters.AddWithValue("reserved", true);
                                cmd.Parameters.AddWithValue("consultationId", entity.ConsultationID);
                                cmd.Parameters.AddWithValue("id", result.ID);
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            try
                            {
                                cmd.CommandText = "INSERT INTO Consultation(consultationId,dateTime,description,CPR,employeeId) OUTPUT INSERTED.id VALUES(@consultationId,@dateTime,@description,@CPR,@employeeId)";
                                cmd.Parameters.AddWithValue("consultationId", entity.ConsultationID);
                                cmd.Parameters.AddWithValue("dateTime", entity.Date);
                                cmd.Parameters.AddWithValue("description", entity.Description);
                                cmd.Parameters.AddWithValue("CPR", entity.PatientID);
                                cmd.Parameters.AddWithValue("employeeId", entity.EmployeeID);
                                addedConsultation = (Consultation)cmd.ExecuteScalar();
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("TimeInterval already booked.");
                    }
                }
            }
            return addedConsultation;
        }

        public static Consultation CreateObject(SqlDataReader reader, bool singleRead)
        {
            if (singleRead)
            {
                reader.Read();
            }
            int consultationId = reader.GetInt32(reader.GetOrdinal("consultationId"));
            DateTime date = reader.GetDateTime(reader.GetOrdinal("DateTime"));
            string description = reader.GetString(reader.GetOrdinal("description"));
            int patientId = reader.GetInt32(reader.GetOrdinal("patientId"));
            string employeeId = reader.GetString(reader.GetOrdinal("employeeId"));
            Consultation consultation = new Consultation(consultationId, date, description, patientId, employeeId);
            return consultation;
        }

        //Get Consultation by consultationId
        public Consultation GetConsultationById(object var)
        {
            Consultation consultation = null;
            if (var is string)
            {
                string value = (string)var;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "SELECT * FROM Consultation WHERE consultationId = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            consultation = CreateObject(reader, true);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return consultation;
        }

        //Get Consultation by patientCPR
        public Consultation GetConsultationByPatientCPR(object var)
        {
            Consultation consultation = null;
            if (var is string)
            {
                string value = (string)var;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "SELECT * FROM Consultation WHERE CPR = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            consultation = CreateObject(reader, true);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return consultation;
        }

        //Get Consultation by employeeId
        public Consultation GetConsultationEmployeeId(object var)
        {
            Consultation consultation = null;
            if (var is string)
            {
                string value = (string)var;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "SELECT * FROM Consultation WHERE employeeId = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            consultation = CreateObject(reader, true);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return consultation;
        }

        public bool UpdateConsultation(Consultation entity)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                    try
                    {
                        cmd.CommandText = "UPDATE Consultation SET dateTime = @dateTime, description = @description, CPR = @CPR, employeeId = @employeeId WHERE consultationId = @consultationId";
                        cmd.Parameters.AddWithValue("dateTime", entity.Date);
                        cmd.Parameters.AddWithValue("description", entity.Description);
                        cmd.Parameters.AddWithValue("CPR", entity.PatientID);
                        cmd.Parameters.AddWithValue("employeeId", entity.EmployeeID);
                        cmd.Parameters.AddWithValue("consultationId", entity.ConsultationID);
                        cmd.ExecuteNonQuery();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        result = false;
                        throw e;
                    }
            }
            return result;
        }

        public bool DeleteConsultation(object consultationId)
        {
            bool state = false;
            if (consultationId is string)
            {
                string value = (string)consultationId;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "DELETE FROM Consultation WHERE consultationId = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            state = true;
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }
            return state;
        }
    }
}