using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using MinLægePortalModels.Models;

namespace MinLægePortalAPI.Database
{
    public class PracticeDB
    {

        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public object Create(Practice entity)
        {
            object o = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO Practice(CVR,name,phoneNumber,address,zipCode,openTime,closeTime) OUTPUT INSERTED.id VALUES(@CVR,@name,@phoneNumber,@address,@zipCode,@openTime,@closeTime)";
                        cmd.Parameters.AddWithValue("CVR", entity.CVR);
                        cmd.Parameters.AddWithValue("name", entity.Name);
                        cmd.Parameters.AddWithValue("phoneNumber", entity.Phone);
                        cmd.Parameters.AddWithValue("address", entity.Address);
                        cmd.Parameters.AddWithValue("zipCode", entity.ZipCode);
                        cmd.Parameters.AddWithValue("openTime", entity.OpenTime);
                        cmd.Parameters.AddWithValue("closeTime", entity.CloseTime);
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

        public static Practice CreateObject(SqlDataReader reader, bool singleRead)
        {
            if (singleRead)
            {
                reader.Read();
            }
            string CVR = reader.GetString(reader.GetOrdinal("CVR"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string phone = reader.GetString(reader.GetOrdinal("phoneNumber"));
            string address = reader.GetString(reader.GetOrdinal("address"));
            string zipCode = reader.GetString(reader.GetOrdinal("zipCode"));
            TimeSpan openTime = reader.GetTimeSpan(reader.GetOrdinal("openTime"));
            TimeSpan closeTime = reader.GetTimeSpan(reader.GetOrdinal("closeTime"));
            Practice practice = new Practice(CVR, name, phone, address, zipCode, openTime, closeTime);
            return practice;
        }
        
        public Practice GetPracticeByCVR(object var)
        {
            Practice practice = null;
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
                            cmd.CommandText = "SELECT * FROM Practice WHERE CVR = @value";
                            cmd.Parameters.AddWithValue("value", value);
                            var reader = cmd.ExecuteReader();
                            practice = CreateObject(reader, true);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                    }
                }
            }
            return practice;
        }

        public bool UpdatePractice(Practice entity)
        {
            bool result = false;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                    try
                    {
                        cmd.CommandText = "UPDATE Practice SET name = @name, phoneNumber = @phoneNumber, address = @address, zipCode = @zipCode, openTime = @openTime, closeTime = @closeTime WHERE CVR = @CVR";
                        cmd.Parameters.AddWithValue("CVR", entity.CVR);
                        cmd.Parameters.AddWithValue("name", entity.Name);
                        cmd.Parameters.AddWithValue("phoneNumber", entity.Phone);
                        cmd.Parameters.AddWithValue("address", entity.Address);
                        cmd.Parameters.AddWithValue("zipCode", entity.ZipCode);
                        cmd.Parameters.AddWithValue("openClose", entity.OpenTime);
                        cmd.Parameters.AddWithValue("closeClose", entity.CloseTime);
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

        public bool DeletePractice(object CVR)
        {
            bool state = false;
            if (CVR is string)
            {
                string value = (string)CVR;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "DELETE FROM Practice WHERE CVR = @value";
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
