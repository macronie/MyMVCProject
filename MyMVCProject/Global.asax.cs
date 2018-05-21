using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Gnostice.StarDocsSDK;

namespace MyMVCProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static StarDocs starDocs;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Set up start docs
            starDocs = new StarDocs(
                new ConnectionInfo(new Uri("https://api.gnostice.com/stardocs/v1"),
                "ff706dd13e0d4c5b95468e9979277ebf", "fcfeb6e692a3488bbb8b960d5ffe62d8"));

            // Authenticate
            starDocs.Auth.loginApp();
        }
    }
}
