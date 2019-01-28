using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using coreng.Data;
using CoreNg.RequestResponse.Base;
using CoreNG.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace CoreNG.Domain.Accounts.Requests
{
    public class GetUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
    }

    public class UserRoleViewModel
    {
        public bool IsSelected { get; set; }
        public string RoleName { get; set; }
    }
    
    public class GetUserRequest: BaseRequest<GetUserViewModel, GetUserResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserRequest(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            this.Model = new GetUserViewModel();
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

        public override GetUserResponse Send()
        {
            var handler = new GetUserHandler(_context, this._userManager);
            return handler.Handle(this);
        }
    }

    public class GetUserResponse: BaseResponse<GetUserViewModel>
    {
        public List<UserRoleViewModel> Roles { get; set; }
        
        public GetUserResponse()
        {
            this.Model = new GetUserViewModel();
            this.Roles = new List<UserRoleViewModel>();
        }
    }

    public class GetUserHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public GetUserResponse Handle(GetUserRequest request)
        {
            var response = new GetUserResponse();
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
                        var user = _context.Users.AsNoTracking().Single(x => x.UserName == request.Model.Username);
                        var roles = _context.Roles.AsNoTracking().ToList();
                        foreach (var role in roles)
                        {
                            var roleVM = new UserRoleViewModel();
                            roleVM.RoleName = role.Name;
                            roleVM.IsSelected = _context.UserRoles.Any(x => x.UserId == user.Id);
                            response.Roles.Add(roleVM);
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