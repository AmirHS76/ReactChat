using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReactChat.Application.Interfaces.User;
using System.Security.Claims;

namespace ReactChat.Presentation.Helpers.Attributes
{
    public class CustomAuthorizeAttribute(params string[] roles) : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles = roles;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the user is authenticated
            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Resolve UserService from the service provider
            var userService = context.HttpContext.RequestServices.GetService<IUserService>();
            if (userService == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Retrieve the username from the claims
            var username = context.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            // Fetch the user and check the role
            var baseUser = userService.GetUserByUsernameAsync(username).Result;
            if (!_roles.Any(role => baseUser?.UserRole.ToString() == role))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
