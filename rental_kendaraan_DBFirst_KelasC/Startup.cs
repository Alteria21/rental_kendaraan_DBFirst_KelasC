using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rental_kendaraan_DBFirst_KelasC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using rental_kendaraan_DBFirst_KelasC.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace rental_kendaraan_DBFirst_KelasC
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
            services.AddSingleton<IFileProvider>(
             new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<Models.rental_kendaraanContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<rental_kendaraanContext>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI()
              .AddEntityFrameworkStores<rental_kendaraanContext>().AddDefaultTokenProviders();


            services.AddAuthorization(options => {
                options.AddPolicy("readonlypolicy",
                    builder => builder.RequireRole("Admin", "Manager", "Kasir"));
                options.AddPolicy("writepolicy",
                    builder => builder.RequireRole("Admin", "Kasir"));
                options.AddPolicy("editpolicy",
                    builder => builder.RequireRole("Admin", "Kasir"));
                options.AddPolicy("deletepolicy",
                builder => builder.RequireRole("Admin", "Kasir"));
            });


            services.AddScoped<Peminjaman>();
            services.AddScoped<Pengembalian>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
