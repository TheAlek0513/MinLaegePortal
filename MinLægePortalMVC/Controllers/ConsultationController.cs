using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MinLægePortalModels.Models;
using MinLægePortalMVC.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MinLægePortalMVC.Controllers
{
    public class ConsultationController : Controller
    {
        private DataTransferObjects dataTransfer = new DataTransferObjects();

        [HttpGet]
        public ActionResult IndexDoctor(Consultation consultation)
        {
            Consultation consultationTransfer = GetConsultationById(consultation.ConsultationId);
            Doctor doctor = GetDoctor(consultationTransfer.EmployeeId);
            Patient patient = GetPatientById(consultationTransfer.PatientId);

            dataTransfer.Consultation = consultationTransfer;
            dataTransfer.Doctor = doctor;
            dataTransfer.Patient = patient;
            return View(dataTransfer);
        }
        public ActionResult IndexPatient(Consultation consultation)
        {
            Consultation consultationTransfer = GetConsultationById(consultation.ConsultationId);
            Doctor doctor = GetDoctor(consultationTransfer.EmployeeId);
            Patient patient = GetPatientById(consultationTransfer.PatientId);

            dataTransfer.Consultation = consultationTransfer;
            dataTransfer.Doctor = doctor;
            dataTransfer.Patient = patient;
            return View(dataTransfer);
        }

        public ActionResult ChatRoomPatient(Consultation consultation)
         {
            Consultation consultationUpdate = GetConsultationById(consultation.ConsultationId);
            consultationUpdate.RoomId = consultation.RoomId;
            UpdateConsultaion(consultationUpdate);
            return View(consultationUpdate);
        }
        public ActionResult ChatRoomDoctor(Consultation consultation)
        {
            return View(consultation);
        }

        //Get consultation by consultationId
        public Consultation GetConsultationById(int id)
        {
            Consultation consultation = null;

            RestRequest request = new RestRequest("api/Consultation", Method.GET);
            request.AddParameter("id", id);
            RestResponse response = (RestResponse)RestClientManager.RestClientManager.Client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                consultation = JsonConvert.DeserializeObject<Consultation>(response.Content);
            }
            return consultation;
        }

        //Get patient by patientId
        public Patient GetPatientById(int patientId)
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

        //Update consultation
        public void UpdateConsultaion(Consultation consultation)
        {
            RestRequest consultationCreateRequest = new RestRequest("api/Consultation", Method.PUT);
            consultationCreateRequest.AddJsonBody(consultation);

            var response = RestClientManager.RestClientManager.Client.Execute(consultationCreateRequest);

            Consultation con = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                con = JsonConvert.DeserializeObject<Consultation>(response.Content);
            }
            else
            {
                ViewBag.ExceptionAsString = response.Content;
            }
        }

        //Returns to the overview of consultations by either Doctor or Patient
        public ActionResult BackToOverview()
        {
            if(User.IsInRole("Doctor"))
            {
                return RedirectToAction("Consultations", "Doctor");
            }
            else
            {
                return RedirectToAction("Consultations", "Patient");
            }
        }
    }
}