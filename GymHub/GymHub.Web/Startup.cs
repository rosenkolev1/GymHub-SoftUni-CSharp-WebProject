using AutoMapper;
using GymHub.Automapper.AutomapperProfiles;
using GymHub.Data;
using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services;
using GymHub.Services.Messaging;
using GymHub.Services.SeederFolder;
using GymHub.Services.ServicesFolder.CartService;
using GymHub.Services.ServicesFolder.CategoryService;
using GymHub.Services.ServicesFolder.CountryService;
using GymHub.Services.ServicesFolder.GenderService;
using GymHub.Services.ServicesFolder.PaymentMethodService;
using GymHub.Services.ServicesFolder.ProductCommentService;
using GymHub.Services.ServicesFolder.ProductService;
using GymHub.Services.ServicesFolder.RoleService;
using GymHub.Services.ServicesFolder.SaleService;
using GymHub.Web.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using Stripe.Checkout;
using System;
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
                    Configuration.GetConnectionString("DefaultConnection")
                    ));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            //Add Identity
            services.AddDefaultIdentity<User>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<Role>().AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            //Add sessions
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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

            //Add Stripe
            StripeConfiguration.ApiKey = this.Configuration["Stripe:ApiKey"];
            services.AddTransient<SessionService>();

            //Add Automapper
            services.AddAutoMapper(typeof(UserProfile));

            //Services for working with the database
            services.AddTransient<IGenderService, GenderService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, Services.ServicesFolder.ProductService.ProductService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IProductCommentService, ProductCommentService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISaleService, SaleService>();
            services.AddTransient<IPaymentMethodService, Services.ServicesFolder.PaymentMethodService.PaymentMethodService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Delete database at startup
            DeleteDatabase(app, true);

            //Migrate Database at startup
            MigrateDatabase(app, true);

            //Seed database at startup        
            SeedDatabaseAsync(app, true).GetAwaiter().GetResult();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            else if(env.IsProduction())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();



            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

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
                    var seeder = new Seeder(serviceScope.ServiceProvider, this.Configuration);
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
