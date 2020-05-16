using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Threading.Tasks;

//[assembly: OwinStartup(typeof(SignalRSelfHost.Program.Startup))]
namespace SignalRSelfHost
{
    class Program
    {
        static IDisposable SignalR;

        static void Main(string[] args)
        {
            string url = "http://*:8088";
            //SignalR = WebApp.Start(url);
            SignalR = WebApp.Start<Startup>(url);

            Console.ReadKey();
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.Map("/signalr", map =>
                {
                    map.UseCors(CorsOptions.AllowAll);
                    var hubConfiguration = new HubConfiguration
                    {
                        EnableDetailedErrors = true,
                        EnableJSONP = true
                    };

                    map.RunSignalR(hubConfiguration);
                });
            }

            //app.UseCors(CorsOptions.AllowAll);
            //app.MapSignalR();
        }

        [HubName("MyHub")]
        public class MyHub : Hub
        {
            public override Task OnConnected()
            {
                Clients.Caller.identifyMessage();
                return base.OnConnected();
            }

            public void Identify(string name)
            {
                Clients.All.broadcastMessage("SYSTEM", $"Welcome - {name}");
            }

            public void Send(string name, string message)
            {
                Console.WriteLine($"{name}: {message}");
                Clients.All.broadcastMessage(name, message);
            }
        }
    }
}