using Microsoft.AspNet.Identity;
using MinLægePortalModels.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinLægePortalMVC.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        // GET: Patient
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

        //Updates DataTransferObjects for View
        //Takes an input of string, but if empty nothing out side of the method happens
        //If string isn' t empty it gets the timeIntervals for that selected Doctor
        private DataTransferObjects _updateDataTransferObject(string id)
        {
            DataTransferObjects dataTransfer = new DataTransferObjects();
            string userName = User.Identity.GetUserName();
            Consultation consultation = new Consultation();
            Patient pat = GetPatient(userName);
            Practice practice = GetPractice(pat.CVR);
            List<Doctor> doctors = GetDoctorsByCVR(pat.CVR);
            List<Consultation> consultations = GetConsultationsById(pat.PatientId);
            List<ConsultationString> consultationStrings = _getConsultationStrings(consultations);
            ConsultationString consultationString = new ConsultationString();
            List<TimeInterval> timeIntervals = new List<TimeInterval>();

            if (id != "")
            {
                timeIntervals = GetTimeIntervals(id);
            }

            dataTransfer.ConsultationString = consultationString;
            dataTransfer.TimeIntervals = timeIntervals;
            dataTransfer.Practice = practice;
            dataTransfer.Patient = pat;
            dataTransfer.Doctors = doctors;
            dataTransfer.Consultation = consultation;
            dataTransfer.Consultations = consultations;
            dataTransfer.ConsultationStrings = consultationStrings;

            return dataTransfer;
        }

        //Create Patient
        public Patient Create(Patient patient)
        {
            RestRequest patientCreateRequest = new RestRequest("api/Patient", Method.POST);
            patientCreateRequest.AddJsonBody(patient);

            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(patientCreateRequest);

            Patient pat = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                pat = JsonConvert.DeserializeObject<Patient>(response.Content);
            }
            else
            {
                ViewBag.ExceptionAsString = response.Content;
            }

            return pat;
        }

        //Create consultation
        [HttpPost]
        public ActionResult CreateConsultation(Consultation consultation, Patient patient, TimeInterval timeInterval)
        {
            ActionResult viewToReturn;
            Patient newPatient = GetPatientById(patient.PatientId);
            consultation.DateTime = timeInterval.DateTime;
            consultation.EmployeeId = timeInterval.EmployeeId;
            consultation.PatientId = newPatient.PatientId;
            if (timeInterval.DateTime.Minute % 30 != 0) throw new Exception("Illegal minute... How did You get here?? Begone, hacker! callPoliceOn(this.User);");
            if (consultation.EmployeeId != null && patient.PatientId > 0 && consultation.Description != null)
            {
                RestRequest consultationCreateRequest = new RestRequest("api/Consultation", Method.POST);
                consultationCreateRequest.AddJsonBody(consultation);

                RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(consultationCreateRequest);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    viewToReturn = RedirectToAction("Index", "Patient");
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

        //Get Patient by CPR
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

        //Get Patient by patientId
        public Patient GetPatientById(int id)
        {
            Patient patient = null;

            RestRequest request = new RestRequest("api/Patient", Method.GET);
            request.AddParameter("patientId", id);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                patient = JsonConvert.DeserializeObject<Patient>(response.Content);
            }
            return patient;
        }


        //Get TimeIntervals by employeeId
        public ActionResult GetTimeIntervalsByEmployeeId(Doctor doctor)
        {
            string employeeId = doctor.EmployeeId;
            DataTransferObjects dataTransfer = _updateDataTransferObject(employeeId);

            return View("Index", dataTransfer);
        }

        private List<TimeInterval> GetTimeIntervals(string employeeId)
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

        //Get doctor by Id
        public Doctor GetDoctorById(string employeeId)
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

        //Get doctors by Id
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

        //Get practice by CVR
        public Practice GetPractice(string cvr)
        {
            RestRequest request = new RestRequest("api/Practice", Method.GET);
            request.AddParameter("cvr", cvr);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            Practice practice = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                practice = JsonConvert.DeserializeObject<Practice>(response.Content);
            }
            return practice;
        }

        //Get consultations by id
        public List<Consultation> GetConsultationsById(int patientId)
        {
            List<Consultation> consultations = new List<Consultation>();

            RestRequest request = new RestRequest("api/Consultation", Method.GET);
            request.AddParameter("patientId", patientId);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                consultations = JsonConvert.DeserializeObject<List<Consultation>>(response.Content);
            }

            if (consultations.Count > 0)
            {
                //Using Linq to sort the list by the date of consultation
                List<Consultation> SortedList = consultations.OrderBy(con => con.DateTime).ToList();
            }
            else if(consultations == null)
            {
                consultations = new List<Consultation>();
            }
            return consultations;
        }

        //GetConsultationStrings for a list
        private List<ConsultationString> _getConsultationStrings(List<Consultation> consultations)
        {
            List<ConsultationString> consultationAndString = new List<ConsultationString>();
            if (consultations.Count() > 0)
            {
                foreach (Consultation con in consultations)
                {
                    Doctor doctor = GetDoctorById(con.EmployeeId);
                    string doctorString = doctor.FirstName + " " + doctor.LastName;
                    ConsultationString obj = new ConsultationString(con, "", doctorString, con.Description);
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
            return RedirectToAction("IndexPatient", "Consultation", consultation);
        }
    }
}