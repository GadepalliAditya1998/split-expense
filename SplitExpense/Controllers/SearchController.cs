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

        [HttpGet("users/connections")]
        public IEnumerable<SearchConnectionUserListItem> GetUserConnections(string query)
        {
            return this.userService.GetUserConnectionSearchResults(this.ContextUser.Id, query);
        }

        [HttpGet("users")]
        public IEnumerable<UserSearchResultItem> GetUserSearchResults(string query)
        {
            return this.userService.GetUserSearchResultItems(this.ContextUser.Id, query);
        }
    }
}
