using System;

namespace WebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            SignalRServer.MyServer.SetupRouting();
        }

        protected void Session_Start(object sender, EventArgs e){}
        protected void Application_BeginRequest(object sender, EventArgs e){}
        protected void Application_AuthenticateRequest(object sender, EventArgs e){}
        protected void Application_Error(object sender, EventArgs e){}
        protected void Session_End(object sender, EventArgs e){}
        protected void Application_End(object sender, EventArgs e){}
    }
}