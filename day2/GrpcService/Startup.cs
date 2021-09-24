using System;
using System.IO;
using System.Reflection;
using System.Threading;
using GrpcService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GrpcService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            // services
            //     .AddAuthentication()
            //     .AddJwtBearer(options =>
            //     {
            //         options.Authority = "https://login.thinktecture.com/auth/realms/tt-internal";
            //         options.Audience = "grpc-service";
            //     });

            services.AddControllers();
            services.AddScoped<IStrategy>(provider =>
            {
                Thread.Sleep(1000);
                return new TestStrategy();
            });

            services.AddDbContext<AkteDbContext>(options =>
            {
                options.UseSqlite("Data Source=/home/boris/c/wiesbaden/dotnetcore/GrpcService/mydb.db");
            });
            
            // services.AddAuthorization(options =>
            // {
            //     // Used for authorized attributes without further roles or policy
            //     options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //     
            //     // Used for all actions that DON'T have an authorized attribute
            //     options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //     
            //     // Used for all actions that have an authorized attribute with the policy "username_longer_5" 
            //     options.AddPolicy("username_longer_5", builder =>
            //     {
            //         builder
            //             //.RequireAuthenticatedUser()
            //             .RequireAssertion(context => context.User.Identity.Name.Length > 5);
            //     });
            // });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // ASPNETCORE_ENVIRONMENT
        // Development
        // Staging
        // Production
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }
            
            app.UseRouting();
            
            // app.UseAuthentication();
            // app.UseAuthorization();            
                
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
            
        }
    }
}