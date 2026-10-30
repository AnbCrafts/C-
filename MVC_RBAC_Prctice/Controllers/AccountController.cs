using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVC_RBAC_Prctice.Models;

namespace MVC_RBAC_Prctice.Controllers
{
    public class AccountController : Controller
    {
        private Repository repo = new Repository();

        // GET: Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                User user = repo.GetUserDetails(model.UserName, model.Password);

                if (user != null)
                {
                    // 1. Create ticket containing user's roles in UserData
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        user.UserName,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(20),
                        model.RememberMe,
                        user.Roles // Storing roles comma-separated: SUPERADMIN,ADMIN,USER
                    );

                    // 2. Encrypt ticket and store in cookie
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        HttpOnly = true
                    };

                    if (model.RememberMe)
                    {
                        authCookie.Expires = authTicket.Expiration;
                    }

                    Response.Cookies.Add(authCookie);

                    // 3. Store friendly display name/id in session if needed
                    Session["UserName"] = user.UserName;
                    Session["FullName"] = string.IsNullOrEmpty(user.FullName) ? user.UserName : user.FullName;
                    Session["Roles"] = user.Roles;

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid User Name or Password.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View(model);
            }
        }

        // POST: Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            // Clear authentication cookie
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
                {
                    Expires = DateTime.Now.AddYears(-1)
                };
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Account/AccessDenied
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
