using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.ViewModels
{
    public class GroupExpenseListItem
    {
        public GroupExpenseListItem()
        {
            this.Name = string.Empty;
            this.PaidByName = string.Empty;
            this.UserShares = new List<ExpenseUserShare>();
        }

        public int ExpenseId { get; set; }

        public int GroupId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime ExpenseDate { get; set; }

        public ExpenseSplitType SplitType { get; set; }

        public int PaidBy { get; set; }

        public string PaidByName { get; set; }

        public double PaidAmount { get; set; }

        public double ToBePaidAmount { get; set; }

        public bool IsLent { get; set; }

        public IList<ExpenseUserShare> UserShares { get; set; }
    }
}
