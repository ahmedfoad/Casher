using Casher.Models.Administration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq; using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Casher
{
    public class MvcApplication : System.Web.HttpApplication
    {
        
        
        string[] date = DateTime.Now.ToString(CultureInfo.GetCultureInfo("ar-SA")).Substring(0, 8).Split('/');
        private string[] time = DateTime.Now.TimeOfDay.ToString().Split(':');
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
      
    }
}
