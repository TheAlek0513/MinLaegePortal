using MinLægePortalAPI.Database;
using MinLægePortalModels.Exceptions;
using MinLægePortalModels.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;
using MinLægePortalModels.Models;
using MinLægePortalAPI.Database;
using System.Data.SqlClient;

namespace MinLægePortalAPI.Controllers
{
    [RoutePrefix("api/Consultation")]
    public class ConsultationController : ApiController
    {
        private ConsultationDB _consultationDB = new ConsultationDB();
        private TimeIntervalController _timeIntervalCtrl = new TimeIntervalController();

        [HttpPost]
        public IHttpActionResult Post([FromBody] Consultation consultation)
        {
            IHttpActionResult result;
            try
            {
            }
            catch (SqlException)
            {
            }
            catch (AlreadyExistsException)
            {
                result = Content(HttpStatusCode.Conflict, $"The dataset already exists.");
            }

            return result;
        }

        [HttpGet]
        //[Route("api/Consultation/id")]
        public IHttpActionResult GetConsultationById(int id)
        {
            IHttpActionResult result;
            try
            {
                Consultation consultationFound = _consultationDB.GetConsultationById(id);
                if (consultationFound != null)
                {
                    result = Ok(consultationFound);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch(ArgumentException)
            {
                result = Content(HttpStatusCode.NotFound, $"The consultation with id #{id} was not found!");
            }
            
            return result;
        }

        [HttpGet]
        [Route("api/Consultation/{patientId}")]
        public IHttpActionResult GetConsultationByPatientId(int patientId)
        {
            IHttpActionResult result;
            try
            {
                Consultation consultationFound = _consultationDB.GetConsultationByPatientId(patientId);
                if (consultationFound != null)
                {
                    result = Ok(consultationFound);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (ArgumentException)
            {
            }

            return result;
        }

        [HttpGet]
        public IHttpActionResult GetConsultationsByPatientId(int patientId)
        {
            IHttpActionResult result;
            try
            {
                List<Consultation> consultationsFound = _consultationDB.GetConsultationsByPatientId(patientId);
                if (consultationsFound != null)
                {
                    result = Ok(consultationsFound);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (ArgumentException)
            {
                result = Content(HttpStatusCode.NotFound, $"The consultations with patientId #{patientId} was not found!");
            }


            return result;
        }

        [HttpGet]
        [Route("api/Consultation/{employeeId}")]
        public IHttpActionResult GetConsultationByEmployeeId(string employeeId)
        {
            IHttpActionResult result;
            try
            {
                Consultation consultationFound = _consultationDB.GetConsultationByEmployeeId(employeeId);
                if (consultationFound != null)
                {
                    result = Ok(consultationFound);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (ArgumentException)
            {
                result = Content(HttpStatusCode.NotFound, $"The consultation with employeeId #{employeeId} was not found!");
            }


            return result;
        }
        [HttpGet]
        public IHttpActionResult GetConsultationsByEmployeeId(string employeeId)
        {
            IHttpActionResult result;
            try
            {
                List<Consultation> consultations = new List<Consultation>();
                consultations = _consultationDB.GetConsultationsByEmployeeId(employeeId);
                if (consultations != null)
                {
                    result = Ok(consultations);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch (ArgumentException)
            {
                result = Content(HttpStatusCode.NotFound, $"The consultations with employeeId #{employeeId} was not found!");
            }


            return result;

        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] Consultation consultation)
        {
            IHttpActionResult result;
            try
            {
                TransactionOptions options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.RepeatableRead,
                    Timeout = TimeSpan.FromSeconds(60) //<-- Timeout to prevent gridlocks, or any other type of blockage.
                };
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    _consultationDB.UpdateConsultation(consultation);
                    scope.Complete();

                    result = Ok();
                }
            }
            catch(SqlException)
            {
                result = Content(HttpStatusCode.InternalServerError, $"Data could not be inserted.");
            }
            return result;
        }
    }
}
