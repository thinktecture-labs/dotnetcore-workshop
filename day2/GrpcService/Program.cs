using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace GrpcService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((Action<HostBuilderContext, IConfigurationBuilder>) ((hostingContext, config) =>
                {
                    IHostEnvironment hostingEnvironment = hostingContext.HostingEnvironment;
                    bool reloadOnChange = hostingContext.Configuration.GetValue<bool>("hostBuilder:reloadConfigOnChange", true);
                    config
                        .AddJsonFile("appsettings.json", true, reloadOnChange)
                        .AddJsonFile("appsettings." + hostingEnvironment.EnvironmentName + ".json", true, reloadOnChange);
                    if (hostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(hostingEnvironment.ApplicationName))
                    {
                        Assembly assembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                        if (assembly != (Assembly) null)
                            config.AddUserSecrets(assembly, true);
                    }
                    config.AddEnvironmentVariables();
                    if (args == null)
                        return;
                    config.AddCommandLine(args);
                }))
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
                }))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}