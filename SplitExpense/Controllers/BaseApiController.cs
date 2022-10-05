using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models;

namespace SplitExpense
{
    [ApiController]
    [Authorize]
    public class BaseApiController : ControllerBase
    {
        public HttpContext Context { get => this.Request.HttpContext; }

        public User ContextUser
        {
            get 
            {
                if(Context != null && Context.Items["User"] != null)
                {
                    return (User)Context.Items["User"];
                }

                throw new ArgumentNullException();
            }
        }
    }
}
