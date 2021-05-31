using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Http;
using MinLægePortalAPI.Database;
using MinLægePortalModels.Models;
using MinLægePortalModels.Exceptions;
using System.Transactions;

namespace MinLægePortalAPI.Controllers
{
    [RoutePrefix("api/Practice")]
    public class PracticeController : ApiController
    {
        private PracticeDB _practiceDB = new PracticeDB();

        [HttpPost]
        public IHttpActionResult Post([FromBody] Practice practice)
        {
            IHttpActionResult result;
            try
            {
                if (practice.CVR.Trim().Length > 0 && practice.Name.Trim().Length > 0 && practice.PhoneNumber.Trim().Length == 8 && practice.Address.Trim().Length > 0 && practice.ZipCode.Trim().Length == 4)
                {
                    TransactionOptions options = new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.Serializable,
                        Timeout = TimeSpan.FromSeconds(60) //<-- Timeout to prevent gridlocks, or any other type of blockage.
                    };

                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        Practice addedPractice = _practiceDB.InsertPracticeIntoDatabase(practice);
                        scope.Complete();

                        result = Ok(addedPractice);
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
        [Route("api/Practice/cvr")]
        public IHttpActionResult GetPracticeByCVR(string cvr)
        {
            IHttpActionResult result;
            try
            {
                Practice practice = _practiceDB.GetPracticeByCVR(cvr);
                result = Ok(practice);
            }
            catch (ArgumentException)
            {
                throw new PracticeNotFoundException();
            }
            return result;
        }

        public Practice GetPracticeByPersonCVR(string cvr)
        {
            Practice practice = null;
            try
            {
                practice = _practiceDB.GetPracticeByCVR(cvr);
            }
            catch (ArgumentException)
            {
                throw new PracticeNotFoundException();
            }
            return practice;
        }
    }
}