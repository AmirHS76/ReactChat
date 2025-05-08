using Microsoft.AspNetCore.Http;
using System.Text;

namespace ReactChat.Application.Services.Captcha
{
    public class SessionCaptchaStore
    {
        private const string CaptchaKey = "CaptchaCode";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionCaptchaStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Save(string captchaText)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Set(CaptchaKey, Encoding.UTF8.GetBytes(captchaText));
            }
        }

        public string? Retrieve()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null && session.TryGetValue(CaptchaKey, out var value))
            {
                return Encoding.UTF8.GetString(value);
            }
            return null;
        }
    }
}
