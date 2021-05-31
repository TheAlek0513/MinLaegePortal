using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MinLægePortalModels.Models;

namespace MinLægePortalAPI.Database
{
    public class PatientDB
    {

        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public object Create(Patient entity)
        {
            object o = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO Patient(CPR,firstName,lastName,phoneNumber,address,zipCode) OUTPUT INSERTED.id VALUES(@CPR,@firstName,@lastName,@phoneNumber,@address,@zipCode)";
                        cmd.Parameters.AddWithValue("CPR", entity.CPR);
                        cmd.Parameters.AddWithValue("firstName", entity.FirstName);
                        cmd.Parameters.AddWithValue("lastName", entity.LastName);
                        cmd.Parameters.AddWithValue("phoneNumber", entity.Phone);
                        cmd.Parameters.AddWithValue("address", entity.Address);
                        cmd.Parameters.AddWithValue("zipCode", entity.ZipCode);
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
        public static Patient CreateObject(SqlDataReader reader, bool singleRead)
        {
            if (singleRead)
            {
                reader.Read();
            }
            int patientId = reader.GetInt32(reader.GetOrdinal("patientId"));
            string firstName = reader.GetString(reader.GetOrdinal("firstName"));
            string lastName = reader.GetString(reader.GetOrdinal("lastName"));
            string phone = reader.GetString(reader.GetOrdinal("phoneNumber"));
            string address = reader.GetString(reader.GetOrdinal("address"));
            string zipCode = reader.GetString(reader.GetOrdinal("zipCode"));
            string cpr = reader.GetString(reader.GetOrdinal("CPR"));

            Patient patient = new Patient(patientId, firstName, lastName, phone, address, zipCode, cpr);
            return patient;
        }

        public Patient GetPatientById(object var)
        {
            Patient patient = null;
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
                            cmd.CommandText = "SELECT * FROM Patient WHERE Id = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            patient = CreateObject(reader, true);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                    }
                }
            }
            return patient;
        }

        public bool UpdatePatient(Patient entity)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                    try
                    {
                        //cmd.CommandText = "UPDATE Consultation SET dateTime = @dateTime, description = @description, CPR = @CPR, employeeId = @employeeId WHERE consultationId = @consultationId";
                        //cmd.Parameters.AddWithValue("dateTime", entity.Date);
                        //cmd.Parameters.AddWithValue("description", entity.Description);
                        //cmd.Parameters.AddWithValue("CPR", entity.PatientID);
                        //cmd.Parameters.AddWithValue("employeeId", entity.EmployeeID);
                        //cmd.Parameters.AddWithValue("consultationId", entity.ConsultationID);
                        //cmd.ExecuteNonQuery();
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


        public bool DeletePatient(Patient entity)
        {
            Patient patient = entity;
            bool result = false;
            if (patient.CPR is string)
            {
                string value = (string)patient.CPR;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "DELETE * FROM Patient WHERE CPR = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteNonQuery();
                            result = true;
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                    }
                }
            }
            return result;

        }
        public IEnumerable<Patient> GetAll()
        {
            IEnumerable<Patient> patients = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "SELECT * FROM Patient";
                        var reader = cmd.ExecuteReader();
                        patients = CreateList(reader);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
                return patients;
            }

        }
        /*
         * These methods below are here to create objects of type of this DB class
         */
        public IEnumerable<Patient> CreateList(SqlDataReader reader)
        {
            List<Patient> patients = new List<Patient>();
            while (reader.Read())
            {
                Patient patient = CreateObject(reader, false);
                patients.Add(patient);
            }
            return patients;
        }
    }

}