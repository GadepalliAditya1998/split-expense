using Microsoft.AspNetCore.Http;
using SplitExpense.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Services.Core
{
    public class BaseService
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public User ContextUser
        {
            get
            {
                if (this.HttpContextAccessor != null && this.HttpContextAccessor.HttpContext.Items["User"] != null)
                {
                    return (User)this.HttpContextAccessor.HttpContext.Items["User"];
                }

                throw new ArgumentNullException();
            }
        }

        public BaseService(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
        }
    }
}
