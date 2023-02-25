using System;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TMS.Core;

namespace TMS.WebAPP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<TMSContext>(null);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                throw new HttpException(404, string.Empty);
            }
            else if (httpException != null && httpException.GetHttpCode() == 500)
            {
                throw new HttpException(500, string.Empty);
            }
        }
    }
}