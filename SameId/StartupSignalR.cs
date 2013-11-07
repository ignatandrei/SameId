using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SameId.StartupSignalR))]
namespace SameId
{
    public class StartupSignalR
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888\
            app.MapSignalR();
        }
    }
}
