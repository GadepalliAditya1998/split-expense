using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.Core
{
    public class ExpenseUser
    {
        public int Id { get; set; }

        public int ExpenseId { get; set; }

        public int UserId { get; set; }

        public Double Amount { get; set; }

        public double Balance { get; set; }
    }
}
