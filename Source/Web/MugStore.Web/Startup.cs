using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MugStore.Data;
using MugStore.Services.Common;
using MugStore.Services.Data;
using MugStore.Web.Attributes;
using MugStore.Web.Infrastructure.Mapping;
using MugStore.Web.Middlewares;

namespace MugStore.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession();

            services.AddAuthentication().AddCookie(options => options.LoginPath = "/Admin/Home/Login");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("LoggedIn", policy =>
                {
                    policy.Requirements.Add(new LoggedInRequirement());
                    policy.AuthenticationSchemes = new List<string>() { CookieAuthenticationDefaults.AuthenticationScheme };
                });
            });

            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true; // This header is not added automatically, so I add it in middleware.
            });

            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(90);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });

            services.AddTransient(typeof(IDbRepository<>), typeof(DbRepository<>));
            services.AddSingleton<IConfiguration>(Configuration);

            // services
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthorizationHandler, LoggedInHandler>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IBulletinsService, BulletinsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<ICitiesService, CitiesService>();
            services.AddTransient<ICouriersService, CouriersService>();
            services.AddTransient<IFeedbacksService, FeedbacksService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<ILoggerService, LoggerService>();
            services.AddTransient<IMailService, MailService>();

            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Cache static files for 30 days
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
                    ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddYears(1).ToString("R", CultureInfo.InvariantCulture));
                }
            });

            // Fix redurect to 404 page and send response code 404
            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    // Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/Home/404";
                    await next();
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "Image",
                    pattern: "Download/{name}",
                    defaults: new { controller = "Image", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "ProductImage",
                    pattern: "DownloadProductImage/{name}",
                    defaults: new { controller = "Image", action = "ProductImage" });
                endpoints.MapControllerRoute(
                    name: "GalleryCategory",
                    pattern: "gallery/{acronym}",
                    defaults: new { controller = "Gallery", action = "Category" });
                endpoints.MapControllerRoute(
                    name: "Tag",
                    pattern: "tag/{acronym}",
                    defaults: new { controller = "Gallery", action = "Tag" });
                endpoints.MapControllerRoute(
                    name: "Product",
                    pattern: "p/{acronym}",
                    defaults: new { controller = "Product", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "BlogPost",
                    pattern: "blog/{acronym}",
                    defaults: new { controller = "Blog", action = "Post" });
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
