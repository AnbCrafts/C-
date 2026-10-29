using System;
using System.Web.Mvc;
using MVCPracticeProject.Models;

namespace MVCPracticeProject.Controllers
{
    public class PatientController : Controller
    {
        private PatientRepository repo = new PatientRepository();

        // GET: Patient
        public ActionResult Index()
        {
            var patients = repo.GetPatients();
            return View(patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {
            var patient = repo.GetPatientById(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            try
            {
                string result = repo.InsertPatient(patient);
                TempData["Message"] = result;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(patient);
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            var patient = repo.GetPatientById(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            try
            {
                string result = repo.UpdatePatient(patient);
                TempData["Message"] = result;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(patient);
            }
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            var patient = repo.GetPatientById(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                string result = repo.DeletePatient(id);
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
