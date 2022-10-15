using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services;

namespace SplitExpense
{
    [Route("/api/expenses/")]
    public class ExpenseController : BaseApiController
    {
        private readonly ExpenseService expenseService;

        public ExpenseController(ExpenseService expenseService)
        {
            this.expenseService = expenseService;
        }

        [HttpGet("all")]
        public IEnumerable<Expense> GetExpenses()
        {
            return this.expenseService.GetExpenses();
        }

        [HttpPost("add")]
        public int AddExpense(AddExpense expense)
        {
            return this.expenseService.AddExpense(expense);
        }

        [HttpGet("group/{groupId}/list")]
        public IEnumerable<GroupExpenseListItem> GetGroupExpenses(int groupId)
        {
            return this.expenseService.GetGroupUserExpenseListItem(groupId, this.ContextUser.Id);
        }

        [HttpGet("group/{groupId}/balances")]
        public IEnumerable<ExpenseGroupUserBalanceListItem> GetGroupUserWiseBalances(int groupId)
        {
            return this.expenseService.GetGroupUserWiseBalances(groupId, this.ContextUser.Id);
        }

        [HttpDelete("{expenseId}/delete")]
        public bool DeleteIndividualExpense(int expenseId)
        {
            return this.expenseService.DeleteIndividualExpense(expenseId);
        }

        [HttpDelete("groups/{groupId}/{expenseId}/delete")]
        public bool DeleteGroupExpense(int groupId, int expenseId)
        {
            return this.expenseService.DeleteGroupExpense(groupId, expenseId);
        }
    }
}