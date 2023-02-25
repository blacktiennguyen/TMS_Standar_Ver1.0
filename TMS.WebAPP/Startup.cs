using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TMS.WebAPP.Startup))]
namespace TMS.WebAPP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
