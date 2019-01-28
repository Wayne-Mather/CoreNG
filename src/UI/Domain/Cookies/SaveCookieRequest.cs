using CoreNg.RequestResponse.Base;
using Microsoft.AspNetCore.Http;

namespace coreng.Domain.Cookies
{
    public class SaveCookieRequest<T>: BaseRequest<CookieResponseViewModel<T>, SaveCookieResponse<T>> where T: class
    {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;

        public SaveCookieRequest(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
            this.Model = new CookieResponseViewModel<T>();
        }

        public SaveCookieResponse<T> Handle()
        {
            return new SaveCookieHandler(_request, _response).Handle(this);
        }
    }

    public class SaveCookieResponse<T>: BaseResponse<CookieResponseViewModel<T>> where T: class 
    {
        public SaveCookieResponse()
        {
            this.Model = new CookieResponseViewModel<T>();
        }
    }

    public class SaveCookieHandler: BaseCookieHandler
    {
        public SaveCookieHandler(HttpRequest request, HttpResponse response) : base(request, response)
        {
        }

        public SaveCookieResponse<T> Handle<T>(SaveCookieRequest<T> request) where T: class 
        {
            var response = new SaveCookieResponse<T>();

            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.Model.CookieName))
                {
                    response.Model.CookieName = request.Model.CookieName;
                    
                    response.Model.CookieData =
                            base.SetCookie<T>(request.Model.CookieName, request.Model.CookieData);
                    response.IsSuccessful = true;
                }
                else
                {
                    response.AddCustomError("Cookie name not set");
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