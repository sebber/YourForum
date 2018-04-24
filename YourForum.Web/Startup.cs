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
using YourForum.Core.Data;
using YourForum.Core.Infrastructure;
using YourForum.Core.Infrastructure.Tags;
using YourForum.Core.Models;

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
                    .AddDbContext<YourForumContext>(options => options.UseNpgsql(connectionString));

            services.AddAutoMapper();

            services.AddMediatR();

            services.AddHtmlTags(new TagConventions());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<Account, IdentityRole>()
                .AddEntityFrameworkStores<YourForumContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = false;
            });

            services.AddMvc(opt =>
                    {
                        opt.Filters.Add(typeof(DbContextTransactionFilter));
                        opt.Filters.Add(typeof(ValidatorActionFilter));
                        opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider());
                        opt.Filters.Add(typeof(ForumTenantFilter));
                    })
                    .AddFeatureFolders()
                    .AddAreaFeatureFolders()
                    .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{forumId:int}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
