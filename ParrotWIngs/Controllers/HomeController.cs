using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ParrotWIngs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParrotWIngs.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "PW home page";

            ApplicationDbContext db = ApplicationDbContext.Create();

            var user = db.Users.ToList().FirstOrDefault(x => x.Email == User.Identity.Name);
            ViewBag.UserName = user.Email;
            try
            {
                ViewBag.Balance = db.UserAccounts.ToList().FirstOrDefault(x => x.UserId == user.Id).Balance;
            }
            catch
            {
                ViewBag.Balance = null;
            }

            return View();
        }
    }
}
