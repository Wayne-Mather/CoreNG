using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using coreng.Data;
using CoreNg.RequestResponse.Base;
using CoreNg.RequestResponse.Interfaces;
using CoreNG.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace CoreNG.Domain.Accounts.Requests
{
    public class SearchUsersViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }
        
    }
    
    public class SearchUsersRequest: BaseSearchRequest<SearchUsersViewModel, SearchUsersResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchUsersRequest(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            this.Model = new SearchUsersViewModel();
        }

        public override bool Validate()
        {
            if (base.Validate())
            {
                
            }

            return this.IsValid;
        }

        public override SearchUsersResponse Send()
        {
            var handler = new SearchUsersHandler(_context, this._userManager);
            return handler.Handle(this);
        }
    }

    public class SearchUsersResponse: BaseSearchResponse<SearchUsersViewModel,SearchUsersViewModel>
    {
        public SearchUsersResponse()
        {
            this.Results = new List<SearchResult<SearchUsersViewModel>>();
            this.Model = new SearchUsersViewModel();
        }
    }

    public class SearchUsersHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchUsersHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public SearchUsersResponse Handle(SearchUsersRequest request)
        {
            var response = new SearchUsersResponse();
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
                    IQueryable<ApplicationUser> qry = _context.Users;
                    
                    if (!string.IsNullOrEmpty(request.Model.Username))
                    {
                        qry = qry.Where(x => EfHelper.Like(x.UserName, request.Model.Username));
                    }

                    var lst = qry.ToList();
                    foreach (var user in lst)
                    {
                        var sr = new SearchResult<SearchUsersViewModel>();
                        sr.Result = new SearchUsersViewModel();
                        sr.Result.Username = user.UserName;
                        response.Results.Add(sr);
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