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
using MinLægePortalModels.Exceptions;

namespace MinLægePortalAPI.Database
{
    public class ConsultationDB
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public Consultation InsertConsultationIntoDatabase(Consultation consultation)
        {
            DateTime dateTime = consultation.DateTime;
            string description = consultation.Description;
            int patientId = consultation.PatientId;
            string employeeId = consultation.EmployeeId;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                string checkString = "SELECT * FROM Consultation WHERE (employeeId = @employeeId AND dateTime = @dateTime)";

                var sqlSelectReader = con.ExecuteReader(checkString, new { employeeId, dateTime});

                if (!sqlSelectReader.Read())
                {
                    sqlSelectReader.Close();
                    string checkString2 = "SELECT * FROM Consultation WHERE (patientId = @patientId AND dateTime = @dateTime)";

                    var sqlSelectReader2 = con.ExecuteReader(checkString2, new { patientId, dateTime });

                    if (!sqlSelectReader2.Read())
                    {
                        sqlSelectReader2.Close();
                        string queryString = "INSERT INTO Consultation (dateTime, description, patientId, employeeId) VALUES (@dateTime, @description, @patientId, @employeeId); SELECT SCOPE_IDENTITY()";

                        int id = con.ExecuteScalar<int>(queryString, new
                        {
                            dateTime,
                            description,
                            patientId,
                            employeeId
                        });
                        con.Close();
                        Consultation result = GetConsultationById(id);

                        return result;
                    }
                    else
                    {
                        throw new AlreadyExistsException("A consultation with that patient and at that time already exists.");

                    }
                }
                else
                {
                    throw new AlreadyExistsException("A consultation with that doctor and at that time already exists.");
                }
                    
            }
        }

        //Get Consultation by consultationId
        public Consultation GetConsultationById(int consultationId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Consultation WHERE consultationId = @consultationId";
                Consultation result = con.Query<Consultation>(sqlString, new { consultationId }).FirstOrDefault();
                return result;
            }
        }

        //Get Consultation by patientId
        public Consultation GetConsultationByPatientId(int patientId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Consultation WHERE patientId = @patientId";
                Consultation result = con.Query<Consultation>(sqlString, new { patientId }).FirstOrDefault();
                return result;
            }
        }

        //Get Consultations by patientId
        public List<Consultation> GetConsultationsByPatientId(int patientId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Consultation WHERE patientId = @patientId";
                List<Consultation> result = con.Query<Consultation>(sqlString, new { patientId }).ToList();
                List<Consultation> sortedResult = result.OrderBy(r => r.DateTime).ToList();
                return sortedResult;
            }
        }

        //Get Consultation by employeeId
        public Consultation GetConsultationByEmployeeId(string employeeId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Consultation WHERE employeeId = @employeeId";
                Consultation result = con.Query<Consultation>(sqlString, new { employeeId }).FirstOrDefault();
                return result;
            }
        }

        //Get Consultations by employeeId
        public List<Consultation> GetConsultationsByEmployeeId(string employeeId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sqlString = "SELECT * FROM Consultation WHERE employeeId = @employeeId";
                List<Consultation> result = con.Query<Consultation>(sqlString, new { employeeId }).ToList();
                List<Consultation> sortedResult = result.OrderBy(r => r.DateTime).ToList();
                return sortedResult;
            }
        }

        public void UpdateConsultation(Consultation consultation)
        {
            int consultationId = consultation.ConsultationId;
            DateTime dateTime = consultation.DateTime.AddHours(2);
            string description = consultation.Description;
            int patientId = consultation.PatientId;
            string employeeId = consultation.EmployeeId;
            string roomId = consultation.RoomId;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var sqlStatement = "UPDATE Consultation SET datetime = @dateTime, description = @description, patientId = @patientId, employeeId = @employeeId, roomId = @roomId WHERE consultationId = @consultationId";
                con.Execute(sqlStatement, new {  dateTime, description, patientId, employeeId, roomId, consultationId  });
            }
        }
    }
}