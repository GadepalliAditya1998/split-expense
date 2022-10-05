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

        public int Userid { get; set; }

        public string Name { get; set; }

        public double DebtAmount { get; set; }

        public double LentAmount { get; set; }
    }
}
