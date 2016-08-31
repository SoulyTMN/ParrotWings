using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(ParrotWIngs.Startup))]

namespace ParrotWIngs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            app.Map("/api", inner =>
             {
                 inner.UseWebApi(config);
             });
        }
    }
}
