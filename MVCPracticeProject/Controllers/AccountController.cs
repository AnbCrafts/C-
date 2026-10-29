using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVCPracticeProject.Models;
using MVCPracticeProject.ViewModels;

namespace MVCPracticeProject.Controllers
{
    public class AccountController : Controller
    {
        private DoctorRepository repo = new DoctorRepository();

        // GET: Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(DoctorRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var doctor = new Doctor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialization = model.Specialization,
                    DepartmentId = model.DepartmentId,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password
                };

                string result = repo.RegisterDoctor(doctor);
                if (result == "Success")
                {
                    TempData["SuccessMessage"] = "Registration successful! Please login with your credentials.";
                    return RedirectToAction("Login");
                }

                ViewBag.Error = result;
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(DoctorLoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var doctor = repo.ValidateDoctor(model.Email, model.Password);
                if (doctor != null)
                {
                    // Set session variables
                    Session["DoctorId"] = doctor.DoctorId;
                    Session["DoctorName"] = "Dr. " + doctor.FirstName + " " + doctor.LastName;
                    Session["DoctorEmail"] = doctor.Email;

                    // Set authentication cookie
                    FormsAuthentication.SetAuthCookie(doctor.Email, model.RememberMe);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "DocPatient");
                }

                ModelState.AddModelError("", "Invalid Email or Password.");
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }
    }
}
