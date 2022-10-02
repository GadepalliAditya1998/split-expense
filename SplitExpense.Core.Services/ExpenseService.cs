using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services.Core;

namespace SplitExpense.Core.Services
{
    public class ExpenseService
    {
        private readonly DatabaseContext DB;

        public ExpenseService(DatabaseContext db)
        {
            this.DB = db;
        }

        public int AddExpense(AddExpense expense)
        {
            return this.DB.Insert(expense);
        }
    }
}