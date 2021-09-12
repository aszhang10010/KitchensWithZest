using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KitchensWithZest.Startup))]
namespace KitchensWithZest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
