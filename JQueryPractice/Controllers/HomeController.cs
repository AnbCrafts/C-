using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JQueryPractice.Models;

namespace JQueryPractice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/JSGrid
        public ActionResult JSGrid()
        {
            return View();
        }

        // GET: Home/RedirectDemo
        public ActionResult RedirectDemo()
        {
            return View();
        }

        #region 1. Redirect Action Results (Slides 2 - 4)

        // Slide 2: RedirectResult - Redirect to an external URL or specific URL
        public RedirectResult TestRedirect()
        {
            return Redirect("https://mccit.net");
        }

        // Slide 2 & 3: RedirectToRouteResult - Redirect to an action using route parameters
        public RedirectToRouteResult TestRedirectToRoute()
        {
            return RedirectToRoute(new { controller = "Home", action = "About" });
        }

        // Slide 3: RedirectToAction - Redirect to a specific Controller and Action
        public ActionResult TestRedirectToAction()
        {
            return RedirectToAction("Contact", "Home");
        }

        // Slide 3: RedirectToAction - Controller name is optional if targeting the same controller
        public ActionResult TestRedirectToActionSameController()
        {
            return RedirectToAction("About");
        }

        #endregion

        #region 2. Status Results (Slides 26 - 29)

        // Slide 26: HttpStatusCodeResult - 401 Unauthorized with custom status description
        public HttpStatusCodeResult UnauthorizedStatusCode()
        {
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "You are not authorized to access this controller action.");
        }

        // Slide 26: HttpStatusCodeResult - 502 Bad Gateway with custom message
        public HttpStatusCodeResult BadGateway()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadGateway, "I have no idea what this error means.");
        }

        // Slide 28: HttpUnauthorizedResult - Specialized helper for 401 Unauthorized
        public HttpStatusCodeResult UnauthorizedResult()
        {
            return new HttpUnauthorizedResult("You are not authorized to access this controller action.");
        }

        // Slide 29: HttpNotFoundResult - Helper method HttpNotFound for 404 Not Found
        public HttpNotFoundResult NotFound()
        {
            return HttpNotFound("We didn't find that action, sorry!");
        }

        #endregion

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult AjaxMethod()
        {
            AssignmentDBEntities entities = new AssignmentDBEntities();
            List<Doctor> doctors = (from doctor in entities.Doctors
                                    select doctor).ToList();
            return Json(doctors, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
