using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(SignalRSelfHost.Program.Startup))]
namespace SignalRSelfHost
{
    class Program
    {
        static IDisposable SignalR;

        static void Main(string[] args)
        {
            string url = "http://*:8088";
            SignalR = WebApp.Start(url);

            Console.ReadKey();
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);

                /*  CAMEL CASE & JSON DATE FORMATTING
                 use SignalRContractResolver from
                https://stackoverflow.com/questions/30005575/signalr-use-camel-case

                var settings = new JsonSerializerSettings()
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };

                settings.ContractResolver = new SignalRContractResolver();
                var serializer = JsonSerializer.Create(settings);

               GlobalHost.DependencyResolver.Register(typeof(JsonSerializer),  () => serializer);                

                 */

                app.MapSignalR();
            }
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