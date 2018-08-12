using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace FindMyPet.MyServiceStack
{
    public class Global : System.Web.HttpApplication
    {
        private const string licenseKeyText = "6217-e1JlZjo2MjE3LE5hbWU6TWlndWVsIFNvcmlhbm8sVHlwZTpJbmRpZSxIYXNoOmROYWR4TVFDdmdwNVpuOWNjdGgwZldoQldnNnR0aWphTkZ3Mm5FS0dPQTZYYmFGSmRtdVF5UHZNczZnVzkwUmlUYzQrVEFkZlV5WkNTWjJkWTVqOGlPWThELzVUYXhOc2pvVDltSGxWeEJLdlNqeHBaaHAyZXJENVBPVXEwa3BkYmFMdnNMeWZ4NTdVSzNiL25IMG8yenNORXQ0citWdUFkVTNmY214aXhuaz0sRXhwaXJ5OjIwMTktMDYtMTN9";

        protected void Application_Start(object sender, EventArgs e)
        {
            Licensing.RegisterLicense(licenseKeyText);
            new FindMyPetHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}