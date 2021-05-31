using MinLægePortalAPI.Database;
using MinLægePortalModels.Models;
using MinLægePortalAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace MinLægePortalTest
{
    public class StartUp
    {
        private static PracticeDB _practiceDB = new PracticeDB();
        private static DoctorController _doctorCtrl = new DoctorController();
        private static PatientController _patientController = new PatientController();
        private static ConsultationController _consultationCtrl = new ConsultationController();
        private static TimeIntervalController _timeIntervalCtrl = new TimeIntervalController();

        public static void InsertData()
        {
            _insertPractice();
            _insertDoctor();
            _insertTimeInterval();
            _insertPatient();
            _insertConsultation();
        }

        private static void _insertPractice()
        {
            TimeSpan openTime = new TimeSpan(8, 0, 0);
            TimeSpan closeTime = new TimeSpan(16, 0, 0);
            Practice practice1 = new Practice("987655", "Test Klinik", "43215678", "Lægegade 23", "9000", openTime, closeTime);
            _practiceDB.InsertPracticeIntoDatabase(practice1);
        }
        private static void _insertDoctor()
        {
            string firstName = "Hans";
            string lastName = "Jonsson";
            string phone = "87651234";
            string address = "Mingade 43";
            string zipCode = "9000";
            string employeeId = "235097";
            string CVR = "987655";

            Doctor doctor1 = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);
            _doctorCtrl.Post(doctor1);
        }
        private static void _insertPatient()
        {
            string cpr = "060154-1337";
            string fName = "sherloque";
            string lName = "holmes";
            string phone = "13371337";
            string address = "221B bagervej";
            string zipcode = "E17A";
            string cvr = "987655";
            Patient patient1 = new Patient(cpr, fName, lName, phone, address, zipcode, cvr);
            _patientController.Post(patient1);

        }
        private static void _insertTimeInterval()
        {
            DateTime dateTime = new DateTime(2022, 8, 10, 14, 30, 0);
            int duration = 15;
            string employeeId = "235097";
            int? consultationId = null;
            bool reserved = false;

            TimeInterval time1 = new TimeInterval(dateTime, duration, employeeId, consultationId, reserved);
            _timeIntervalCtrl.Post(time1);


            DateTime dateTime2 = new DateTime(2021, 8, 10, 14, 30, 0);
            int duration2 = 15;
            string employeeId2 = "235097";
            int? consultationId2 = null;
            bool reserved2 = false;

            TimeInterval time2 = new TimeInterval(dateTime2, duration2, employeeId2, consultationId2, reserved2);
            _timeIntervalCtrl.Post(time2);
        }
        private static void _insertConsultation()
        {
            DateTime dateTime = new DateTime(2022, 8, 10, 14, 30, 0);
            string description = "Svar på blodprøver";
            int patientId = 1;
            string employeeId = "235097";

            Consultation consultation1 = new Consultation(dateTime, description, patientId, employeeId);
            _consultationCtrl.Post(consultation1);
        }
    }
    public class CleanUp
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static void RemoveData()
        {
            _RemoveTimeInterval();
            _RemoveConsultation();
            _RemovePatient("060154 - 1337");
            _RemovePatient("123456-0987");
            _RemoveDoctor("235097");
            _RemoveDoctor("954200");
            _RemovePractice("987655");
        }

        private static void _RemovePractice(string CVR)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string checkString = "SELECT * From Practice WHERE (CVR = @CVR)";
                var sqlSelectReader = conn.ExecuteReader(checkString, new { CVR });

                if (sqlSelectReader.Read())
                {
                    sqlSelectReader.Close();
                    string sqlString = "DELETE FROM Practice WHERE (CVR = @CVR);";
                    conn.ExecuteReader(sqlString, new { CVR });
                }
                conn.Close();
            }
        }
        private static void _RemoveDoctor(string employeeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string checkString = "SELECT * From Doctor WHERE (employeeId = @employeeId)";
                var sqlSelectReader = conn.ExecuteReader(checkString, new { employeeId });

                if (sqlSelectReader.Read())
                {
                    sqlSelectReader.Close();
                    string sqlString = "DELETE FROM Doctor WHERE employeeId=@employeeID;";
                    conn.ExecuteReader(sqlString, new { employeeId = employeeId });
                }
                conn.Close();
            }
        }
        private static void _RemovePatient(string CPR)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string checkString = "SELECT * From Patient WHERE (CPR = @CPR)";
                var sqlSelectReader = conn.ExecuteReader(checkString, new { CPR });

                if (sqlSelectReader.Read())
                {
                    sqlSelectReader.Close();
                    string sqlString = "DELETE FROM Patient WHERE (CPR=@CPR)";
                    conn.ExecuteReader(sqlString, new { CPR });
                }
                conn.Close();
            }

        }
        private static void _RemoveTimeInterval()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sqlString = "DELETE FROM TimeTable; DBCC CHECKIDENT ('TimeTable', RESEED, 0);";
                conn.ExecuteReader(sqlString);

                conn.Close();
            }
        }
        private static void _RemoveConsultation()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sqlString = "DELETE FROM Consultation; DBCC CHECKIDENT ('Consultation', RESEED, 0);";
                conn.ExecuteReader(sqlString);

                conn.Close();

            }
        }
    }
}
