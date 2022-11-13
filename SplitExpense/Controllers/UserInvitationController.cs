using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitExpense.Core.Services;

namespace SplitExpense.Controllers
{
    [Route("/api/invites/")]
    public class UserInvitationController: BaseApiController
    {
        private readonly UserInviteService userInviteService;

        public UserInvitationController(UserInviteService userInviteService)
        {
            this.userInviteService = userInviteService;
        }

        [HttpGet("app")]
        public dynamic GetUserApplicationInviteURL()
        {
            return new { InviteLink = this.userInviteService.GetApplicationUserInvite(this.ContextUser.Id) };
        }

        [HttpGet("group/{groupId}")]
        public dynamic GetUserGroupInviteURL(int groupId)
        {
            return new { InviteLink = this.userInviteService.GetGroupUserInvite(this.ContextUser.Id, groupId) };
        }

        [AllowAnonymous]
        [HttpGet("app/{inviteId}/verify")]
        public bool VerifyAppInvite(string inviteId)
        {
            return this.userInviteService.VerifyAppInvite(inviteId);
        }

        [HttpGet("group/{inviteId}/verify")]
        public bool VerifyGroupInvite(string inviteId)
        {
            return this.userInviteService.VerifyGroupInvite(Guid.Parse(inviteId));
        }
    }
}
