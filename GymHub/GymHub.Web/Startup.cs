using AutoMapper;
using GymHub.Common.AutomapperProfiles;
using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.SeederFolder;
using GymHub.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Policy;
using System.Threading.Tasks;

namespace GymHub.Web
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
            //Add Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Add Identity
            services.AddDefaultIdentity<User>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<Role>().AddEntityFrameworkStores<ApplicationDbContext>();

            //Add Razor and views
            services.AddControllersWithViews();
            services.AddRazorPages();

            //Services for working with the database
            services.AddTransient<IGenderService, GenderService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICartService, CartService>();

            services.AddAutoMapper(typeof(UserProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Seed database at startup        
            SeedDatabaseAsync(app, true).GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private async Task SeedDatabaseAsync(IApplicationBuilder app, bool willSeed)
        {
            if (willSeed)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var seeder = new Seeder(serviceScope.ServiceProvider);
                    await seeder.SeedAsync();
                }
            }
        }
    }
}
