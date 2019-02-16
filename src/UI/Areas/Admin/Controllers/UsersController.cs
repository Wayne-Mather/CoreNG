using coreng.Controllers;
using coreng.Data;
using coreng.Domain.Cookies;
using CoreNg.RequestResponse.Base;
using CoreNG.Domain.Accounts.Requests;
using CoreNG.Persistence.Sqlite;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;



namespace coreng.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController: BaseCoreNgController
    {  
                                           
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext authContext, CoreNgDbContext appContext, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory) : base(userManager, roleManager, signInManager, authContext, appContext, httpContextAccessor, loggerFactory)
        {
        }

        private string _cookieName = "CORENG_USERS";
        
        #region Search
        
        [HttpGet]
        public IActionResult Index()
        {
            var cookieRequest = new GetCookieRequest<SearchUsersViewModel>(Request, Response);
            cookieRequest.Model.CookieName = _cookieName;
            var cookieResponse = cookieRequest.Send();

            var request = new SearchUsersRequest(_authContext, _userManager);

            if (cookieResponse.IsSuccessful)
            {
                request.Model = cookieResponse.Model.CookieData;
            }

            var response = request.Send();
            response.Model = request.Model;
            return View(response);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(SearchUsersResponse vm)
        {
            var cookieRequest = new SaveCookieRequest<SearchUsersViewModel>(Request, Response);
            cookieRequest.Model.CookieData = vm.Model;
            cookieRequest.Model.CookieName = _cookieName;
            var cookieResponse = cookieRequest.Send();

            var request = new SearchUsersRequest(_authContext, _userManager);
            request.Model = vm.Model;

            var response = request.Send();
            response.Model = request.Model;
            return View(response);
        }
        
        #endregion 
        
        #region Create
        
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new CreateUserResponse();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserResponse vm)
        {
            var request = new CreateUserRequest(_authContext, _userManager);
            request.Model = vm.Model;
            var response = request.Send();
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }

            vm.IsSuccessful = false;
            vm.ErrorMessages = response.ErrorMessages;
            return View(vm);
        }
        
        #endregion 
        
        #region Edit 
        
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var vm = new UpdateUserResponse();
            vm.Model.Fill(id, _authContext);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UpdateUserResponse vm)
        {
            if (vm == null || vm.Model == null)
            {
                return RedirectToAction("Index");
            }

            var request = new UpdateUserRequest(_authContext, _userManager);
            request.Model = vm.Model;
            var response = request.Send();
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }

            return View(response);
        }
        
        #endregion 
    }
}