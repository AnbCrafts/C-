using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_RBAC_Prctice.Models;

namespace MVC_RBAC_Prctice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // Accessible by SUPERADMIN and ADMIN
        [AuthorizeRole(Role.SUPERADMIN, Role.ADMIN)]
        public ActionResult Index()
        {
            return View();
        }

        // Accessible only by SUPERADMIN
        [AuthorizeRole(Role.SUPERADMIN)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page. (SUPERADMIN ONLY)";
            return View();
        }

        // Accessible only by ADMIN
        [AuthorizeRole(Role.ADMIN)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page. (ADMIN ONLY)";
            return View();
        }

        // Accessible only by USER
        [AuthorizeRole(Role.USER)]
        public ActionResult UserPage()
        {
            ViewBag.Message = "User Page (USER ONLY)";
            return View();
        }
    }
}
