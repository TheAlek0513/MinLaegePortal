using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MinLægePortalModels.Models;
using MinLægePortalModels.Exceptions;
using System.Transactions;
using Dapper;


namespace MinLægePortalAPI.Database
{
    public class PatientDB
    {

        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public Patient InsertPatientToDatabase(Patient patient)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string CPR = patient.CPR;
                string firstName = patient.FirstName;
                string lastname = patient.LastName;
                string phoneNumber = patient.PhoneNumber;
                string address = patient.Address;
                string zipCode = patient.ZipCode;
                string CVR = patient.CVR;

                string queryString = "INSERT INTO Patient(CPR,firstName,lastName,phoneNumber,address,zipCode,CVR) VALUES (@CPR,@firstName,@lastName,@phoneNumber,@address,@zipCode,@CVR); SELECT SCOPE_IDENTITY()";

                int id = con.ExecuteScalar<int>(queryString, new
                {
                    CPR,
                    firstName,
                    lastname,
                    phoneNumber,
                    address,
                    zipCode,
                    CVR
                });
                var result = GetPatientById(id);
                            
                return result;
            }
        }

        public Patient GetPatientByCPR(string cpr)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Patient WHERE CPR = @CPR";
                Patient result = con.Query<Patient>(sqlString, new { CPR = cpr }).FirstOrDefault();
                return result;
            }
        }

        public Patient GetPatientById(int patientId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Patient WHERE patientId = @patientId";
                Patient result = con.Query<Patient>(sqlString, new { patientId }).FirstOrDefault();
                return result;
            }
        }
    }

}