using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WebApplication.EfModel;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var dbcontext = scope.ServiceProvider.GetRequiredService<MyContext>();
                dbcontext.Database.Migrate();

                /*var p = new Person() {Id = Guid.NewGuid(), Name = "Wilhelms", Vorname = "Boris"};
                dbcontext.Persons.Add(p);
                dbcontext.SaveChanges();*/
/*
                dbcontext.Persons.Remove(new Person()
                    {Id = new Guid("3672918E-4A36-4D51-ACF5-AFAD7E8B45DD")});*/

                var p = dbcontext.Persons.FromSqlRaw("GetPersons").ToList();
                
                dbcontext.SaveChanges();
                /*var p = dbcontext.Persons.AsNoTracking().FirstOrDefault();
                dbcontext.Persons.Remove(p);*/

                /*dbcontext.SaveChanges();*/

                /*
                var personse = dbcontext.Persons
                    .Where(p => p.Vorname == "Boris")
                    .OrderBy(p => p.Id)
                    .Select(p => new { p.Name })
                    .ToList();*/
            }

            // DO SOME INIT
            //host.Run();
            // DO SOME CLEANUP
        }

        public static bool IsBoris(Person p)
        {
            return p.Vorname == "Boris";
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                /*
                .ConfigureLogging((Action<HostBuilderContext, ILoggingBuilder>) ((hostingContext, logging) =>
                    {
                        bool flag = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                        if (flag)
                            logging.AddFilter<EventLogLoggerProvider>((Func<LogLevel, bool>) (level => level >= LogLevel.Warning));
                        logging.AddConfiguration((IConfiguration) hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.AddEventSourceLogger();
                        if (flag)
                            logging.AddEventLog();
                        logging.Configure((Action<LoggerFactoryOptions>) (options => options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId | ActivityTrackingOptions.TraceId | ActivityTrackingOptions.ParentId));
                    })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    IHostEnvironment hostingEnvironment = hostingContext.HostingEnvironment;
                    config
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true);
                    
                    if (hostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(hostingEnvironment.ApplicationName))
                    {
                        var assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                        if (assembly != (Assembly) null)
                        {
                            config.AddUserSecrets(assembly, true);
                        }
                    }
                    config.AddEnvironmentVariables();
                    if (args == null)
                        return;
                    config.AddCommandLine(args);

                })*/
                .ConfigureWebHostDefaults(webBuilder => 
                {  
                    webBuilder.UseStartup<Startup>();
                });
    }
}