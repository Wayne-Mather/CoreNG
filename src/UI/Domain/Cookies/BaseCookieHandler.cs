using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace coreng.Domain.Cookies
{
    public abstract class BaseCookieHandler
    {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;

        protected BaseCookieHandler(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }
        
        protected bool CookieExists(string cookieName)
        {
            return _request != null && _request.Cookies.ContainsKey(cookieName);
        }
        
        protected T GetCookie<T>(string cookieName)
        {
            if (_request != null && _request.Cookies.ContainsKey(cookieName))
            {
                var base64 = _request.Cookies[cookieName];
                var code = Encoding.ASCII.GetString(Convert.FromBase64String(base64));

                try
                {
                    return JsonConvert.DeserializeObject<T>(code);
                }
                catch
                {
                    DeleteCookie(cookieName);
                }
            }
            return default(T);
        }
        
        protected T SetCookie<T>(string cookieName, T model)
        {
            if (_request != null && _response != null)
            {
                this.DeleteCookie(cookieName);

                var options = new CookieOptions {Expires = DateTime.Now.AddDays(1)};
                var code = JsonConvert.SerializeObject(model, Formatting.None);
                var base64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(code));
                _response.Cookies.Append(cookieName, base64, options);
                return model;
            }
            return default(T);
        }
        
        protected void DeleteCookie(string cookieName)
        {
            if (_request != null && _response != null)
            {
                if (_request.Cookies.ContainsKey(cookieName))
                {
                    _response.Cookies.Delete(cookieName);
                }
            }
        }
    }
}