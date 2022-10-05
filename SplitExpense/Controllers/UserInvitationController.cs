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
        public string GetUserApplicationInviteURL()
        {
            return this.userInviteService.GetApplicationUserInvite(this.ContextUser.Id);
        }

        [HttpGet("group/{groupId}")]
        public string GetUserGroupInviteURL(int groupId)
        {
            return this.userInviteService.GetGroupUserInvite(this.ContextUser.Id, groupId);
        }

        [HttpGet("app/{inviteId}/verify")]
        public bool VerifyAppInvite(string inviteId)
        {
            return this.userInviteService.VerifyAppInvite(inviteId);
        }

        [HttpGet("group/{inviteId}/verify")]
        public bool VerifyGroupInvite(string inviteId)
        {
            return this.userInviteService.VerifyGroupInvite(inviteId);
        }
    }
}
