//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;
//using coreng.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.Extensions.Logging;
//
//namespace coreng.Areas.Identity.Pages.Account
//{
//    [AllowAnonymous]
//    public class RegisterModel : PageModel
//    {
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly ILogger<RegisterModel> _logger;
//
//        public RegisterModel(
//            UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager,
//            ILogger<RegisterModel> logger)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _logger = logger;
//        }
//
//        [BindProperty]
//        public InputModel Input { get; set; }
//
//        public string ReturnUrl { get; set; }
//
//        public class InputModel
//        {
//            [Required]
//            [Display(Name = "Username")]
//            public string Username { get; set; }
//
//            [Required]
//            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
//            [DataType(DataType.Password)]
//            [Display(Name = "Password")]
//            public string Password { get; set; }
//
//            [DataType(DataType.Password)]
//            [Display(Name = "Confirm password")]
//            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//            public string ConfirmPassword { get; set; }
//        }
//
//        public void OnGet(string returnUrl = null)
//        {
//            ReturnUrl = returnUrl;
//        }
//
//        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
//        {
//            returnUrl = returnUrl ?? Url.Content("~/");
//            if (ModelState.IsValid)
//            {
//                // TODO: Add corporate domain name here either static variable, configuration, DI....
//                var user = new ApplicationUser { UserName = Input.Username, Email = $"{Input.Username}@127.0.0.1" };
//                var result = await _userManager.CreateAsync(user, Input.Password);
//                if (result.Succeeded)
//                {
//                    _logger.LogInformation("User created a new account with password.");
//                    await _signInManager.SignInAsync(user, isPersistent: false);
//                    return LocalRedirect(returnUrl);
//                }
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }
//
//            // If we got this far, something failed, redisplay form
//            return Page();
//        }
//    }
//}
