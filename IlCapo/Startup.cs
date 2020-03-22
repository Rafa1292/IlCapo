using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IlCapo.Startup))]
namespace IlCapo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
