using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rental_kendaraan_DBFirst_KelasC.Models;

[assembly: HostingStartup(typeof(rental_kendaraan_DBFirst_KelasC.Areas.Identity.IdentityHostingStartup))]
namespace rental_kendaraan_DBFirst_KelasC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}