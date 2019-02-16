using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using coreng.Data;
using CoreNg.RequestResponse.Base;
using CoreNG.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreNG.Domain.Accounts.Requests
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }
        
        public string Username { get; set; }

        public string Email { get; set; }
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        public bool HavePasswordChange { get; internal set; }

        public void Fill(string id, ApplicationDbContext authContext)
        {
            if (authContext != null)
            {
                var user = authContext.Users.Single(x => x.Id == id);
                this.Email = user.Email;
                this.Username = user.UserName;
                this.Id = id;

            }
        }
    }
    
    public class UpdateUserRequest: BaseRequest<UpdateUserViewModel, UpdateUserResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserRequest(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            this.Model = new UpdateUserViewModel();
        }

        public override bool Validate()
        {
            if (base.Validate())
            {
                var havePasswordChange = false;
                var haveBothPasswords = true;
                
                if (string.IsNullOrEmpty(this.Model.Username))
                {
                    this.AddEmptyPropertyError(nameof(this.Model.Username));
                }

                if (string.IsNullOrEmpty(this.Model.Email))
                {
                    this.AddEmptyPropertyError(nameof(this.Model.Email));
                }

                if (!string.IsNullOrEmpty(this.Model.Password))
                {
                    havePasswordChange = true;
                    this.AddEmptyPropertyError(nameof(this.Model.Password));
                    haveBothPasswords = false;

                    if (string.IsNullOrEmpty(this.Model.ConfirmPassword))
                    {
                        this.AddEmptyPropertyError(nameof(this.Model.ConfirmPassword));
                        haveBothPasswords = false;
                    }
                    else
                    {
                        haveBothPasswords = true;
                    }
                }

                if (havePasswordChange && haveBothPasswords)
                {
                    if (this.Model.Password != this.Model.ConfirmPassword)
                    {
                        this.AddCustomerError("Password and ConfirmPassword do not match");
                    }
                }

                this.Model.HavePasswordChange = havePasswordChange;

            }

            return this.IsValid;
        }

        public override UpdateUserResponse Send()
        {
            var handler = new UpdateUserHandler(_context, this._userManager);
            return handler.Handle(this);
        }
    }

    public class UpdateUserResponse: BaseResponse<UpdateUserViewModel>
    {
        public UpdateUserResponse()
        {
            this.Model = new UpdateUserViewModel();
        }
    }

    public class UpdateUserHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateUserHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public UpdateUserResponse Handle(UpdateUserRequest request)
        {
            var response = new UpdateUserResponse();
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
                        var user = _userManager.FindByNameAsync(request.Model.Username).Result;
                        if (user != null)
                        {
                            user.Email = request.Model.Email;
                            _userManager.UpdateAsync(user);

                            if (request.Model.HavePasswordChange)
                            {
                                var res = _userManager.RemovePasswordAsync(user).Result;
                                res = _userManager.AddPasswordAsync(user, request.Model.Password).Result;
                            }
                        }
                        else
                        {
                            response.AddCustomError("Record exists but UserManager cannot find");
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