using System;
using System.Collections.Generic;
using CoreNg.RequestResponse.Interfaces;

namespace CoreNg.RequestResponse.Base
{
    public abstract class BaseResponse<TViewModel> : IResponse<TViewModel>
    {
        public TViewModel Model { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool IsSuccessful { get; set; }
        
        public BaseResponse()
        {
            this.ErrorMessages = new List<string>();
            this.IsSuccessful = true;
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