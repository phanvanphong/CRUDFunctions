using System;
using DemoDotNet5.Areas.Identity.Data;
using DemoDotNet5.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(DemoDotNet5.Areas.Identity.IdentityHostingStartup))]
namespace DemoDotNet5.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DemoDotNet5Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DemoDotNet5ContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options =>
                {

                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                })
                    .AddEntityFrameworkStores<DemoDotNet5Context>();
            });
        }
    }
}