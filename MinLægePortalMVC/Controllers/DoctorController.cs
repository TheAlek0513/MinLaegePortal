using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RestSharp;
using MinLægePortalModels.Models;
using System.Dynamic;
using MinLægePortalMVC.Models;

namespace MinLægePortalMVC.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        public ActionResult Index()
        {
            DataTransferObjects dataTransfer = _updateDataTransferObject("");
            return View(dataTransfer);
        }

        public ActionResult Consultations()
        {
            DataTransferObjects dataTransfer = _updateDataTransferObject("");
            return View(dataTransfer);
        }

        private DataTransferObjects _updateDataTransferObject(string id)
        {
            DataTransferObjects dataTransfer = new DataTransferObjects();
            string userName = User.Identity.GetUserName();
            Consultation consultation = new Consultation();
            Doctor doc = GetDoctor(userName);
            Practice practice = GetPractice(doc.CVR);
            List<Doctor> doctors = GetDoctorsByCVR(doc.CVR);
            List<Consultation> consultations = GetConsultationsById(doc.EmployeeId);
            List<ConsultationString> consultationStrings = GetConsultationStrings(consultations);
            Patient patient = new Patient();
            ConsultationString consultationString = new ConsultationString();
            List<TimeInterval> timeIntervals = _getTimeIntervals(doc.EmployeeId);

            if (id != "")
            {
                timeIntervals = _getTimeIntervals(id);
            }

            dataTransfer.ConsultationString = consultationString;
            dataTransfer.TimeIntervals = timeIntervals;
            dataTransfer.Practice = practice;
            dataTransfer.Doctor = doc;
            dataTransfer.Doctors = doctors;
            dataTransfer.Consultation = consultation;
            dataTransfer.Consultations = consultations;
            dataTransfer.ConsultationStrings = consultationStrings;
            dataTransfer.Patient = patient;

            return dataTransfer;
        }

        //Create doctor
        public Doctor Create(Doctor doctor)
        {
            RestRequest doctorCreateRequest = new RestRequest("api/Doctor", Method.POST);
            doctorCreateRequest.AddJsonBody(doctor);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(doctorCreateRequest);

            Doctor doc = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                doc = JsonConvert.DeserializeObject<Doctor>(response.Content);
            }
            else
            {
                ViewBag.ExceptionAsString = response.Content;
            }

            return doc;
        }

        //Create consultation
        [HttpPost]
        public ActionResult CreateConsultation(Consultation consultation, Patient patient, TimeInterval timeInterval)
        {
            ActionResult viewToReturn;
            Patient newPatient = GetPatient(patient.CPR);
            consultation.EmployeeId = timeInterval.EmployeeId;
            consultation.PatientId = newPatient.PatientId;
            if (timeInterval.DateTime.Minute % 30 != 0) throw new Exception("Illegal minute... How did You get here?? Begone, hacker! callPoliceOn(this.User);");
            if(consultation.EmployeeId != null && patient.CPR != null && consultation.Description != null)
            {
                consultation.DateTime = timeInterval.DateTime;

                RestRequest consultationCreateRequest = new RestRequest("api/Consultation", Method.POST);
                consultationCreateRequest.AddJsonBody(consultation);

                RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(consultationCreateRequest);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    viewToReturn = RedirectToAction("Index", "Doctor");
                }
                else
                {
                    string failMessage = "Kunne ikke oprette konsultation.";
                    viewToReturn = RedirectToAction("Index", "Fail", new { failMessage });
                }
            }
            else
            {
                string failMessage = "Fejlede grundet manglende informationer. Prøv igen.";
                viewToReturn = RedirectToAction("Index", "Fail", new { failMessage });
            }
            
            return viewToReturn;
        }

        //Autocreate multiple timeIntervals by doctor(employeeId)
        [HttpPost]
        public ActionResult AutoGenerateTimes(Doctor doc)
        {
            RestRequest autoGenerateRequest = new RestRequest("api/TimeInterval", Method.POST);
            autoGenerateRequest.AddJsonBody(doc);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(autoGenerateRequest);

            bool status = false;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                status = JsonConvert.DeserializeObject<bool>(response.Content);
            }
            return RedirectToAction("Index", "Doctor");
        }

        //Get doctor by employeeId
        public Doctor GetDoctor(string employeeId)
        {
            Doctor doctor = null;

            RestRequest request = new RestRequest("api/Doctor", Method.GET);
            request.AddParameter("employeeId", employeeId);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                doctor = JsonConvert.DeserializeObject<Doctor>(response.Content);
            }
            return doctor;
        }

        //Get doctors by CVR
        public List<Doctor> GetDoctorsByCVR(string cvr)
        {
            RestRequest request = new RestRequest("api/Doctor", Method.GET);
            request.AddParameter("cvr", cvr);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);


            List<Doctor> doctors = new List<Doctor>();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                doctors = JsonConvert.DeserializeObject<List<Doctor>>(response.Content);
            }
            return doctors;
        }

        //Get timeIntervals by employeeId
        public ActionResult GetTimeIntervalsByEmployeeId(Doctor doctor)
        {
            string employeeId = doctor.EmployeeId;
            DataTransferObjects dataTransfer = _updateDataTransferObject(employeeId);

            return View("Index",dataTransfer);
        }

        private List<TimeInterval> _getTimeIntervals(String employeeId)
        {
            List<TimeInterval> timeIntervals = new List<TimeInterval>();
            RestRequest request = new RestRequest("api/TimeInterval", Method.GET);
            request.AddParameter("employeeId", employeeId);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                timeIntervals = JsonConvert.DeserializeObject<List<TimeInterval>>(response.Content);
            }
            return timeIntervals;
        }

        //Get patient by patientId
        public Patient GetPatient(int patientId)
        {
            Patient patient = null;

            RestRequest request = new RestRequest("api/Patient", Method.GET);
            request.AddParameter("patientId", patientId);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                patient = JsonConvert.DeserializeObject<Patient>(response.Content);
            }
            return patient;
        }

        //Get patient by CPR
        public Patient GetPatient(string cpr)
        {
            Patient patient = null;

            RestRequest request = new RestRequest("api/Patient", Method.GET);
            request.AddParameter("cpr", cpr);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                patient = JsonConvert.DeserializeObject<Patient>(response.Content);
            }
            return patient;
        }
     
        //Get practice by CVR
        public Practice GetPractice(string cvr)
        {
            RestRequest request = new RestRequest("api/Practice", Method.GET);
            request.AddParameter("cvr", cvr);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            Practice practice = null; ;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                practice = JsonConvert.DeserializeObject<Practice>(response.Content);
            }
            return practice;
        }

        
        //Get consultations by employeeId
        public List<Consultation> GetConsultationsById(string employeeId)
        {
            RestRequest request = new RestRequest("api/Consultation", Method.GET);
            request.AddParameter("employeeId", employeeId);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            List<Consultation> consultations = new List<Consultation>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                consultations = JsonConvert.DeserializeObject<List<Consultation>>(response.Content);
            }
            return consultations;
        }

        //Get consultationStrings for a list
        public List<ConsultationString> GetConsultationStrings(List<Consultation> consultations)
        {
            List<ConsultationString> consultationAndString = new List<ConsultationString>();
            if (consultations.Count() > 0)
            {
                foreach (Consultation con in consultations)
                {
                    Patient patient = GetPatient(con.PatientId);
                    string patientString = patient.FirstName + " " + patient.LastName;
                    ConsultationString obj = new ConsultationString(con, patientString, "", con.Description);
                    consultationAndString.Add(obj);
                }
            }
            else
            {
                consultationAndString.Add(new ConsultationString(null, "", "", "Ingen konsultationer"));
            }

            return consultationAndString;
        }
        
        //Get the selected consultation shown in it's own page
        [HttpGet]
        public ActionResult ShowSelectedConsultation(ConsultationString consulString)
        {
            Consultation consultation = consulString.Consultation;
            if (consultation == null)
            {
                return View("Index");
            }
            return RedirectToAction("IndexDoctor", "Consultation", consultation);
        }
    }
}