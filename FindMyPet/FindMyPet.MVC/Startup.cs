using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FindMyPet.MVC.Startup))]
namespace FindMyPet.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
