using Microsoft.Extensions.Configuration;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
using SplitExpense.Core.Services.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Services
{
    public class UserInviteService
    {
        private readonly DatabaseContext DB;

        private readonly IConfiguration configuration;

        public UserInviteService(DatabaseContext db, IConfiguration configuration)
        {
            this.DB = db;
            this.configuration = configuration;
        }

        public string GetApplicationUserInvite(int userId)
        {
            var invitation = this.DB.FirstOrDefault<UserInvite>("WHERE UserId = @0 AND ReferralType = @1 AND IsDeleted = @2 ", userId, InviteType.App, false);
            if (invitation == null)
            {
                invitation = new UserInvite()
                {
                    ReferralType = InviteType.App,
                    UserId = userId,
                };

                this.DB.Insert(invitation);
            }
                
            return $"{configuration["AppUrl"]}/invite/app/{invitation.ReferralId}";
        }

        public string GetGroupUserInvite(int userId, int groupId)
        {
            if(!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false) || !this.DB.Exists<ExpenseGroupUser>("WHERE GroupId = @0 AND UserId = @1 AND IsDeleted = @2", groupId, userId, false))
            {
                throw new Exception("Unable to generate invite for the group");
            }

            var invitation = this.DB.FirstOrDefault<UserInvite>("WHERE UserId = @0 AND ReferralType = @1 AND GroupId = @2 AND ExpiresOn > @3 AND IsDeleted = @4 ", userId, InviteType.Group, groupId, DateTime.UtcNow.Date, false);
            if (invitation == null)
            {
                invitation = new UserInvite()
                {
                    ReferralType = InviteType.Group,
                    UserId = userId,
                    GroupId = groupId,
                    ExpiresOn = DateTime.UtcNow.AddDays(7).Date
                };

                this.DB.Insert(invitation);
            }

            return $"{configuration["AppUrl"]}/invite/group/{invitation.ReferralId}";
        }

        public bool VerifyAppInvite(string inviteId)
        {
            return this.DB.Exists<UserInvite>("WHERE ReferralId = @0 AND ReferralType = @1 AND IsDeleted = @2", inviteId, InviteType.App, false);
        }

        public bool VerifyGroupInvite(Guid inviteId)
        {
            return this.DB.Exists<UserInvite>("WHERE ReferralId = @0 AND ReferralType = @1 AND ExpiresOn > @2 AND IsDeleted = @3", inviteId, InviteType.Group, DateTime.UtcNow.Date,  false);
        }
    }
}
