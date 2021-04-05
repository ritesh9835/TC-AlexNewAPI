using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace TazzerClean.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())
              .UseNLog()
              .ConfigureWebHostDefaults(webHostBuilder =>
              {
                  webHostBuilder
                      .UseContentRoot(Directory.GetCurrentDirectory())
                      .UseIIS()
                      .UseStartup<Startup>();
              })
              .Build();
            host.Run();
        }
    }
}
