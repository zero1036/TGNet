using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TG.Example.Host.Startup))]
namespace TG.Example.Host
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
