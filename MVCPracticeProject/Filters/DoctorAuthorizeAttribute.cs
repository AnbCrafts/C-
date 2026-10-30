using System.Web.Mvc;
using System.Web.Routing;

namespace MVCPracticeProject.Filters
{
    public class DoctorAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

        
            if (session == null || session["DoctorId"] == null)
            {
                
                filterContext.Controller.TempData["Message"] = "Please log in as a doctor to perform this action.";

                
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Doctor" },
                        { "action", "Login" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
