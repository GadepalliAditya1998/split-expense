using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services;

namespace SplitExpense.Controllers
{
    [Route("/api/search/")]
    public class SearchController : BaseApiController
    {
        private readonly UserService userService;

        public SearchController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("users")]
        public IEnumerable<SearchUserListItem> GetUserConnections(string query)
        {
            return this.userService.GetUserConnections(this.ContextUser.Id, query);
        }
    }
}
