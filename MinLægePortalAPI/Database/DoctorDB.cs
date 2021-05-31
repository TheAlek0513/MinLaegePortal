using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MinLægePortalModels.Models;

namespace MinLægePortalAPI.Database
{
    public class DoctorDB
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public object Create(Doctor entity)
        {
            object o = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO Doctor(employeeId,firstName,lastName,phoneNumber,address,zipCode,CVR) OUTPUT INSERTED.id VALUES(@employeeId,@firstName,@lastName,@phoneNumber,@address,@zipCode,@CVR)";
                        cmd.Parameters.AddWithValue("employeeId", entity.EmployeeID);
                        cmd.Parameters.AddWithValue("firstName",entity.FirstName);
                        cmd.Parameters.AddWithValue("lastName", entity.LastName);
                        cmd.Parameters.AddWithValue("phoneNumber", entity.Phone);
                        cmd.Parameters.AddWithValue("address", entity.Address);
                        cmd.Parameters.AddWithValue("zipCode", entity.ZipCode);
                        cmd.Parameters.AddWithValue("CVR", entity.EmployeeCVR);
                        o = cmd.ExecuteScalar();
                    }catch(Exception e)
                    {
                        throw e;
                    }
                }
            }
            return o;
        }

        public static Doctor CreateObject(SqlDataReader reader, bool singleRead)
        {
            if (singleRead)
            {
                reader.Read();
            }
            string firstName = reader.GetString(reader.GetOrdinal("firstName"));
            string lastName = reader.GetString(reader.GetOrdinal("lastName"));
            string phone = reader.GetString(reader.GetOrdinal("phoneNumber"));
            string address = reader.GetString(reader.GetOrdinal("address"));
            string zipCode = reader.GetString(reader.GetOrdinal("zipCode"));
            string employeeID = reader.GetString(reader.GetOrdinal("employeeId"));
            string employeeCVR = reader.GetString(reader.GetOrdinal("CVR"));
            Doctor doctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeID, employeeCVR);
            return doctor;
        }

        public Doctor GetDoctorById(object var)
        {
            Doctor doctor = null;
            if(var is string)
            {
                string value = (string)var;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "SELECT * FROM Doctor WHERE id = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            doctor = CreateObject(reader, true);
                        }catch (Exception e)
                        {
                            throw e;
                        }

                    }
                }
            }
            return doctor;
        }

        public bool UpdateDoctor(Doctor entity)
        {
            bool result = true;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                    try
                    {
                        cmd.CommandText = "UPDATE Patient SET firstName = @firstName, lastName = @lastName, phoneNumber = @phoneNumber, address = @address, zipCode = @zipCode, CVR = @CVR WHERE employeeId = @employeeId";
                        cmd.Parameters.AddWithValue("employeeId", entity.EmployeeID);
                        cmd.Parameters.AddWithValue("firstName", entity.FirstName);
                        cmd.Parameters.AddWithValue("lastName", entity.LastName);
                        cmd.Parameters.AddWithValue("phoneNumber", entity.Phone);
                        cmd.Parameters.AddWithValue("address", entity.Address);
                        cmd.Parameters.AddWithValue("zipCode", entity.ZipCode);
                        cmd.Parameters.AddWithValue("CVR", entity.EmployeeCVR);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        result = false;
                        throw e;
                    }
            }
            return result;
        }

        public bool DeleteDoctor(object id)
        {
            bool state = false;
            if (id is string)
            {
                string value = (string)id;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "DELETE FROM Doctor WHERE id = @value";
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