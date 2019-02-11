using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Grundlov.App.Services;
using Microsoft.Extensions.Configuration;
using Grundlov.App.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using Grundlov.App.Models;

namespace Grundlov.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContextPool<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            // Since Blazor is running on the server, we can use an application service
            // to read the forecast data.
            services.AddSingleton<WeatherForecastService>();
            services.AddScoped<EmailSender>();
        }

        public void Configure(IBlazorApplicationBuilder app/*, ApplicationDbContext dbContext*/)
        {
            app.AddComponent<App>("app");
        }
    }
}
