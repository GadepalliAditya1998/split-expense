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

        private double TotalOwingAmount { get; set; }

        private double TotalLentAmount { get; set; }

        private double PaidAmount { get; set; }

        private double AmountReturned { get; set; }

        private bool isInDebt;

        public double Balance
        {
            get
            {
                var balance = (TotalLentAmount + PaidAmount) - (TotalOwingAmount + AmountReturned);
                this.isInDebt = balance < 0;
                return Math.Abs(balance);
            }
        }


        public bool IsInDebt
        {
            get
            {
                return isInDebt;
            }
        }
    }
}
