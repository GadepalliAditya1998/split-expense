namespace SplitExpense.Core.Models.ViewModels
{
    public class UserConnectionListItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public bool IsReferred { get; set; }

        public DateTime? JoinByReferralOn { get; set; }
    }
}
