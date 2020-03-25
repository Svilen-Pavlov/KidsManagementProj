using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using KidsManagement.Data;
using KidsManagement.Data.Models;
using KidsManagement.Services.External.CloudinaryService;
using KidsManagement.Services.Groups;
using KidsManagement.Services.Parents;
using KidsManagement.Services.Payments;
using KidsManagement.Services.Students;
using KidsManagement.Services.Teachers;
using KidsManagement.Web.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KidsManagement.Web
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
            services.AddTransient<IGroupsService, GroupsService>();
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddTransient<ITeachersService, TeachersService>();
            services.AddTransient<IParentsService, ParentsService>();
            services.AddTransient<IPaymentsService, PaymentsService>();
            
            services.AddTransient<ICloudinaryService, CloudinaryService>();


            services.AddDbContext<KidsManagementDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>
                (options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 2;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<KidsManagementDbContext>()
                //.AddUserStore<KidsManagementDbContext>()
                //.AddRoleStore<KidsManagementDbContext>()
                .AddDefaultTokenProviders();

            services.AddRazorPages();

            services.AddControllersWithViews(configure =>
            {
                configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            //External Services

            Account account = new Account(
               this.Configuration["Cloudinary:Cloud_Name"],
               this.Configuration["Cloudinary:API_Key"],
               this.Configuration["Cloudinary:API_Secret"]);
            
            Cloudinary cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scopedService = app.ApplicationServices.CreateScope())
            {
                var dbContext = scopedService.ServiceProvider.GetService<KidsManagementDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                //var seeder = new IdentitySeeder(scopedService.ServiceProvider, dbContext); seeder.SeedAll().GetAwaiter().GetResult();
                
            }


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles(); //has todo with identity UI

            app.UseRouting();

            app.UseAuthentication(); //IDENTITY
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
