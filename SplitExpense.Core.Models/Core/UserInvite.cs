using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.Core
{
    public class UserInvite
    {
        public UserInvite()
        {
            this.ReferralId = Guid.NewGuid();
        }

        public int Id  { get; set; }

        public Guid ReferralId { get; set; }

        public int UserId { get; set; }

        public InviteType ReferralType { get; set; }

        public int? GroupId { get; set; }

        public DateTime? ExpiresOn { get; set; }
    }
}
