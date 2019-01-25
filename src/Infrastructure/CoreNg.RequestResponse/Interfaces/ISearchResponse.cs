using System;
using System.Collections.Generic;

namespace CoreNg.RequestResponse.Interfaces
{   
    public interface ISearchResponse<TRequestViewModel, TResponseViewModel> 
    {
        TRequestViewModel Filter { get; set; }
        TResponseViewModel Model { get; set; }
        List<string> ErrorMessages { get; set; }
        bool IsSuccessful { get; set; }

        void AddRecordNotFoundError();
        void AddRecordAlreadyExistsError();
        void AddNullRequestError();
        void AddException(Exception ex);
        List<SearchResult<TResponseViewModel>> Results { get; set; }
    }
}