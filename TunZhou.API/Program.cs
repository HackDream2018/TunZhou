using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TunZhou.API
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHost().Run();

        private static IWebHost CreateWebHost() => CreateWebHostBuilder().Build();

        public static IWebHostBuilder CreateWebHostBuilder() =>
            new WebHostBuilder()
             .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, cfg) =>
                {
                    var env = context.HostingEnvironment;
                    // appsettings.json中放置各个环境下的公共配置
                    cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    // appsettings.EnvironmentName.json中放置各个环境下的配置
                    cfg.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
                })
#if RELEASE
                .UseIISIntegration()
#endif
                .UseKestrel()
                .UseStartup<Startup>();
    }
}
