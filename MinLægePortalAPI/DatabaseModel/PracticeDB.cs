using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using MinLægePortalModels.Models;
using System.Transactions;
using Dapper;
using MinLægePortalModels.Exceptions;

namespace MinLægePortalAPI.Database
{
    public class PracticeDB
    {

        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public Practice InsertPracticeIntoDatabase(Practice practice)
        {
            string CVR = practice.CVR;
            string name = practice.Name;
            string phoneNumber = practice.PhoneNumber;
            string address = practice.Address;
            string zipCode = practice.ZipCode;
            TimeSpan openTime = practice.OpenTime;
            TimeSpan closeTime = practice.CloseTime;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string checkString = "SELECT * FROM Practice WHERE (CVR = @CVR)";

                var sqlSelectReader = con.ExecuteReader(checkString, new { CVR });

                if (!sqlSelectReader.Read())
                {
                    sqlSelectReader.Close();
                    string queryString = "INSERT INTO Practice (CVR, name, phoneNumber, address, zipCode, openTime, closeTime) VALUES (@CVR, @name, @phoneNumber, @address, @zipCode, @openTime, @closeTime); ";

                    string returnCVR = con.ExecuteScalar<string>(queryString, new
                    {
                        CVR,
                        name,
                        phoneNumber,
                        address,
                        zipCode,
                        openTime,
                        closeTime
                    });

                    Practice result = GetPracticeByCVR(returnCVR);

                    con.Close();
                    return result;
                }
                else
                {
                    throw new AlreadyExistsException("A practice with that cvr already exists.");
                }
            }
        }

        //Get Consultation by consultationId
        public Practice GetPracticeByCVR(string cvr)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Practice WHERE CVR=@CVR";
                Practice result = con.Query<Practice>(sqlString, new { CVR = cvr }).FirstOrDefault();
                return result;
            }
        }
    }
}
