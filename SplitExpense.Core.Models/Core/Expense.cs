namespace SplitExpense.Core.Models
{
    public class Expense
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? GroupId { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public int PaidByUser { get; set; }

        public double Amount { get; set; }

        public ExpenseSplitType SplitType { get; set; }
    }
}