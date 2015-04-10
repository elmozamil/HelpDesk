using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

//[assembly: OwinStartup(typeof(HelpDesk.Startup))]
[assembly: OwinStartupAttribute(typeof(HelpDesk.Startup))]

namespace HelpDesk
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
