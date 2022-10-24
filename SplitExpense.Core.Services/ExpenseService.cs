using Microsoft.AspNetCore.Http;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services.Core;
using SplitExpense.Core.Services.Extensions;

namespace SplitExpense.Core.Services
{
    public class ExpenseService : BaseService
    {
        private readonly DatabaseContext DB;

        public ExpenseService(DatabaseContext db, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.DB = db;
        }

        public int AddExpense(AddExpense expense)
        {
            int currentUserId = this.ContextUser.Id;
            var expenseUsers = this.GetExpenseUser(expense).ToList();
            if (!expenseUsers.Any())
            {
                throw new Exception("Group has no members");
            }

            if(!expenseUsers.Any(e=> e.UserId == currentUserId))
            {
                expenseUsers.Add(new ExpenseUser()
                {
                    Amount = 0,
                    Balance = 0,
                    UserId = currentUserId
                });
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
                    ExpenseDate = expense.ExpenseDate,
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
                                UserId = user.UserId,
                                Amount = sharePerUser,
                                Balance = sharePerUser,
                            });
                        }

                        break;
                    }

                case ExpenseSplitType.Individual:
                    {
                        expenseUsers.Add(new ExpenseUser()
                        {
                            UserId = 1, // TODO: change implementation here
                            Amount = expense.Amount,
                            Balance = expense.Amount,
                        });
                        break;
                    }
                case ExpenseSplitType.Custom:
                    {
                        expense.ExpenseUsers.ForEach(e =>
                        {
                            expenseUsers.Add(new ExpenseUser()
                            {
                                UserId = e.UserId,
                                Amount = e.Amount,
                                Balance = e.Amount,
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

            var listItem = this.DB.Fetch<GroupExpenseListItem>(";EXEC [GetGroupExpenseListItems] @@GroupId = @0, @@UserId = @1", groupId, userId);
            var expenseUserShares = new List<ExpenseUser>();
            if (listItem.Any())
            {
                var ids = listItem.Where(l=> l.SplitType != ExpenseSplitType.Equally).Select(e => e.ExpenseId).ToCSV();
                if(ids.Any())
                {
                    expenseUserShares = this.DB.Fetch<ExpenseUser>("WHERE ExpenseId IN (" + ids + ") AND IsDeleted = @2", ids, userId, false).ToList();
                    var expenseListItemMap = listItem.ToDictionary(l => l.ExpenseId);
                    expenseUserShares.ForEach(e =>
                    {
                        if (expenseListItemMap.ContainsKey(e.ExpenseId))
                        {
                            expenseListItemMap[e.ExpenseId].UserShares.Add(new ExpenseUserShare()
                            {
                                UserId = e.UserId,
                                Amount = e.Amount
                            });
                        }
                    });
                }
            }

            return listItem;
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

        public bool UpdateGroupExpense(int groupId, int expenseId, AddExpense expense)
        {
            var existingExpense = this.DB.SingleOrDefault<Expense>("WHERE GroupId = @0 AND Id = @1 AND IsDeleted = @2", groupId, expenseId, false);
            if (existingExpense != null)
            {
                var groupUsers = this.DB.Fetch<ExpenseGroupUser>("WHERE GroupId = @0 AND IsDeleted = @1", groupId, false);

                var expenseUsers = this.DB.Fetch<ExpenseUser>("WHERE ExpenseId = @0 AND IsDeleted = @1", expenseId, false);
                var expenseUsersToInsert = new List<ExpenseUser>();
                var expenseUsersToDelete = new List<ExpenseUser>();

                existingExpense.Name = expense.Name;
                existingExpense.Description = expense.Description;
                existingExpense.Amount = expense.Amount;
                existingExpense.SplitType = expense.SplitType;

                if (expense.SplitType == ExpenseSplitType.Equally)
                {
                    var sharePerUser = (expense.Amount / expenseUsers.Count());
                    foreach (var user in expenseUsers)
                    {
                        user.Amount = sharePerUser;
                    }

                    var userToInsert = groupUsers.Where(user => !expenseUsers.Any(eu => eu.UserId == user.UserId));
                    foreach(var user in userToInsert)
                    {
                        expenseUsersToInsert.Add(new ExpenseUser()
                        {
                            Amount = sharePerUser,
                            UserId = user.UserId,
                            ExpenseId = expenseId,
                        }) ;
                    }
                }
                else if(expense.SplitType == ExpenseSplitType.Custom)
                {
                    expenseUsersToDelete = expenseUsers.Where(user => !expense.ExpenseUsers.Any(eu=> eu.UserId == user.UserId)).ToList();
                    expenseUsersToInsert = expense.ExpenseUsers.Where(user => !expenseUsers.Any(eu => eu.UserId == user.UserId))
                                                                .Select(user =>
                                                                {
                                                                   return new ExpenseUser()
                                                                    {
                                                                        ExpenseId = expenseId,
                                                                        Amount = user.Amount,
                                                                        UserId = user.UserId
                                                                    };
                                                                }).ToList();
                    expenseUsers = expenseUsers.Where(user => expense.ExpenseUsers.Any(e => e.UserId == user.UserId)).ToList();
                    var userMap = expense.ExpenseUsers.ToDictionary(user => user.UserId);
                    foreach(var user in expenseUsers)
                    {
                        user.Amount = userMap[user.UserId].Amount;
                    }
                }


                try
                {
                    this.DB.BeginTransaction();
                    this.DB.Update<Expense>(existingExpense, new List<string>() { "Name", "Description", "Amount", "SplitType" });
                    this.DB.BulkUpdate<ExpenseUser>(expenseUsers, new string[] { "Amount", "Balance" });
                    this.DB.BulkInsert<ExpenseUser>(expenseUsersToInsert);
                    if(expenseUsersToDelete.Any())
                    {
                        this.DB.Update<ExpenseUser>("SET IsDeleted = 1 WHERE Id IN (@0)", expenseUsersToDelete.Select(e => e.Id).ToCSV());
                    }

                    this.DB.CompleteTransaction();

                    return true;
                }
                catch (Exception ex)
                {
                    this.DB.AbortTransaction();
                    throw;
                }

            }

            return false;
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

            try
            {
                this.DB.BeginTransaction();
                this.DB.Update<Expense>("SET IsDeleted = @0 WHERE Id = @1 AND GroupId = @2", true, expenseid, groupId);
                this.DB.Update<ExpenseUser>("SET IsDeleted = @0, DateDeleted = @1 WHERE ExpenseId = @2", true, DateTime.UtcNow, expenseid);
                this.DB.CompleteTransaction();

                return true;
            }
            catch (Exception ex)
            {
                this.DB.AbortTransaction();
                throw;
            }
        }
    }
}