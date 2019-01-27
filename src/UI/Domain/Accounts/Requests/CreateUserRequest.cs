using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using coreng.Data;
using CoreNg.RequestResponse.Base;
using CoreNG.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace CoreNG.Domain.Accounts.Requests
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
    }
    
    public class CreateUserRequest: BaseRequest<CreateUserViewModel, CreateUserResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUserRequest(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            this.Model = new CreateUserViewModel();
        }

        public override bool Validate()
        {
            if (base.Validate())
            {
                var haveBothPasswords = true;
                
                if (string.IsNullOrEmpty(this.Model.Username))
                {
                    this.AddEmptyPropertyError(nameof(this.Model.Username));
                }
                
                if (string.IsNullOrEmpty(this.Model.Password))
                {
                    this.AddEmptyPropertyError(nameof(this.Model.Password));
                    haveBothPasswords = false;
                }
                
                if (string.IsNullOrEmpty(this.Model.ConfirmPassword))
                {
                    this.AddEmptyPropertyError(nameof(this.Model.ConfirmPassword));
                    haveBothPasswords = false;
                }

                if (haveBothPasswords)
                {
                    if (this.Model.Password != this.Model.ConfirmPassword)
                    {
                        this.AddCustomerError("Password and ConfirmPassword do not match");
                    }
                }
            }

            return this.IsValid;
        }

        public override CreateUserResponse Send()
        {
            var handler = new CreateUserHandler(_context, this._userManager);
            return handler.Handle(this);
        }
    }

    public class CreateUserResponse: BaseResponse<CreateUserViewModel>
    {
        public CreateUserResponse()
        {
            this.Model = new CreateUserViewModel();
        }
    }

    public class CreateUserHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUserHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public CreateUserResponse Handle(CreateUserRequest request)
        {
            var response = new CreateUserResponse();
            response.IsSuccessful = true;
            
            if (request != null)
            {
                if (!request.Validate())
                {
                    response.IsSuccessful = false;
                    response.ErrorMessages = request.ErrorMessages;
                }
                else
                {
                    if (!_context.Users.Any(x => x.UserName == request.Model.Username))
                    {
                        var u = new ApplicationUser {UserName = request.Model.Username};
                        u.Email = $"{u.UserName}@127.0.0.1";
                        var tsk = _userManager.CreateAsync(u);
                        tsk.Wait();
                        if (tsk.Result.Succeeded)
                        {
                            tsk = _userManager.AddPasswordAsync(u, request.Model.Password);
                            tsk.Wait();
                            if (!tsk.Result.Succeeded)
                            {
                                foreach (var error in tsk.Result.Errors)
                                {
                                    response.AddCustomError(error.Description);
                                }
                            }
                        }
                        else
                        {
                            foreach (var error in tsk.Result.Errors)
                            {
                                response.AddCustomError(error.Description);
                            }
                        }

                    }
                    else
                    {
                        response.AddRecordAlreadyExistsError();
                    }
                }
            }
            else
            {
                response.AddNullRequestError();
            }

            return response;
        }
    }
}