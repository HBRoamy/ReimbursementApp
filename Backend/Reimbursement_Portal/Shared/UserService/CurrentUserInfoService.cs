using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Shared.UserService
{
    public class CurrentUserInfoService: ICurrentUserInfoService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserInfoService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetCurrentUserEmail()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.Name); // if it doesn't return what you desire, use ClaimsTypes.Name
        }
    }
}
