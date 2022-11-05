using FeatureFlagsSample.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

namespace FeatureFlagsSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var appConfigConnectionString = builder.Configuration.GetConnectionString("AppConfig");

            builder.Host.ConfigureAppConfiguration(builder =>
            {
                builder.AddAzureAppConfiguration(options => options
                    .Connect(appConfigConnectionString)
                    .ConfigureRefresh(refreshOptions =>
                    {
                        refreshOptions.Register("Version", refreshAll: true);
                        refreshOptions.SetCacheExpiration(TimeSpan.FromSeconds(1));
                    })
                    .UseFeatureFlags());
            });

            builder.Services.AddFeatureManagement()
                .AddFeatureFilter<TimeWindowFilter>()
                .AddFeatureFilter<PercentageFilter>()
                .AddFeatureFilter<TargetingFilter>()
                .AddFeatureFilter<CustomFeatureFilter>();

            builder.Services.AddAzureAppConfiguration();

            builder.Services.AddSingleton<ITargetingContextAccessor, VIPTargetingContextAccessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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

            app.UseAzureAppConfiguration();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}