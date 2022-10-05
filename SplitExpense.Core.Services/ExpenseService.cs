using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services.Core;

namespace SplitExpense.Core.Services
{
    public class ExpenseService
    {
        private readonly DatabaseContext DB;

        public ExpenseService(DatabaseContext db)
        {
            this.DB = db;
        }

        public int AddExpense(AddExpense expense)
        {
            int currentUserId = 1;
            var expenseUsers = this.GetExpenseUser(expense).ToList();
            if (!expenseUsers.Any())
            {
                throw new Exception("Group has no members");
            }

            try
            {
                this.DB.BeginTransaction();

                var newExpense = new Expense()
                {
                    Name = expense.Name,
                    Amount = expense.Amount,
                    Description = expense.Description,
                    GroupId = expense.GroupId,
                    SplitType = expense.SplitType,
                    PaidByUser = currentUserId,
                    UserId = currentUserId,
                };

                int expenseId = this.DB.Insert(newExpense);
                expenseUsers.ForEach(e =>
                {
                    e.ExpenseId = expenseId;
                });
                this.DB.BulkInsert(expenseUsers);

                this.DB.CompleteTransaction();

                return expenseId;
            }
            catch (Exception e)
            {
                this.DB.AbortTransaction();
                throw new Exception("Unable to add expense");
            }
        }

        private IList<ExpenseUser> GetExpenseUser(AddExpense expense)
        {
            List<ExpenseUser> expenseUsers = new List<ExpenseUser>();
            switch (expense.SplitType)
            {
                case ExpenseSplitType.Equally:
                    {
                        if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", expense.GroupId.Value, false))
                        {
                            throw new Exception("Group doesn't exists");
                        }

                        var groupUsers = this.DB.Fetch<ExpenseGroupUser>("WHERE GroupId = @0 AND IsDeleted = @1", expense.GroupId.Value, false);
                        var sharePerUser = expense.Amount / groupUsers.Count();

                        foreach (var user in groupUsers)
                        {
                            expenseUsers.Add(new ExpenseUser()
                            {
                                Amount = sharePerUser,
                            });
                        }

                        break;
                    }

                case ExpenseSplitType.Individual:
                    {
                        expenseUsers.Add(new ExpenseUser()
                        {
                            Amount = expense.Amount
                        });
                        break;
                    }
                case ExpenseSplitType.Custom:
                    {
                        expense.ExpenseUsers.ForEach(e =>
                        {
                            expenseUsers.Add(new ExpenseUser()
                            {
                                Amount = e.Amount
                            });
                        });
                        break;
                    }
            }

            return expenseUsers;
        }

        public IEnumerable<GroupExpenseListItem> GetGroupUserExpenseListItem(int groupId, int userId)
        {
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Group doesn't exists");
            }

            return this.DB.Fetch<GroupExpenseListItem>(";EXEC [GetGroupExpenseListItems] @@GroupId = @0, @@UserId = @1", groupId, userId);
        }

        public IEnumerable<ExpenseGroupUserBalanceListItem> GetGroupUserWiseBalances(int groupId, int userId)
        {
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Group doesn't exists");
            }

            return this.DB.Fetch<ExpenseGroupUserBalanceListItem>(";EXEC [GetGroupUserWiseBalances] @@GroupId = @0, @@UserId = @1", groupId, userId);
        }

        public IEnumerable<Expense> GetExpenses()
        {
            return this.DB.Fetch<Expense>(string.Empty);
        }

        public bool DeleteIndividualExpense(int id)
        {
            if (!this.DB.Exists<Expense>("WHERE Id = @0 AND GroupId IS NULL AND IsDeleted = @1", id, false))
            {
                throw new Exception("Expense doesn't exists");
            }

            return (this.DB.Update<Expense>("SET IsDeleted = @0 WHERE Id = @1 AND GroupId IS NULL", true, id)) > 0;
        }

        public bool DeleteGroupExpense(int groupId, int expenseid)
        {
            if (!this.DB.Exists<Expense>("WHERE Id = @0 AND GroupId = @1 AND IsDeleted = @2", expenseid, groupId, false))
            {
                throw new Exception("Expense doesn't exists");
            }

            return (this.DB.Update<Expense>("SET IsDeleted = @0 WHERE Id = @1 AND GroupId = @2", true, expenseid, groupId)) > 0;
        }
    }
}