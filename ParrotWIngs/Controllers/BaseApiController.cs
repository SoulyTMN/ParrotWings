using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ParrotWIngs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace ParrotWIngs.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        private ApplicationUser _member;

        public static ApplicationUser FindUserByEmail(string _email)
        {
            ApplicationDbContext db = ApplicationDbContext.Create();
            return db.Users.ToList().Find(x => x.Email == _email);
        }

        public static ApplicationUser FindUserByName(string _name)
        {
            ApplicationDbContext db = ApplicationDbContext.Create();
            return db.Users.ToList().Find(x => x.UserName == _name);
        }

        public string UserIdentityId
        {
            get
            {
                string userId = null;
                try
                {
                    if (User.Identity.Name != null)
                        userId = FindUserByName(User.Identity.Name).Id;
                    else if (Thread.CurrentPrincipal.Identity.Name != null)
                        userId = FindUserByName(Thread.CurrentPrincipal.Identity.Name).Id;
                }
                catch (Exception e)
                {
                }
                return userId;
            }
        }

        public ApplicationUser UserRecord
        {
            get
            {
                try
                {
                    if (_member != null)
                    {
                        return _member;
                    }
                    if (Thread.CurrentPrincipal.Identity.Name != null)
                        _member = FindUserByEmail(Thread.CurrentPrincipal.Identity.Name);
                }
                catch (Exception e)
                {
                }

                return _member;
            }
            set
            {
                _member = value;
            }
        }
    }
}