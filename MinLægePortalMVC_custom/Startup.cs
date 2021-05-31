using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MinLægePortalMVC_custom.Startup))]
namespace MinLægePortalMVC_custom
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
