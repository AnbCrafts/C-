using System;
using System.Linq;
using System.Web.Mvc;

namespace MVC_RBAC_Prctice.Models
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
            {
                throw new ArgumentException("roles");
            }

            this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // 1. If not authenticated at all -> redirect to Login
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            // 2. If authenticated, but doesn't have the required Role -> redirect to Access Denied
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("~/Account/AccessDenied");
                return;
            }
        }
    }
}
