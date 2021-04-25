using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GmailFbOAuth.Startup))]
namespace GmailFbOAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
