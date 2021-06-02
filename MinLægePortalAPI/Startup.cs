using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("MinLægePortalAPI.config", typeof(MinLægePortalAPI.Startup))]

namespace MinLægePortalAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
