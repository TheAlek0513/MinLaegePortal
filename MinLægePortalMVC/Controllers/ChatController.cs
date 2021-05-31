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
    public class ChatController : Controller
    {
        public ActionResult Index(int id)
        {
            Consultation consultation = GetConsultation(id);
            return View(consultation);
        }

        public ActionResult DoctorView(int id)
        {
            dynamic models = new ExpandoObject();
            Consultation consultation = GetConsultation(id);
            models.Consultation = consultation;
            return View(models);
        }

        //Get consultation by consultationId
        public Consultation GetConsultation(int id)
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
    }
}