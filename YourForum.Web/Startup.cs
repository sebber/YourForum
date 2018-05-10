using System;
using AutoMapper;
using FluentValidation.AspNetCore;
using HtmlTags;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YourForum.Web;
using YourForum.Core.Data;
using YourForum.Core.Infrastructure;
using YourForum.Core.Infrastructure.Tags;
using YourForum.Core.Models;
using YourForum.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace YourForum.Web
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
            var connectionString = Configuration.GetConnectionString("YourForumContext");
            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<YourForumContext>(options =>
                    {
                        options.UseNpgsql(connectionString, b => b.MigrationsAssembly("YourForum.Core"));
                    });

            services.AddAutoMapper();

            services.AddMediatR();

            services.AddHtmlTags(new TagConventions());

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPasswordService, DefaultPasswordService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<TenantProvider>();

            services.Configure<RequestLocalizationOptions>(opts =>
                {
                    var supportedCultures = new[]
                    {
                        new CultureInfo("en"),
                        new CultureInfo("sv"),
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });

            services.AddMvc(opt =>
                    {
                        opt.Filters.Add(typeof(DbContextTransactionFilter));
                        opt.Filters.Add(typeof(ValidatorActionFilter));
                        opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider());
                        opt.Filters.Add(typeof(ForumTenantFilter));
                    })
                    .AddFeatureFolders()
                    .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); })
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization(options => {
                        options.DataAnnotationLocalizerProvider = (type, factory) =>
                            factory.Create(typeof(Translation));
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                Mapper.AssertConfigurationIsValid();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseForumTenant();

            app.UseAuthentication();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "signin",
                    template: "{forumId:int}/signin",
                    defaults: new { controller = "Authentication", action = "SignIn" });

                routes.MapRoute(
                    name: "signout",
                    template: "{forumId:int}/signout",
                    defaults: new { controller = "Authentication", action = "SignOut" });

                routes.MapRoute(
                    name: "default",
                    template: "{forumId:int}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
