using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Exceptions;
using SplitExpense.Core.Models;

namespace SplitExpense
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public HttpContext Context { get => this.Request.HttpContext; }

        public User ContextUser
        {
            get 
            {
                if(Context != null && Context.Items["User"] != null)
                {
                    return Context.Items["User"] as User;
                }

                throw new UnAuthorizedException();
            }
        }
    }
}
