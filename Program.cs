using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;

namespace DBE.ENERGY.Web
{
    public class Program
    {
        private static IConfiguration configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();

        public static void Main(string[] args)
        {
            try
            {
                BuildWebHost(args).Run();
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .WriteTo.MSSqlServer(configuration.GetConnectionString("DefaultConnection"), "Log")
                    .CreateLogger();

                /* Serilog debugging itself - debug purpose only*/
                Serilog.Debugging.SelfLog.Enable(msg =>
                {
                    Debug.Print(msg);
                    //Debugger.Break(); //optional 
                });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host temrinated");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                //.UseKestrel(opt =>
                //{

                //})
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    if (hostingContext.HostingEnvironment.EnvironmentName.Contains("Development"))
                        config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: false);
                    if (hostingContext.HostingEnvironment.EnvironmentName.Contains("Test"))
                        config.AddJsonFile("appsettings.Test.json", optional: false, reloadOnChange: false);
                    if (hostingContext.HostingEnvironment.EnvironmentName.Contains("Production"))
                        config.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: false);
                })
                //.UseEnvironment("prod") // uncomment for testing purposes
                .Build();
    }
}