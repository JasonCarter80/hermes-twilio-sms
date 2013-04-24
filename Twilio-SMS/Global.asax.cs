using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Twilio_SMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var jsonMediaTypeFormatters = GlobalConfiguration.Configuration.Formatters
                .Where(x => x.SupportedMediaTypes
                .Any(y => y.MediaType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            // Remove formatters from global config.
            foreach (var formatter in jsonMediaTypeFormatters)
            {
                GlobalConfiguration.Configuration.Formatters.Remove(formatter);
            }
        }
    }
}