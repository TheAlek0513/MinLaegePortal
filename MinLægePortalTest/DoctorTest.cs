using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using MinLægePortalAPI.Controllers;
using MinLægePortalModels.Models;

namespace MinLægePortalTest
{

    [TestClass]
    public class DoctorTest
    {
        private DoctorController _doctorCtrl;

        [TestInitialize]
        public void SetUp()
        {
            CleanUp.RemoveData();
            StartUp.InsertData();
            _doctorCtrl = new DoctorController();
        }

        [TestMethod]
        public void TestCreateDoctor1_Valid()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "Brown";
            string phone = "19852015";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "9303";
            string employeeId = "954200";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName,lastName,phone,address,zipCode,employeeId,CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(OkNegotiatedContentResult<Doctor>));
        }

        [TestMethod]
        public void TestCreateDoctor2_AlreadyExists()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "Brown";
            string phone = "19852015";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "9303";
            string employeeId = "235097";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }
        [TestMethod]
        public void TestCreateDoctor3_NoFirstName()
        {
            //Arrange
            string firstName = "";
            string lastName = "Brown";
            string phone = "19852015";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "9303";
            string employeeId = "954200";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreateDoctor4_NoLastName()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "";
            string phone = "19852015";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "9303";
            string employeeId = "954200";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreateDoctor5_NoPhone()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "Brown";
            string phone = "";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "9303";
            string employeeId = "954200";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreateDoctor6_NoAddress()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "Brown";
            string phone = "19852015";
            string address = "";
            string zipCode = "9303";
            string employeeId = "954200";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreateDoctor7_NoZipCode()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "Brown";
            string phone = "19852015";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "";
            string employeeId = "954200";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreateDoctor8_NoEmployee()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "Brown";
            string phone = "19852015";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "9303";
            string employeeId = "";
            string CVR = "987654";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }

        [TestMethod]
        public void TestCreateDoctor9_NoCVR()
        {
            //Arrange
            string firstName = "Emmett";
            string lastName = "Brown";
            string phone = "19852015";
            string address = "Lyon Drive, Lyon Estates, Hill Valley";
            string zipCode = "9303";
            string employeeId = "954200";
            string CVR = "";

            Doctor newDoctor = new Doctor(firstName, lastName, phone, address, zipCode, employeeId, CVR);

            //Act
            IHttpActionResult addedDoctorResult = _doctorCtrl.Post(newDoctor);

            //Assert
            Assert.IsInstanceOfType(addedDoctorResult, typeof(NegotiatedContentResult<string>));
        }
    }
}
