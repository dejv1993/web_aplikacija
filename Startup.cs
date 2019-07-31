using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Builder;
[assembly: OwinStartup(typeof(BriscollaGame.Startup))]

namespace BriscollaGame
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          
            ConfigureAuth(app);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Login")
            }
                );
        }
    }
}