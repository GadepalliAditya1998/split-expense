using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using SplitExpense.Core.Exceptions;
using SplitExpense.Core.Handlers;
using SplitExpense.Core.Services;

namespace SplitExpense.Core.Filters
{
    public class AuthorizeApiFilter : IAuthorizationFilter
    {
        private readonly IConfiguration Configuration;

        private readonly IAuthTokenHandler AuthTokenHandler;

        private readonly UserService UserService;

        public AuthorizeApiFilter(IConfiguration configuration, IAuthTokenHandler authTokenHandler, UserService userService)
        {
            this.Configuration = configuration;
            this.AuthTokenHandler = authTokenHandler;
            this.UserService = userService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!this.IsAnonymousRoute(context))
            {
                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if(string.IsNullOrEmpty(token))
                {
                    throw new UnAuthorizedException();
                }

                var tokenUser = this.AuthTokenHandler.GetTokenUserInfo(token);
                
                if (tokenUser == null)
                {
                    throw new UnAuthorizedException();
                }

                var user = this.UserService.GetUserById(tokenUser.UserId);
                if (user == null)
                {
                    throw new UnAuthorizedException();
                }

                // attach user to context on successful jwt validation
                context.HttpContext.Items["User"] = user;
            }
        }

        private bool IsAnonymousRoute(AuthorizationFilterContext context)
        {
            return ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(true).Any(a => a is AllowAnonymousAttribute);
        }
    }
}
