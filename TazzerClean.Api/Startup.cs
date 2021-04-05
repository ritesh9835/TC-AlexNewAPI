using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DataAccess.Database;
using DataContracts.Common;
using DataContracts.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Serilog;
using ServiceContracts.Admin;
using ServiceContracts.Auth;
using ServiceContracts.CategoryManager;
using ServiceContracts.Common;
using ServiceContracts.ExtraServices;
using ServiceContracts.Notification;
using ServiceContracts.Order;
using ServiceContracts.Pages;
using ServiceContracts.PromoCode;
using ServiceContracts.Questionnaire;
using ServiceContracts.ServiceManager;
using ServiceContracts.Session;
using ServiceContracts.Subscriptions;
using ServiceContracts.UnitManager;
using ServiceContracts.UserManager;
using TazzerClean.Util;

namespace TazzerClean.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TazzerCleanUK API",
                    Description = "TazzerCleanUK API Official",
                    TermsOfService = new Uri("https://tazzerclean.co.uk/terms")
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });

                
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //swagger.IncludeXmlComments(xmlPath);

            });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("user", p =>
                {
                    p.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "user");
                });

                opts.AddPolicy(Policies.Admin.ToString(), policyUser => policyUser.RequireClaim(ClaimTypes.Role, RoleType.Admin.ToString()));

                opts.AddPolicy(Policies.Professional.ToString(), policyUser => policyUser.RequireClaim(ClaimTypes.Role, RoleType.Professional.ToString()));

                opts.AddPolicy(Policies.Customer.ToString(), policyUser => policyUser.RequireClaim(ClaimTypes.Role, RoleType.Customer.ToString()));

                opts.AddPolicy(Policies.CustomerAndHigher.ToString(),
                    policyUser => policyUser.RequireClaim(ClaimTypes.Role,
                        RoleType.Admin.ToString(),
                        RoleType.Professional.ToString(),
                        RoleType.Customer.ToString()));

                opts.AddPolicy(Policies.ProfessionalAndHigher.ToString(),
                    policyUser => policyUser.RequireClaim(ClaimTypes.Role,
                        RoleType.Admin.ToString(),
                        RoleType.Professional.ToString()));
            });

            services.AddMemoryCache();


            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            //    .AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = false,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = Configuration["Jwt:Issuer"],
            //        ValidAudience = Configuration["Jwt:Issuer"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            //    };
            //});
                .AddJwtBearer(c =>
                {
                    c.RequireHttpsMetadata = false;
                    c.SaveToken = true;
                    c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    c.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs/chat")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddCors();

            services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest).AddControllersAsServices();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            LogManager.Configuration.Variables["ConnectionStrings"] = Configuration.GetConnectionString("TazzerCleanCs");

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TazzerClean UK Api");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseExceptionHandler(config =>
            {
                config.Run(context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        Log.Error(error.Error, "Application error in Api.");
                    }

                    return Task.CompletedTask;
                });
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DataAccessRepository());
            builder.RegisterType<AuthManager>().As<IAuthManager>().SingleInstance();
            builder.RegisterType<CategoryManager>().As<ICategoryManager>().SingleInstance();
            builder.RegisterType<UnitManager>().As<IUnitManager>().SingleInstance();
            builder.RegisterType<ServiceManager>().As<IServiceManager>().SingleInstance();
            builder.RegisterType<CommonManager>().As<ICommonManager>().SingleInstance();
            builder.RegisterType<UserManager>().As<IUserManager>().SingleInstance();
            builder.RegisterType<PasswordHasher>().As<IPasswordHasher>().SingleInstance();
            builder.RegisterType<EmailService>().As<IEmailService>().SingleInstance();
            builder.RegisterType<AdminManager>().As<IAdminManager>().SingleInstance();
            builder.RegisterType<SessionManager>().As<ISessionManager>().SingleInstance();
            builder.RegisterType<PagesService>().As<IPagesService>().SingleInstance();
            builder.RegisterType<PromoCodeService>().As<IPromoCodeService>().SingleInstance();
            builder.RegisterType<ExtraServiceManager>().As<IExtraServiceManager>().SingleInstance();
            builder.RegisterType<SubscriptionManager>().As<ISubscriptionManager>().SingleInstance();
            builder.RegisterType<OrderService>().As<IOrderService>().SingleInstance();
            builder.RegisterType<QuestionnaireManager>().As<IQuestionnaireManager>().SingleInstance();
        }
    }
}
