using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services;

namespace SplitExpense.Controllers
{
    [Route("/api/login")]
    public class LoginController : BaseApiController
    {
        private readonly UserService userService;

        public LoginController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public string Login(LoginUser loginUser)
        {
            return this.userService.LoginUser(loginUser);
        }
    }
}
