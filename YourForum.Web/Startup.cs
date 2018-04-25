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
using YourForum.Core.Services;

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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPasswordService, DefaultPasswordService>();

            services.AddMvc(opt =>
                    {
                        opt.Filters.Add(typeof(DbContextTransactionFilter));
                        opt.Filters.Add(typeof(ValidatorActionFilter));
                        opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider());
                        opt.Filters.Add(typeof(ForumTenantFilter));
                    })
                    .AddFeatureFolders()
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
