using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.ViewModels
{
    public class AddExpense
    {
        public AddExpense()
        {
            this.ExpenseUsers = new List<ExpenseUserShare>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? GroupId { get; set; }

        public string Description { get; set; }

        public ExpenseSplitType SplitType { get; set; }

        public double Amount { get; set; }

        public List<ExpenseUserShare> ExpenseUsers { get; set; }
    }
}
