using System.Web;
using System.Web.Mvc;

namespace WEB_API_Practice_Project_1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
