using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParrotWIngs.Controllers
{
    public class SignUpController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            ViewBag.UserIsAuthenticated = User.Identity.IsAuthenticated;

            return View();
        }
    }
}
