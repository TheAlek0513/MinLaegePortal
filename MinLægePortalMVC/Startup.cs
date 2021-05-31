using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MinLægePortalMVC.Startup))]
namespace MinLægePortalMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
