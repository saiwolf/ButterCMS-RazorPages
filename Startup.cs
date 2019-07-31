using ButterCMS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SWMNU_NET.Configuration;
using SWMNU_NET.Data;
using SWMNU_NET.Services;

namespace SWMNU_NET
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.AddPageRoute("/Blog/Index", "");
                        options.Conventions.AddPageRoute("/Blog/PostDetail", "/blog/{slug}");
                        options.Conventions.AddPageRoute("/Taxonomy/TagDetails", "/tag/{slug}");
                        options.Conventions.AddPageRoute("/Taxonomy/TagDetails", "/tag/{slug}/p/{page}");
                        options.Conventions.AddPageRoute("/Taxonomy/CategoryDetails", "/category/{slug}");
                        options.Conventions.AddPageRoute("/Taxonomy/CategoryDetails", "/category/{slug}/p/{page}");
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));

            services.AddSingleton<IHostingEnvironment>(_env);

            services.AddOptions();
            services.Configure<UrlOptions>(Configuration.GetSection("UrlOptions"));
            services.Configure<ButterCmsOptions>(Configuration.GetSection("ButterCMSOptions"));

            services.AddScoped<ButterCMSClient>(c =>
                new ButterCMSClient(c.GetRequiredService<IOptions<ButterCmsOptions>>().Value.ApiKey));

            services.AddTransient<TaxonomyService>();
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
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
