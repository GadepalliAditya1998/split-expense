namespace SplitExpense.Core.Models.Core.Database
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Guid? ReferralId { get; set; }

        public bool IsActive { get; set; }
    }
}
