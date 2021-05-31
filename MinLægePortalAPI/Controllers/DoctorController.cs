using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinLægePortalAPI.Database;
using MinLægePortalModels.Models;
using MinLægePortalModels.Exceptions;
using System.Transactions;

namespace MinLægePortalAPI.Controllers
{
    [RoutePrefix("api/Doctor")]
    public class DoctorController : ApiController
    {
        private DoctorDB _doctorDB = new DoctorDB();
        private PracticeController _practiceCtrl = new PracticeController();

        [HttpPost]
        public IHttpActionResult Post([FromBody] Doctor doctor)
        {
            IHttpActionResult result;
            try
            {
                if (doctor.CVR.Trim().Length > 0 && doctor.FirstName.Trim().Length > 0 && doctor.LastName.Trim().Length > 0 && doctor.PhoneNumber.Trim().Length == 8 && doctor.Address.Trim().Length > 0 && doctor.ZipCode.Trim().Length == 4 && doctor.EmployeeId.Trim().Length > 0)
                {
                    Practice practice = _practiceCtrl.GetPracticeByPersonCVR(doctor.CVR);
                    if(practice != null)
                    {
                        Doctor existingDoctor = _doctorDB.GetDoctorById(doctor.EmployeeId);
                        if (existingDoctor == null)
                        {
                            TransactionOptions options = new TransactionOptions
                            {
                                IsolationLevel = IsolationLevel.Serializable,
                                Timeout = TimeSpan.FromSeconds(60) //<-- Timeout to prevent gridlocks, or any other type of blockage.
                            };

                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                            {
                                Doctor addedDoctor = _doctorDB.InsertDoctorToDatabase(doctor);
                                scope.Complete();

                                result = Ok(addedDoctor);
                            }
                        }
                        else
                        {
                            throw new AlreadyExistsException();
                        }
                    }
                    else
                    {
                        throw new PracticeNotFoundException();
                    }
                }
                else
                {
                    result = Content(HttpStatusCode.Conflict, "The Arguments provided were invalid.");
                }
            }
            catch(SqlException)
            {
                result = Content(HttpStatusCode.InternalServerError, "Could not insert data into database.");
            }
            catch (AlreadyExistsException)
            {
                result = Content(HttpStatusCode.Conflict, $"The dataset already exists.");
            }
            catch (PracticeNotFoundException)
            {
                result = Content(HttpStatusCode.NotFound, "The practice with the typed CVR was not found.");
            }
            return result;
        }

        [HttpGet]
        [Route("api/Doctor/employeeId")]
        public IHttpActionResult GetDoctorById(string employeeId)
        {
            IHttpActionResult result;
            try
            {
                Doctor doctor = _doctorDB.GetDoctorById(employeeId);
                result = Ok(doctor);
            }
            catch (ArgumentException)
            {
                result = NotFound();
            }
            return result;
        }

        public Doctor GetDoctorByEmployeeId(string employeeId)
        {
            Doctor doctor = null;
            try
            {
                doctor = _doctorDB.GetDoctorById(employeeId);
            }
            catch (ArgumentException e)
            {
                throw (e);
            }
            return doctor;
        }

        [HttpGet]
        //[Route("api/Doctor/cvr")]
        public IHttpActionResult GetDoctorsByCVR(string cvr)
        {
            IHttpActionResult result;
            try
            {
                List<Doctor> doctors = _doctorDB.GetDoctorsByCVR(cvr);
                result = Ok(doctors);
            }
            catch (ArgumentException)
            {
                result = NotFound();
            }
            return result;
        }

        //[HttpGet]
        //public IHttpActionResult All()
        //{
        //    IHttpActionResult result;
        //    try
        //    {
        //        List<Doctor> doctors = _doctorDB.GetAllDoctors();
        //        result = Ok(doctors);
        //    }
        //    catch(ArgumentException)
        //    {
        //        result = NotFound();
        //    }
        //    return result;
        //}

    }
}
