using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MugStore.Data;
using MugStore.Services.Common;
using MugStore.Services.Data;
using MugStore.Web.Attributes;
using MugStore.Web.Infrastructure.Mapping;
using System.Reflection;

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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSession();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("LoggedIn", policy => policy.Requirements.Add(new LoggedInRequirement()));
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

            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/Home/404";
                    await next();
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Image",
                    template: "Download/{name}",
                    defaults: new { controller = "Image", action = "Index" });
                routes.MapRoute(
                    name: "ProductImage",
                    template: "DownloadProductImage/{name}",
                    defaults: new { controller = "Image", action = "ProductImage" });
                routes.MapRoute(
                    name: "GalleryCategory",
                    template: "gallery/{acronym}",
                    defaults: new { controller = "Gallery", action = "Category" });
                routes.MapRoute(
                    name: "Tag",
                    template: "tag/{acronym}",
                    defaults: new { controller = "Gallery", action = "Tag" });
                routes.MapRoute(
                    name: "Product",
                    template: "p/{acronym}",
                    defaults: new { controller = "Product", action = "Index" });
                routes.MapRoute(
                    name: "BlogPost",
                    template: "blog/{acronym}",
                    defaults: new { controller = "Blog", action = "Post" });
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
