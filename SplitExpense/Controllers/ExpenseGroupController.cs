using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
using SplitExpense.Core.Services;

namespace SplitExpense.Controllers
{
    [Route("api/group")]
    public class ExpenseGroupController : BaseApiController
    {
        private readonly ExpenseGroupService expenseGroupService;

        public ExpenseGroupController(ExpenseGroupService expenseGroupService)
        {
            this.expenseGroupService = expenseGroupService;
        }

        [HttpGet("all")]
        public IEnumerable<ExpenseGroup> GetExpenseGroups()
        {
            return this.expenseGroupService.GetAllGroups();
        }

        [HttpPost("add")]
        public int AddGroup(ExpenseGroup expenseGroup)
        {
            return this.expenseGroupService.AddGroup(expenseGroup);
        }

        [HttpPost("{groupId}/delete")]
        public bool DeleteGroup(int groupId)
        {
            return this.expenseGroupService.DeleteGroup(groupId);
        }

        [HttpPost("{groupId}/adduser")]
        public int AddGroupUser(int groupId, ExpenseGroupUser user)
        {
            return this.expenseGroupService.AddGroupUser(groupId, user);
        }
    }
}
