using System;
using Cookit.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Cookit.Areas.Identity.IdentityHostingStartup))]
namespace Cookit.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            /* THIS CODE IS NOT NEEDED HERE, IT IS ALREADY RUN IN THE MAIN STARTUP.CS FILE */

            //builder.ConfigureServices((context, services) => {
            //    services.AddDbContext<CookitContext>(options =>
            //        options.UseSqlServer(
            //            context.Configuration.GetConnectionString("CookitContextConnection")));

            //    services.AddDefaultIdentity<CookitUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //        .AddEntityFrameworkStores<CookitContext>();
            //});
        }
    }
}