using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoneyPortal.Startup))]
namespace MoneyPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
