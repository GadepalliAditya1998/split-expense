using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.ViewModels
{
    public class ExpenseGroupUserListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GroupId { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }
    }
}
