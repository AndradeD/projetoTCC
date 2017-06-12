using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EntradaDados.Startup))]
namespace EntradaDados
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
