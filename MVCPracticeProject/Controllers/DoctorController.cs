using System;
using System.Web.Mvc;
using MVCPracticeProject.Models;

namespace MVCPracticeProject.Controllers
{
    public class DoctorController : Controller
    {
        private DoctorRepository repo = new DoctorRepository();

        // GET: Doctor
            public ActionResult Index(){
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
    }
}