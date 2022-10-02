using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.Core
{
    public class ExpenseGroupUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public bool IsAdmin { get; set; }
    }
}
