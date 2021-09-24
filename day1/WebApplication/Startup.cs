using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WebApplication.EfModel;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApplication", Version = "v1"});
            });

            services.AddGrpc(options =>
            {
                
                
            });
            services.AddSingleton<IGreeterGenerator, GreeterGenerator>();

            services.AddOptions<MySettings>().Bind(Configuration.GetSection("MySetting"));
            services.AddSingleton<IValidateOptions<MySettings>, MySettings>();
            services.AddHttpContextAccessor();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Basic";
                    options.DefaultChallengeScheme = "Basic";
                })
                .AddBasic("Basic", options =>
                {
                    options.Realm = "myWebApp";
                    options.Events = new BasicAuthenticationEvents()
                    {
                        OnValidateCredentials = context =>
                        {
                            if (context.Username == context.Password)
                            {
                                var claims = new[]
                                {
                                    new Claim(ClaimTypes.NameIdentifier, context.Username, ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer),
                                    new Claim(ClaimTypes.Name, context.Username, ClaimValueTypes.String,
                                        context.Options.ClaimsIssuer)
                                };

                                context.Principal =
                                    new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                                context.Success();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                // Used for all places where no Authorize Attribute is used
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                // Used for all places where a Authorize Attribute is used without policy
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                
                // Used for all places where a Authorize Attribute is used with the policy "username_longer_5" 
                options.AddPolicy("username_longer_5", 
                    builder => 
                        builder
                            .RequireAuthenticatedUser()
                            .RequireAssertion(context => Task.FromResult(context.User.Identity.Name.Length > 5)) 
                );
            });

            services.AddDbContext<MyContext>(builder =>
            {
                builder.UseSqlite("Data Source=mydb.db");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication v1"));
            }

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/test.txt", async context => await context.Response.WriteAsync("Hello world"));
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcGreeter>();
            });
        }
    }

    public class MySettings: IValidateOptions<MySettings>
    {
        public int MyIntKey { get; set; }
        public string MyStringKey { get; set; }
        public bool MyBoolKey { get; set; }
        
        public ValidateOptionsResult Validate(string name, MySettings options)
        {
            if (options.MyIntKey < 1)
            {
                return ValidateOptionsResult.Fail("Must set Int Key");
            }
            
            return ValidateOptionsResult.Success;
        }
    }
}