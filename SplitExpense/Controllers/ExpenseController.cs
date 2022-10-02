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

        [HttpPost("{expenseId}/delete")]
        public bool DeleteIndividualExpense(int expenseId)
        {
            return this.expenseService.DeleteIndividualExpense(expenseId);
        }

        [HttpPost("groups/{groupId}/{expenseId}/delete")]
        public bool DeleteGroupExpense(int groupId, int expenseId)
        {
            return this.expenseService.DeleteGroupExpense(groupId, expenseId);
        }
    }
}