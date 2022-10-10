using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services;
using SplitExpense.Core.Services.Core;

namespace SplitExpense.Controllers
{
    [Route("/api/login")]
    public class LoginController : BaseApiController
    {
        private readonly UserService userService;
        private readonly IConfiguration configurationManager;

        public LoginController(UserService userService, IConfiguration configurationManager)
        {
            this.userService = userService;
            this.configurationManager = configurationManager;
        }

        [HttpPost("")]
        [AllowAnonymous]
        public object Login(LoginUser loginUser)
        {
            return new
            {
                Token = this.userService.LoginUser(loginUser)
            };
        }
    }
}
