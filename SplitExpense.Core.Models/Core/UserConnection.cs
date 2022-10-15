using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.Core
{
    public class UserConnection
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ConnectedUserId { get; set; }
    }
}
