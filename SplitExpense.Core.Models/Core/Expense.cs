namespace SplitExpense.Core.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public int? GroupId { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }
    }
}