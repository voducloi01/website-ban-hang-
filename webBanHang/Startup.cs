using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webBanHang.Startup))]
namespace webBanHang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
