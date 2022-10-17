using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.ViewModels
{
    public class ExpenseGroupUserBalanceListItem
    {
        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        private double DebtAmount { get; set; }

        private double LentAmount { get; set; }

        public double Balance
        {
            get
            {
                return Math.Abs(this.DebtAmount - this.LentAmount);
            }
        }

        public bool IsInDebt
        {
            get
            {
                return this.DebtAmount > this.LentAmount;
            }
        }
    }
}
