using System.Collections.Generic;
using CoreNg.RequestResponse.Interfaces;

namespace CoreNg.RequestResponse.Base
{
    public abstract class BaseRequest<TViewModel,TResponse> : IRequest<TViewModel, TResponse> where TResponse : IResponse<TViewModel>
    {
        public TViewModel Model { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool IsValid { get; set; }

        public BaseRequest()
        {
            this.ErrorMessages = new List<string>();
            this.IsValid = false;
        }

        public virtual TResponse Send()
        {
            return default(TResponse);
        }

        public virtual bool Validate()
        {
            if (this.Model == null)
            {
                this.IsValid = false;
            }
            else
            {
                this.IsValid = true;
            }
            return this.IsValid;
        }

        private void AddError(string msg)
        {
            this.ErrorMessages.Add(msg);
            this.IsValid = false;
        }
        
        public void AddEmptyPropertyError(string propertyName)
        {
            this.AddError($"Property {propertyName} cannot be null or empty");
        }

        public void AddNullPropertyError(string propertyName)
        {
            this.AddError($"Property {propertyName} cannot be null");
        }

        public void AddCustomerError(string msg)
        {
            this.AddError(msg);
        }

    }
}