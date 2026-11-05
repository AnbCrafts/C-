using System.Web.Http;
using WebActivatorEx;
using WEB_API_Practice_Project_1;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WEB_API_Practice_Project_1
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "WEB_API_Practice_Project_1 - Doctors Management API");
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
