using Cookit.Authorization;
using Cookit.Data;
using Cookit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit
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
            //int MyMaxModelBindingCollectionSize = 100;
            //Int32.TryParse(Configuration["MyMaxModelBindingCollectionSize"],
            //                           out MyMaxModelBindingCollectionSize);

            //services.Configure<MvcOptions>(options =>
            //       options.MaxModelBindingCollectionSize = MyMaxModelBindingCollectionSize);

            services.AddRazorPages();
            // for rating component
            services.AddServerSideBlazor();

            services.AddDbContext<CookitContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("Default")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<CookitUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CookitContext>();

            

            // require Authorization on all pages by default
            // this means we will need to make sure any pages where we don't need authorization
            // have allow anonymous attribute
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CUDPolicy", policy =>
                    policy.Requirements.Add(new OwnerRequirement()));
                // found out that the below line causes issues with Blazor
                //options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
            services.AddSingleton<IAuthorizationHandler, RecipeAuthorizationHandler>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            // add our custom service for dealing with the image files
            services.AddTransient<ImageHandler>();

            // add our email sender
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                endpoints.MapBlazorHub();
            });
        }
    }
}
