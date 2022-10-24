using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
using SplitExpense.Core.Models.ViewModels;
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

        [HttpGet("list")]
        public IEnumerable<ExpenseUserGroupListItem> GetExpenseGroups()
        {
            return this.expenseGroupService.GetUserGroups(this.ContextUser.Id);
        }

        [HttpPost("add")]
        public int AddGroup(ExpenseGroup expenseGroup)
        {
            expenseGroup.UserId = this.ContextUser.Id;
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

        [HttpGet("{groupId}/members")]
        public IEnumerable<ExpenseGroupUserListItem> AddGroupUser(int groupId)
        {
            return this.expenseGroupService.GetGroupUsers(groupId);
        }

        [HttpDelete("{groupId}/user/{userId}")]
        public dynamic DeleteGroupUser(int groupId, int userId)
        {
            return new { IsDeleted = this.expenseGroupService.DeleteGroupUser(groupId, userId) };
        }

        [HttpPost("{groupId}/recordPayment")]
        public PaymentTransaction RecordPaymentTransaction(int groupId, AddPaymentTransaction addPaymentTransaction)
        {
            return this.expenseGroupService.RecordGroupExpensePayment(groupId, addPaymentTransaction);
        }

        [HttpGet("{groupId}/payments")]
        public IEnumerable<PaymentTransactionListItem> GetGroupPaymentTransactions(int groupId)
        {
            return this.expenseGroupService.GetGroupPaymentTransactions(groupId);
        }
    }
}
