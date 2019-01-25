using System.Collections.Generic;

namespace CoreNg.RequestResponse.Interfaces
{
    public interface IRequest<TViewModel, out TResponse> where TResponse :IResponse<TViewModel>
    {
        TViewModel Model { get; set; }
        List<string> ErrorMessages { get; set; }
        bool IsValid { get; set; }

        TResponse Send();

        bool Validate();
        void AddEmptyPropertyError(string propertyName);
        void AddNullPropertyError(string propertyName);
        void AddCustomerError(string msg);
    }
}