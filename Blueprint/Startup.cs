using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Blueprint.Startup))]
namespace Blueprint
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
