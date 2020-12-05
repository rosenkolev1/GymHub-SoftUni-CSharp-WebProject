using AutoMapper;
using GymHub.Automapper.AutomapperProfiles;
using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.SeederFolder;
using GymHub.Web.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendGrid;
using System.Threading.Tasks;
using YourProjectName.Services.Messaging;

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

            services.AddDatabaseDeveloperPageExceptionFilter();

            //Add Identity
            services.AddDefaultIdentity<User>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<Role>().AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            //Add Razor and views
            services.AddControllersWithViews(option =>
            {
                option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddRazorPages();

            //Authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizeAsAdminHandler", policy =>
                    policy.Requirements.Add(new AuthorizeAsAdminRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, AuthorizeAsAdminHandler>();

            // Add sendgrid
            var sendGrid = new SendGridEmailSender(this.Configuration["SendGrid:ApiKey"]);
            services.AddSingleton(sendGrid);

            //Add Automapper
            services.AddAutoMapper(typeof(UserProfile));

            //Services for working with the database
            services.AddTransient<IGenderService, GenderService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IProductCommentService, ProductCommentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Delete database at startup
            DeleteDatabase(app, false);

            //Migrate Database at startup
            MigrateDatabase(app, false);

            //Seed database at startup        
            SeedDatabaseAsync(app, true).GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

        private void MigrateDatabase(IApplicationBuilder app, bool willMigrate)
        {
            if (willMigrate)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                }
            }
        }

        private void DeleteDatabase(IApplicationBuilder app, bool willDelete)
        {
            if (willDelete)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.EnsureDeleted();
                }
            }
        }
    }
}
