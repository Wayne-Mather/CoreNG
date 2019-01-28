using System.Linq;
using coreng.Data;
using CoreNG.Persistence.Sqlite;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Clauses.ResultOperators;

namespace coreng.Controllers
{
    public abstract class BaseCoreNgController: Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly ApplicationDbContext _authContext;
        protected readonly CoreNgDbContext _appContext;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILoggerFactory _loggerFactory;

        protected readonly string _userName;
        protected readonly ApplicationUser CurrentUser;
        
        protected BaseCoreNgController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext authContext,
            CoreNgDbContext appContext,
            IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _authContext = authContext;
            _appContext = appContext;
            _httpContextAccessor = httpContextAccessor;
            _loggerFactory = loggerFactory;

            var user = httpContextAccessor?.HttpContext?.User;
            this._userName = user != null && user.Identity != null ? user.Identity.Name : "";

            if (!string.IsNullOrEmpty(_userName) && _authContext.Users.Any(x=>x.UserName == _userName))
            {
                this.CurrentUser = _authContext.Users.FirstOrDefault(x => x.UserName == _userName);
            }
            else
            {
                this.CurrentUser = new ApplicationUser() {UserName = "System"};
            }

        }
    }
}