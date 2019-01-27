using System.ComponentModel.DataAnnotations;
using System.Linq;
using coreng.Data;
using CoreNg.RequestResponse.Base;
using CoreNG.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace CoreNG.Domain.Accounts.Requests
{
    public class DeleteUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
    }
    
    public class DeleteUserRequest: BaseRequest<DeleteUserViewModel, DeleteUserResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserRequest(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            this.Model = new DeleteUserViewModel();
        }

        public override bool Validate()
        {
            if (base.Validate())
            {
                if (string.IsNullOrEmpty(this.Model.Username))
                {
                    this.AddEmptyPropertyError(nameof(this.Model.Username));
                }
            }

            return this.IsValid;
        }

        public override DeleteUserResponse Send()
        {
            var handler = new DeleteUserHandler(_context, this._userManager);
            return handler.Handle(this);
        }
    }

    public class DeleteUserResponse: BaseResponse<DeleteUserViewModel>
    {
        public DeleteUserResponse()
        {
            this.Model = new DeleteUserViewModel();
        }
    }

    public class DeleteUserHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteUserHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public DeleteUserResponse Handle(DeleteUserRequest request)
        {
            var response = new DeleteUserResponse();
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
                    if (_context.Users.Any(x => x.UserName == request.Model.Username))
                    {
                        var u = _context.Users.Single(x => x.UserName == request.Model.Username);
                        var tsk = _userManager.DeleteAsync(u);
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
                        response.AddRecordNotFoundError();
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