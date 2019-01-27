using System.Linq;
using coreng.Data;
using CoreNG.Persistence;
using CoreNG.Persistence.Sqlite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace coreng
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // dotnet ef migrations add xxx --context ApplicationDbContext
            // dotnet ef database update --context ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                    
                    // NOTE: Use the driver required to host the identity tables 
                    // but remember to adjust appsettings.json as needed
                    options.UseSqlite(Configuration.GetConnectionString("IdentityConnection"))
                // options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"))
            );
            
            services.AddDefaultIdentity<ApplicationUser>(o =>
                {
                    // TODO: Configure as necessary for your environment
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 5;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // NOTE: Adjust this to have application data in the right persistence layer
            services.AddDbContext<CoreNG.Persistence.Sqlite.CoreNgDbContext>(o =>
            {
                o.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            CoreNgDbContext coreNgContext, 
            ApplicationDbContext authContext,
            UserManager<ApplicationUser> userManager
            ) 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // seed a default user if does not already exist
            if (!authContext.Users.Any(x => x.UserName == "admin"))
            {
                var u = new ApplicationUser();
                u.UserName = "admin";
                u.Email = "admin@127.0.0.1";
                var task = userManager.CreateAsync(u);
                task.Wait();
                task = userManager.AddPasswordAsync(u, "password123");
                task.Wait();
            }

            DbSeeder.Seed(coreNgContext);

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
