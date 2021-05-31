using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dependencies;
using MinLægePortalModels.Models;
using MinLægePortalAPI.Database;
using Newtonsoft.Json;
using System.Data.SqlClient;
using MinLægePortalModels.Exceptions;
using System.Transactions;

namespace MinLægePortalAPI.Controllers
{
    [RoutePrefix("api/Patient")]
    public class PatientController : ApiController
    {
        private PatientDB _patientDB = new PatientDB();
        private PracticeController _practiceCtrl = new PracticeController();

        [HttpPost]
        public IHttpActionResult Post([FromBody] Patient patient)
        {
            IHttpActionResult result;
            try
            {
                if(patient.CPR.Trim().Length > 0 && patient.FirstName.Trim().Length > 0 && patient.LastName.Trim().Length > 0 && patient.PhoneNumber.Trim().Length == 8 && patient.Address.Trim().Length > 0 && patient.ZipCode.Trim().Length == 4 && patient.CVR.Trim().Length > 0)
                {
                    Practice practice = _practiceCtrl.GetPracticeByPersonCVR(patient.CVR);
                    if(practice != null)
                    {
                        Patient existingPatient = _patientDB.GetPatientByCPR(patient.CPR);
                        if(existingPatient == null)
                        {
                            TransactionOptions options = new TransactionOptions
                            {
                                IsolationLevel = IsolationLevel.Serializable,
                                Timeout = TimeSpan.FromSeconds(60) //<-- Timeout to prevent gridlocks, or any other type of blockage.
                            };

                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                            {
                                Patient addedPatient = _patientDB.InsertPatientToDatabase(patient);
                                scope.Complete();

                                result = Ok(addedPatient);
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
            catch (SqlException)
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
        public IHttpActionResult GetPatientByCPR(string cpr)
        {
            IHttpActionResult result;
            try
            {
                Patient patientFound = _patientDB.GetPatientByCPR(cpr);
                if (patientFound != null)
                {
                    result = Ok(patientFound);
                }
                else
                {
                    throw new ArgumentException("Not Found");
                }
            }
            catch (ArgumentException)
            {
                result = Content(HttpStatusCode.NotFound, $"The cpr #{cpr} was not found!");
            }

            return result;
        }

        [HttpGet]
        public IHttpActionResult GetPatientById(int patientId)
        {
            IHttpActionResult result;
            try
            {
                Patient patientFound = _patientDB.GetPatientById(patientId);
                if (patientFound != null)
                {
                    result = Ok(patientFound);
                }
                else
                {
                    throw new ArgumentException("Not Found");
                }
            }
            catch (ArgumentException)
            {
                result = Content(HttpStatusCode.NotFound, $"The patientId #{patientId} was not found!");
            }

            return result;
        }
    }
    
}
