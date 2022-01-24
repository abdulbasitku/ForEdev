using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace WebAPI_DataLoader
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var response = context.Response;

            string method = context.Request.HttpMethod;
            if (context.Request.HttpMethod == "OPTIONS")
            {
                response.AddHeader("Access-Control-Allow-Methods", "*");
                response.End();
            }
        }
    }
}
