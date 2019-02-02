using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using coreng.Controllers;
using coreng.Data;
using CoreNG.Persistence.Sqlite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace coreng.Areas.Api
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class TokenViewModel
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime GenerationDate { get; set; }
    }
    
    [Route("/api/auth")]
    [Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController: BaseCoreNgController
    {
        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext authContext, CoreNgDbContext appContext, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory) : base(userManager, roleManager, signInManager, authContext, appContext, httpContextAccessor, loggerFactory)
        {
        }

        [HttpPost]
        [Route("Ping")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(bool), 404)]
        public IActionResult Ping(bool doFail)
        {
            if (doFail)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }
        
        /// <summary>
        /// If the application knows the token is about to expire
        /// let it refresh the token for as long as it wants over a 24 hour cycle
        /// after 24 hours, the application has to reauthenticate
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Refresh")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenViewModel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public IActionResult Refresh()
        {
            if (Startup.JwtSecurityToken != null)
            {
                var email = Startup.JwtSecurityToken.Subject;
                if (!string.IsNullOrEmpty(email))
                {
                    var tsk = _userManager.FindByEmailAsync(email);
                    tsk.Wait();
                    if (tsk.Result != null)
                    {
                        var vm = CreateToken(tsk.Result, Startup.JwtSecurityToken.ValidTo);
                        if (vm == null)
                        {
                            return Forbid();
                        }
                        else
                        {
                            return Ok(vm);
                        }
                    }
                }
            }

            return BadRequest();
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( TokenViewModel ), 200 )]
        [ProducesResponseType( typeof( string ), 400 )]
        [ProducesResponseType( typeof( string ), 404 )]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var tsk = _userManager.FindByNameAsync(model.Username);
            tsk.Wait();

            if (tsk.Result != null)
            {
                return Ok(CreateToken(tsk.Result, null));
            }

            return BadRequest("Bad Request");
        }

        private TokenViewModel CreateToken(ApplicationUser user, DateTime? generationDate)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration["Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.Now;

            if (generationDate.HasValue)
            {
                // does the user have to login again?
                var diff = now.Subtract(generationDate.Value);
                if (diff.TotalHours > 24)
                {
                    return null;
                }

                now = generationDate.Value;
            }
            
            var expiry = now.AddMinutes(30);
            
            var token = new JwtSecurityToken("CoreNG",
                "CoreNG",
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var vm = new TokenViewModel()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiryDate = expiry,
                GenerationDate = now 
            };
            return vm;
        }
    }
}