using Grundlov.App.Data;
using Grundlov.App.Models;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace Grundlov.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Grundlov.App")));

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Adds the Server-Side Blazor services, and those registered by the app project's startup.
            services.AddServerSideBlazor<App.Startup>();

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });

            
            services.AddAuthentication()
                .AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = Configuration["Twitter:ConsumerKey"];
                    twitterOptions.ConsumerSecret = Configuration["Twitter:ConsumerSecret"];
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            dbContext.Database.MigrateAsync();

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Register OAuth callbacks here.
            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path != "/test")
            //    {
            //        await next();
            //    }
            //    else
            //    {
            //        context.Response.StatusCode = 404;
            //        context.Response.Headers[HeaderNames.ContentType] = "text/html; charset=utf-8";
            //        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("BAM!"));
            //        await context.Response.Body.FlushAsync();
            //    }
            //});

            // Use component registrations and static files from the app project.
            app.UseServerSideBlazor<App.Startup>();
        }
    }
}
