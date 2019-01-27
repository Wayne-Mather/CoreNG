using System;
using System.IO;
using System.Net;
using System.Reflection;
using coreng;
using coreng.Data;
using CoreNG.Persistence;
using CoreNG.Persistence.Sqlite;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace TestCoreNG.Ui
{
    public abstract class BaseTest : IDisposable
    {
        protected ApplicationDbContext _authContext;
        protected IDbContext _context;
        protected UserManager<ApplicationUser> _userManager;

        private IServiceScope _scope;
        private IServiceProvider _services;

        protected BaseTest()
        {
            CreateServices();
        }

        private void CreateServices()
        {
            var assembly = Assembly.GetAssembly(typeof(coreng.Startup));

            IWebHost host = WebHost.CreateDefaultBuilder(null)
                .UseStartup(assembly.FullName)
                .UseUrls()
                .Build();

            _scope = host.Services.CreateScope();
            _services = _scope.ServiceProvider;
            _authContext = _services.GetRequiredService<ApplicationDbContext>();
            _context = _services.GetRequiredService<CoreNgDbContext>();

            _authContext.Database.OpenConnection();
            _authContext.Database.EnsureCreated();
            
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();
            
            _userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
        }

        public void Dispose()
        {
            _authContext?.Dispose();
            _context?.Dispose();
            _userManager?.Dispose();
            _scope?.Dispose();
        }
    }
}