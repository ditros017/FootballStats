using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FootballStats.Web.Startup))]
namespace FootballStats.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
