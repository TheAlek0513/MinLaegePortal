using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MinLægePortalMVC.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private ApplicationUserManager _userManager;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginCheck()
        {
            if(User != null)
            {
                string rolename = _userManager.GetRoles(User.Identity.Name).FirstOrDefault();
                string ctrlString = "";
                if (rolename.Equals("Patient"))
                {
                    ctrlString = "Patient";
                }
                else if (rolename.Equals("Doctor"))
                {
                    ctrlString = "Doctor";
                }
                else if(rolename.Equals("Administrator"))
                {
                    ctrlString = "Doctor";
                }
                return RedirectToAction("Index", ctrlString);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}