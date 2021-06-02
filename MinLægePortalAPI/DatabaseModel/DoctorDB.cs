using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using MinLægePortalModels.Models;
using MinLægePortalModels.Exceptions;
using System.Transactions;
using Dapper;
using System.Collections.Generic;

namespace MinLægePortalAPI.Database
{
    public class DoctorDB
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public Doctor InsertDoctorToDatabase(Doctor doctor)
        {
            
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string employeeId = doctor.EmployeeId;
                string firstName = doctor.FirstName;
                string lastName = doctor.LastName;
                string phoneNumber = doctor.PhoneNumber;
                string address = doctor.Address;
                string zipCode = doctor.ZipCode;
                string CVR = doctor.CVR;

                string queryString = "INSERT INTO Doctor(employeeId,firstName,lastName,phoneNumber,address,zipCode,CVR) VALUES (@employeeId,@firstName,@lastName,@phoneNumber,@address,@zipCode,@CVR)";

                int id = con.ExecuteScalar<int>(queryString, new
                {
                    employeeId,
                    firstName,
                    lastName,
                    phoneNumber,
                    address,
                    zipCode,
                    CVR
                });
                con.Close();
                    
                Doctor result = GetDoctorById(employeeId);
                            
                return result;
            }
        }

        public Doctor GetDoctorById(string employeeId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Doctor WHERE employeeId = @employeeId";
                Doctor result = con.Query<Doctor>(sqlString, new { employeeId }).FirstOrDefault();
                return result;
            }
        }

        public List<Doctor> GetDoctorsByCVR(string cvr)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Doctor WHERE CVR = @CVR";
                string CVR = cvr;

                List<Doctor> doctors = con.Query<Doctor>(sqlString, new { CVR = cvr }).ToList();
                return doctors;
            }
        }
    }
}