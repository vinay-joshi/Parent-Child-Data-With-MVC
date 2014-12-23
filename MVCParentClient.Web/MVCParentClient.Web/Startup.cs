using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCParentClient.Web.Startup))]
namespace MVCParentClient.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
