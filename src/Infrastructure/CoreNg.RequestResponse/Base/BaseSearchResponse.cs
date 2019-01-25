using System;
using System.Collections.Generic;
using CoreNg.RequestResponse.Interfaces;

namespace CoreNg.RequestResponse.Base
{
    
    
    public abstract class BaseSearchResponse<TRequestViewModel, TViewModel> : ISearchResponse<TRequestViewModel, TViewModel>
    {
        public TRequestViewModel Filter { get; set; }
        public TViewModel Model { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool IsSuccessful { get; set; }
        public List<SearchResult<TViewModel>> Results { get; set; }

        public BaseSearchResponse()
        {
            this.Results = new List<SearchResult<TViewModel>>();
        }
        
        private void AddError(string msg)
        {
            this.ErrorMessages.Add(msg);
            this.IsSuccessful = false;
        }
        
        public void AddRecordNotFoundError()
        {
            this.AddError("Record not found");
        }

        public void AddRecordAlreadyExistsError()
        {
            this.AddError("Record already exists");
        }

        public void AddNullRequestError()
        {
            this.AddError("Request cannot be null");
        }

        public void AddException(Exception ex)
        {
            this.AddError($"Exception Caught: {ex.Message}");
        }

        public void AddCustomError(string msg)
        {
            this.AddError(msg);
        }
    }
}