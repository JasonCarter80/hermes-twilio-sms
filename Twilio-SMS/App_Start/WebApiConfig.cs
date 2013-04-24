using System.Web.Http;

namespace Twilio_SMS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{from}/{to}/{message}"
                );

            config.Routes.MapHttpRoute(
                name: "GetApi",
                routeTemplate: "api/{controller}/getAll/{from}"
                );

            config.Routes.MapHttpRoute(
                name: "GetApiIncoming",
                routeTemplate: "api/{controller}",
                defaults: new { controller = "Incoming"}
                );
        }
    }
}