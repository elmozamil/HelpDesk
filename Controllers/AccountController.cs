using HelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HelpDesk.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Login(Account model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            //if (Membership.ValidateUser(model.userName, model.password))
            if (ValidateUser(model.userName, model.password))
            {
                FormsAuthentication.SetAuthCookie(model.userName, model.rememberMe);

                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(returnUrl);
                }
                return this.RedirectToAction("Index", "Home");
            }
            this.ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            return this.View(model);
        }


        private bool ValidateUser(string username, string password)
        {
            if(username.Equals(password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Home");
        }
    }
}