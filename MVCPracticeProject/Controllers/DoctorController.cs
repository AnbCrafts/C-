using System;
using System.Web.Mvc;
using MVCPracticeProject.Models;
using MVCPracticeProject.Filters;

namespace MVCPracticeProject.Controllers
{
    public class DoctorController : Controller
    {
        private DoctorRepository repo = new DoctorRepository();

        // GET: Doctor
        public ActionResult Index()
        {
            var doctors = repo.GetAllDoctors();
            return View(doctors);
        }

        public ActionResult DocList()
        {
            var doctors = repo.GetAllDoctors();
            return View(doctors);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            var doctor = repo.GetSingleDoctorById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        // GET: Doctor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            try
            {
                string result = repo.CreateDoctor(doctor);

                TempData["Message"] = result;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(doctor);
            }
        }

        // GET: Doctor/Edit/5
        [DoctorAuthorize]
        public ActionResult Edit(int id)
        {
            Doctor doctor = repo.GetSingleDoctorById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Edit
        [HttpPost]
        [DoctorAuthorize]
        public ActionResult Edit(Doctor doctor)
        {
            try
            {
                string result = repo.UpdateDoctor(doctor);

                TempData["Message"] = result;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(doctor);
            }
        }

        // GET: Doctor/Delete/5
        [DoctorAuthorize]
        public ActionResult Delete(int id)
        {
            Doctor doctor = repo.GetSingleDoctorById(id);

            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [DoctorAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                string result = repo.DeleteSingleDoctorById(id);

                TempData["Message"] = result;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: Doctor/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Doctor/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Doctor doctor)
        {
            try
            {
                string result = repo.RegisterDoctor(doctor);
                if (result == "Success")
                {
                    TempData["Message"] = "Doctor registered successfully! Please log in.";
                    return RedirectToAction("Login");
                }

                ViewBag.Error = result;
                return View(doctor);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(doctor);
            }
        }

        // GET: Doctor/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Doctor/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.Error = "Please enter both Email and Password.";
                    return View();
                }

                var doctor = repo.ValidateDoctor(email, password);
                if (doctor != null)
                {
                    Session["DoctorId"] = doctor.DoctorId;
                    Session["DoctorName"] = "Dr. " + doctor.FirstName + " " + doctor.LastName;
                    Session["DoctorEmail"] = doctor.Email;

                    return RedirectToAction("Index", "DocPatient");
                }

                ViewBag.Error = "Invalid Email or Password.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: Doctor/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            TempData["Message"] = "You have been logged out.";
            return RedirectToAction("Login");
        }
    }
}
