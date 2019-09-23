using System.IO;
using System.Reflection;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation.AspNetCore;
using GFS.Core;
using GFS.Data.EFCore;
using GFS.Data.Model.Entities;
using GFS.Data.Model.Utility;
using GFS.Domain.Core;
using GFS.Domain.Impl;
using GFS.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag;
using NSwag.AspNetCore;
using Serilog;

namespace GFS.Web
{
    public class Startup
    {
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();
            Dictionary = new ConfigurationBuilder()
                .AddJsonFile("dictionary.json", false)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        private IConfiguration Configuration { get; }
        private IConfiguration Dictionary { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<GfsDbContext>(options => options.UseNpgsql(connectionString));


            var builder = services.AddIdentityCore<User>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    corsBuilder => corsBuilder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build());
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<GfsDbContext>().AddDefaultTokenProviders();

            services.Configure<JwtAuthentication>(Configuration.GetSection("JwtAuthentication"));
            services
                .AddMvc(x => x.Filters.AddService<ExceptionFilter>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddAuthorization(options => options.AddPolicy("Admin", policy => policy.RequireRole("Admin")));
            services.AddAuthorization(
                options => options.AddPolicy("Standard", policy => policy.RequireRole("Standard")));
            services.AddAuthorization(options =>
                options.AddPolicy("User", policy => policy.RequireAuthenticatedUser()));
            services.Configure<EmailConfig>(Configuration.GetSection("Email"));
            services.Configure<Dictionary>(Dictionary.GetSection("pl"));
            services.AddScoped<ExceptionFilter>();
            var context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


            services.AddScoped<IDrawerService, DrawerService>();
            services.AddScoped<IFurnitureService, FurnitureService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProducerService, ProducerService>();
            services.AddScoped<IFabricService, FabricService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IOrderPdfService, OrderPdfService>();

            services.AddAutoMapper();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //app.UseExceptionHandler(
            //    builder =>
            //    {
            //        builder.Run(
            //            async context =>
            //            {
            //                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            //                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            //                var error = context.Features.Get<IExceptionHandlerFeature>();
            //                if (error != null)
            //                {
            //                    context.Response.AddApplicationError(error.Error.Message);
            //                    await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
            //                }
            //            });
            //    });
            app.UseCors("CorsPolicy");
            app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "GFS API";
                    document.Info.Description = "Generating furniture formatters web system";
                    document.Info.Contact = new SwaggerContact
                    {
                        Name = "MarAdi Developers"
                    };
                };
            });
            loggerFactory.AddSerilog();
            //  app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.HasValue && context.Request.Path.Value != "/" &&
                    !context.Request.Path.Value.Contains("/api"))
                {
                    context.Response.ContentType = "text/html";

                    await context.Response.SendFileAsync(
                        env.ContentRootFileProvider.GetFileInfo("wwwroot/index.html")
                    );

                    return;
                }

                await next();
            });
            app.UseMvc();
        }
    }
}