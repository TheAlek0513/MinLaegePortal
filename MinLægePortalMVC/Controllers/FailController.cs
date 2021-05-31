using MinLægePortalModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinLægePortalMVC.Controllers
{
    public class FailController : Controller
    {
        // GET: Fail
        public ActionResult Index(string failMessage)
        {
            DataTransferObjects dataTransfer = new DataTransferObjects();
            dataTransfer.FailMessage = failMessage;
            return View(dataTransfer);
        }

        public ActionResult BackTo(string returnUrl)
        {
            return Redirect(returnUrl);
        }
    }
}