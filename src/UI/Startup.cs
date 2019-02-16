using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using coreng.Data;
using CoreNG.Domain.Accounts.Requests;
using CoreNG.Persistence;
using CoreNG.Persistence.Sqlite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace coreng
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }
        public static JwtSecurityToken JwtSecurityToken { get; private set; }

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

            services.AddAuthentication(opt =>
                    {
                        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;

                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "CoreNG",
                    ValidAudience = "CoreNG",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token"])),
                    ValidateIssuerSigningKey = true 
                };

                cfg.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var accessToken = context.SecurityToken as JwtSecurityToken;
                        if (accessToken != null)
                        {
                            Startup.JwtSecurityToken = accessToken;
                        }
                        else
                        {
                            Startup.JwtSecurityToken = null;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
                
            services
                .AddIdentity<ApplicationUser, IdentityRole>(o =>
                {
                    // TODO: Configure as necessary for your environment
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 5;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // NOTE: Adjust this to have application data in the right persistence layer
            services.AddDbContext<CoreNG.Persistence.Sqlite.CoreNgDbContext>(o =>
            {
                o.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
#if DEBUG
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info() {Title = "CoreNG API", Version = "v1"}); });
#endif
            services.AddCors();

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

            new SeedAuthDbRequest(authContext,userManager).Seed();
            DbSeeder.Seed(coreNgContext);

            app.UseStaticFiles();

            app.UseCors(opt =>
            {
                opt.AllowAnyHeader();
                opt.AllowAnyMethod();
                opt.AllowAnyOrigin();
            });
            
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreNG API V1");
            });
#endif            
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
