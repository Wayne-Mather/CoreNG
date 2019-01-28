using CoreNg.RequestResponse.Base;
using Microsoft.AspNetCore.Http;

namespace coreng.Domain.Cookies
{
    public class CookieResponseViewModel<T> where T: class 
    {
        public string CookieName { get; set; }
        public T CookieData { get; set; }
    }
    
    public class CookieRequestViewModel
    {
        public string CookieName { get; set; }
    }
    
    public class GetCookieRequest<T>: BaseRequest<CookieResponseViewModel<T>, GetCookieResponse<T>> where T: class
    {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;

        public GetCookieRequest(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
            this.Model = new CookieResponseViewModel<T>();
        }

        public GetCookieResponse<T> Handle()
        {
            return new GetCookieHandler(_request, _response).Handle(this);
        }
    }

    public class GetCookieResponse<T>: BaseResponse<CookieResponseViewModel<T>> where T: class 
    {
        public GetCookieResponse()
        {
            this.Model = new CookieResponseViewModel<T>();
        }
    }

    public class GetCookieHandler: BaseCookieHandler
    {
        public GetCookieHandler(HttpRequest request, HttpResponse response) : base(request, response)
        {
        }

        public GetCookieResponse<T> Handle<T>(GetCookieRequest<T> request) where T: class 
        {
            var response = new GetCookieResponse<T>();

            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.Model.CookieName))
                {
                    response.Model.CookieName = request.Model.CookieName;
                    
                    if (base.CookieExists(request.Model.CookieName))
                    {
                        response.Model.CookieData = base.GetCookie<T>(request.Model.CookieName);
                        response.IsSuccessful = true;
                    }
                    else
                    {
                        response.AddCustomError("Cookie does not exist");
                    }
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