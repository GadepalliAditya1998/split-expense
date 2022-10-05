using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services;

namespace SplitExpense.Controllers
{
    [Route("/api/users/")]
    public class UserController: BaseApiController
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public int CreateUser(AddUser user)
        {
            return this.userService.CreateUser(user);
        }

        [HttpPost("exists")]
        public bool DoesUserExists(User user)
        {
            return this.userService.DoesUserExists(user);
        }
    }
}
