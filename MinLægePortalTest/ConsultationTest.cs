using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinLægePortalAPI.Controllers;
using MinLægePortalModels.Models;
using System;
using System.Web.Http;
using System.Web.Http.Results;

namespace MinLægePortalTest
{
    [TestClass]
    public class ConsultationTest
    {
        private ConsultationController _consultationCtrl;

        [TestInitialize]
        public void SetUp()
        {
            CleanUp.RemoveData();
            StartUp.InsertData();
            _consultationCtrl = new ConsultationController();
        }

        [TestMethod]
        public void TestBookConsultation1_Valid()
        {
            //Arrange
            int id = 1;
            DateTime date = new DateTime(2021, 8, 10, 14, 30, 0);
            string description = "Test konsultation";
            int patientID = 1;
            string employeeID = "235097";
            Consultation newConsultation = new Consultation(id, date, description, patientID, employeeID);


            //Act
            IHttpActionResult bookedConsultationResult = _consultationCtrl.Post(newConsultation);

            //Assert
            Assert.IsInstanceOfType(bookedConsultationResult, typeof(OkNegotiatedContentResult<Consultation>));
        }

        [TestMethod]
        public void TestBookConsultations2_AlreadyExists()
        {
            //Arrange
            DateTime dateTime = new DateTime(2022, 8, 10, 14, 30, 0);
            string description = "Svar på blodprøver";
            int patientId = 1;
            string employeeId = "235097";

            Consultation newConsultation = new Consultation(dateTime, description, patientId, employeeId);

            //Act
            IHttpActionResult bookedConsultationResult = _consultationCtrl.Post(newConsultation);

            //Assert
            Assert.IsInstanceOfType(bookedConsultationResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestBookConsultations3_NotInfuture()
        {
            //Arrange
            DateTime dateTime = new DateTime(2002, 8, 10, 14, 30, 0);
            string description = "Test konsultation";
            int patientId = 1;
            string employeeId = "235097";

            Consultation newConsultation = new Consultation(dateTime, description, patientId, employeeId);

            //Act
            IHttpActionResult bookedConsultationResult = _consultationCtrl.Post(newConsultation);

            //Assert
            Assert.IsInstanceOfType(bookedConsultationResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestBookConsultations4_NoDescription()
        {
            //Arrange
            DateTime dateTime = new DateTime(2021, 8, 10, 14, 30, 0);
            string description = "";
            int patientId = 1;
            string employeeId = "235097";

            Consultation newConsultation = new Consultation(dateTime, description, patientId, employeeId);

            //Act
            IHttpActionResult bookedConsultationResult = _consultationCtrl.Post(newConsultation);

            //Assert
            Assert.IsInstanceOfType(bookedConsultationResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestBookConsultations5_NoPatient()
        {
            //Arrange
            DateTime dateTime = new DateTime(2021, 8, 10, 14, 30, 0);
            string description = "Test konsultation";
            int patientId = 0;
            string employeeId = "235097";

            Consultation newConsultation = new Consultation(dateTime, description, patientId, employeeId);

            //Act
            IHttpActionResult bookedConsultationResult = _consultationCtrl.Post(newConsultation);

            //Assert
            Assert.IsInstanceOfType(bookedConsultationResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestBookConsultations5_NoEmployee()
        {
            //Arrange
            DateTime dateTime = new DateTime(2021, 8, 10, 14, 30, 0);
            string description = "Test konsultation";
            int patientId = 1;
            string employeeId = "";

            Consultation newConsultation = new Consultation(dateTime, description, patientId, employeeId);

            //Act
            IHttpActionResult bookedConsultationResult = _consultationCtrl.Post(newConsultation);

            //Assert
            Assert.IsInstanceOfType(bookedConsultationResult, typeof(NegotiatedContentResult<string>));
        }
    }
}
