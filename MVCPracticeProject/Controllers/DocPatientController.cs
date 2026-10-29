using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCPracticeProject.Models;
using MVCPracticeProject.ViewModels;

namespace MVCPracticeProject.Controllers
{                
    public class DocPatientController : Controller
    {
        private DoctorRepository doctorRepo = new DoctorRepository();
        private PatientRepository patientRepo = new PatientRepository();

        // GET: DocPatient (Displays list of doctors and patients together)
        public ActionResult Index()
        {
            var viewModel = new DoctorPatientViewModel
            {
                Doctors = doctorRepo.GetAllDoctors(),
                Patients = patientRepo.GetPatients()
            };

            return View(viewModel);
        }

        // GET: DocPatient/Details/5 (Displays a single doctor along with their assigned patients)
        public ActionResult Details(int id)
        {
            var doctor = doctorRepo.GetSingleDoctorById(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            var patients = patientRepo.GetPatients().Where(p => p.DoctorId == id).ToList();

            var viewModel = new DoctorPatientViewModel
            {
                Doctor = doctor,
                Patients = patients
            };

            return View(viewModel);
        }

        // GET: DocPatient/Edit/5 (Edit Doctor and view their assigned patients)
        public ActionResult Edit(int id)
        {
            var doctor = doctorRepo.GetSingleDoctorById(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            var patients = patientRepo.GetPatients().Where(p => p.DoctorId == id).ToList();

            var viewModel = new DoctorPatientViewModel
            {
                Doctor = doctor,
                Patients = patients
            };

            return View(viewModel);
        }

        // POST: DocPatient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorPatientViewModel viewModel)
        {
            try
            {
                if (viewModel?.Doctor == null)
                {
                    return HttpNotFound();
                }

                string result = doctorRepo.UpdateDoctor(viewModel.Doctor);
                TempData["Message"] = result;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                if (viewModel?.Doctor != null)
                {
                    viewModel.Patients = patientRepo.GetPatients()
                        .Where(p => p.DoctorId == viewModel.Doctor.DoctorId)
                        .ToList();
                }
                return View(viewModel);
            }
        }

        // GET: DocPatient/Delete/5 (Delete Doctor confirmation + assigned patients warning)
        public ActionResult Delete(int id)
        {
            var doctor = doctorRepo.GetSingleDoctorById(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            var patients = patientRepo.GetPatients().Where(p => p.DoctorId == id).ToList();

            var viewModel = new DoctorPatientViewModel
            {
                Doctor = doctor,
                Patients = patients
            };

            return View(viewModel);
        }

        // POST: DocPatient/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                string result = doctorRepo.DeleteSingleDoctorById(id);
                TempData["Message"] = result;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
