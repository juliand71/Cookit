using Cookit.WebApp.Authorization;
using Cookit.WebApp.Data;
using Cookit.WebApp.Models;
using Cookit.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp
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
            services.AddRazorPages();

            services.AddDbContext<CookitContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default")));

            // adding identity and role manager services
            services.AddIdentity<CookitUser, IdentityRole>()
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<CookitContext>();

            // require Authorization on all pages by default
            // this means we will need to make sure any pages where we don't need authorization
            // have allow anonymous attribute
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CUDPolicy", policy =>
                    policy.Requirements.Add(new OwnerRequirement()));
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
            services.AddSingleton<IAuthorizationHandler, RecipeAuthorizationHandler>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            // add our custom service for dealing with the image files
            services.AddTransient<ImageFileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CookitContext cookitContext)
        {
            cookitContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(); // wwwroot folder

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider("D:\\CookitRecipeImages\\"),
                RequestPath = "/recipe-images"
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
