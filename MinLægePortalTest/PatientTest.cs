using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinLægePortalAPI.Controllers;
using MinLægePortalModels.Models;
using System;
using System.Web.Http;
using System.Web.Http.Results;

namespace MinLægePortalTest
{
    [TestClass]
    public class PatientTest
    {
        private PatientController _patientCtrl;

        [TestInitialize]
        public void SetUp()
        {
            CleanUp.RemoveData();
            StartUp.InsertData();
            _patientCtrl = new PatientController();
        }

        [TestMethod]
        public void TestCreatePatient1_Valid()
        {
            //Arrange
            string cpr = "123456-0987";
            string firstName = "Anne";
            string lastName = "Larsen";
            string phone = "12345678";
            string address = "Testergade 22";
            string zipCode = "9000";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);

            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(OkNegotiatedContentResult<Patient>));
        }

        [TestMethod]
        public void TestCreatepatient2_AlreadyExists()
        {
            //Arrange
            string cpr = "060154-1337";
            string fName = "sherloque";
            string lName = "holmes";
            string phone = "13371337";
            string address = "221B bagervej";
            string zipcode = "E17A";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, fName, lName, phone, address, zipcode, cvr);

            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreatepatient3_NoCPR()
        {
            //Arrange
            string cpr = "";
            string firstName = "Julie";
            string lastName = "Hansen";
            string phone = "87654321";
            string address = "Testervej 22";
            string zipCode = "9000";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);


            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }
        [TestMethod]
        public void TestCreatepatient4_NoFirstName()
        {
            //Arrange
            string cpr = "111111-1111";
            string firstName = "";
            string lastName = "Hansen";
            string phone = "87654321";
            string address = "Testervej 22";
            string zipCode = "9000";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);


            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }
        [TestMethod]
        public void TestCreatepatient5_NoLastName()
        {
            //Arrange
            string cpr = "222222-2222";
            string firstName = "Julie";
            string lastName = "";
            string phone = "87654321";
            string address = "Testervej 22";
            string zipCode = "9000";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);


            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreatepatient6_NoPhone()
        {
            //Arrange
            string cpr = "333333-3333";
            string firstName = "Julie";
            string lastName = "Hansen";
            string phone = "";
            string address = "Testervej 22";
            string zipCode = "9000";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);


            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }
        [TestMethod]
        public void TestCreatepatient7_NoAddress()
        {
            //Arrange
            string cpr = "4444-4444";
            string firstName = "Julie";
            string lastName = "Hansen";
            string phone = "87654321";
            string address = "";
            string zipCode = "9000";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);


            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }
        [TestMethod]
        public void TestCreatepatient8_NoZipCode()
        {
            //Arrange
            string cpr = "555555-5555";
            string firstName = "Julie";
            string lastName = "Hansen";
            string phone = "87654321";
            string address = "Testervej 43";
            string zipCode = "";
            string cvr = "987654";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);


            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }
        [TestMethod]
        public void TestCreatepatient8_NoCVR()
        {
            //Arrange
            string cpr = "666666-6666";
            string firstName = "Julie";
            string lastName = "Hansen";
            string phone = "87654321";
            string address = "Testervej 43";
            string zipCode = "9000";
            string cvr = "";
            Patient newPatient = new Patient(cpr, firstName, lastName, phone, address, zipCode, cvr);


            //Act
            IHttpActionResult addedPatientResult = _patientCtrl.Post(newPatient);

            //Assert
            Assert.IsInstanceOfType(addedPatientResult, typeof(NegotiatedContentResult<string>));
        }
    }
}
