using System;
using System.Collections.Generic;

namespace CoreNg.RequestResponse.Interfaces
{
    public interface IResponse<TViewModel>
    {
        TViewModel Model { get; set; }
        List<string> ErrorMessages { get; set; }
        bool IsSuccessful { get; set; }

        void AddRecordNotFoundError();
        void AddRecordAlreadyExistsError();
        void AddNullRequestError();
        void AddException(Exception ex);
        
    }
}