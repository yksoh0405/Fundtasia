using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;

namespace Fundtasia
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var option = new CookieAuthenticationOptions
            {
                AuthenticationType = "AUTH", //Id of authentication
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout")
            };

            app.UseCookieAuthentication(option);
        }
    }
}
