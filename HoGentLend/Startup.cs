using HoGentLend.Models.DAL;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HoGentLend.Startup))]
namespace HoGentLend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
