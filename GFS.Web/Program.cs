using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace GFS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseSetting(WebHostDefaults.DetailedErrorsKey,"true")
                .UseIISIntegration()
                .UseUrls("http://*:55339")
                .UseStartup<Startup>()
                .Build();
        }
    }
}