using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MinLægePortalModels.Models;
using MinLægePortalAPI.Database;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace MinLægePortalAPI.Controllers
{
    public class TestController : ApiController
    {
        private TestPatientLogic tPatientLogic = new TestPatientLogic();
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
            try
            {
                IEnumerable<Patient> list = this.tPatientLogic.GetAllPatients();
                if (list != null && list.Any())
                {
                    response = request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception)
            {
                response = request.CreateResponse(HttpStatusCode.NotFound);
            }
            return response;
        }

    }
    public class TestPatientLogic
    {
        private readonly PatientDB _patientDB;

        public TestPatientLogic()
        {
            this._patientDB = new PatientDB();
        }
        public bool CreatePatient(Patient entity)
        {
            bool result = false;
            object o = this._patientDB.Create(entity);
            if (o is string)
            {
                result = true;
            }
            else if (o is bool)
            {
                if ((bool)o == false)
                {
                    result = false;
                }
            }
            return result;
        }

        public Patient GetPatientById(string id)
        {
            Patient patient = null;
            patient = this._patientDB.GetPatientById(id);
            return patient;
        }
        public IEnumerable<Patient> GetAllPatients()
        {
            IEnumerable<Patient> list = null;
            list = this._patientDB.GetAll();
            return list;
        }
    }
}
