using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLSite.Models;
using System.Web.Security;
using System.Configuration;

namespace TLSite.Controllers
{  
    public class AccountController : Controller
    {
        UserModel userModel = new UserModel();

        public ActionResult LogOn(string returnUrl)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["returnUrl"] = returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(string username, string userpwd, string returnUrl)
        {
            bool rememberme = true;
            ViewData["rootUri"] = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            //if (DateTime.Now > new DateTime(2014, 12, 20))
            //{
            //    ModelState.AddModelError("modelerror", "系统到期了，已使用了30天！");
            //    return View("LogOn");
            //}

            if (ModelState.IsValid)
            {
                var userInfo = userModel.ValidateUser(username, userpwd);
                if (userInfo != null)
                {
                    
                    userModel.SignIn(username, rememberme);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            userInfo.username,
                            DateTime.Now,
                            DateTime.Now.AddMinutes(1440),
                            rememberme,
                            userInfo.role + "|" + userInfo.uid + "|" + userInfo.userkind.ToString() + "|" + userInfo.imgurl,
                            FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("modelerror", "帐号或密码错误，请重新输入");
            }

            return View("LogOn");
        }

        public ActionResult LogOff()
        {
            userModel.SignOut();

            return RedirectToAction("Logon", "Account");
        }

    }
}