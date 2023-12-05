using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Aptech_Final_E_project.Startup))]
namespace Aptech_Final_E_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
