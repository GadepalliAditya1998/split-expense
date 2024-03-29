﻿using Microsoft.AspNetCore.Http;
using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
using SplitExpense.Core.Models.ViewModels;
using SplitExpense.Core.Services.Core;
using SplitExpense.Core.Services.Core.Notifications.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Services
{
    public class ExpenseGroupService : BaseService
    {
        private readonly DatabaseContext DB;

        private readonly EventManager eventManager;

        public ExpenseGroupService(DatabaseContext db, EventManager eventManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            this.DB = db;
            this.eventManager = eventManager;
        }

        public IEnumerable<ExpenseUserGroupListItem> GetUserGroups(int userId)
        {
            return this.DB.Fetch<ExpenseUserGroupListItem>("WHERE UserId = @0", userId);
        }

        public int AddGroup(ExpenseGroup expenseGroup)
        {
            var expenseGroupUser = new ExpenseGroupUser()
            {
                UserId = expenseGroup.UserId,
                IsAdmin = true
            };

            try
            {
                this.DB.BeginTransaction();

                int id = this.DB.Insert(expenseGroup);
                expenseGroupUser.GroupId = id;
                this.DB.Insert(expenseGroupUser);

                this.DB.CompleteTransaction();

                return id;
            }
            catch (Exception ex)
            {
                this.DB.AbortTransaction();
                throw new Exception("Something went wrong");
            }
        }

        public bool UpdateGroup(int groupId, ExpenseGroup expenseGroup)
        {
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Group doesn't exists");
            }

            expenseGroup.Id = groupId;
            return this.DB.Update(expenseGroup, new List<string>() { "Name", "Description" }) > 0;
        }

        public bool DeleteGroup(int groupId)
        {
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Group doesn't exists");
            }

            return this.DB.Update<ExpenseGroup>("SET IsDeleted = @0, DateDeleted = @1 WHERE Id = @2", true, DateTime.UtcNow, groupId) > 0;
        }

        public IEnumerable<ExpenseGroupUserListItem> GetGroupUsers(int groupId)
        {
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Expense Group doesn't exists");
            }

            return this.DB.Fetch<ExpenseGroupUserListItem>("WHERE GroupId = @0", groupId);
        }

        public int AddGroupUser(int groupId, ExpenseGroupUser groupUser)
        {
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Expense Group doesn't exists");
            }

            if (this.DB.Exists<ExpenseGroupUser>("WHERE GroupId = @0 AND IsDeleted = @1 AND UserId = @2", groupId, false, groupUser.UserId))
            {
                throw new Exception("User already exists");
            }

            groupUser.GroupId = groupId;

            var groupUserId = this.DB.Insert(groupUser);
            this.eventManager.TriggerUserAddedToGroupNotification(groupId, new List<int>() { groupUser.UserId });

            return groupUserId;
        }

        public bool DeleteGroupUser(int groupId, int userId)
        {
            int currentUser = this.ContextUser.Id;
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Expense Group doesn't exists");
            }

            if (!this.DB.Exists<ExpenseGroupUser>("WHERE GroupId = @0 AND IsDeleted = @1 AND UserId = @2 AND IsAdmin = @3", groupId, false, currentUser, true))
            {
                throw new Exception("You don't have privileges to remove a user");
            }

            return this.DB.Update<ExpenseGroupUser>("SET IsDeleted = @0 WHERE GroupId = @1 AND UserId = @2", true, groupId, userId) > 0;
        }

        public PaymentTransaction RecordGroupExpensePayment(int groupId, AddPaymentTransaction addPaymentTransaction)
        {
            if (!this.DB.Exists<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false))
            {
                throw new Exception("Group doesn't exists");
            }

            var expenseGroupUserIds = this.DB.Fetch<int>("SELECT UserId FROM [ExpenseGroupUsers] WHERE GroupId = @0 AND IsDeleted = @1", groupId, false);
            if (!(expenseGroupUserIds.Any(e => e == addPaymentTransaction.PaidByUserId) && expenseGroupUserIds.Any(e => e == addPaymentTransaction.PaidToUserId)))
            {
                throw new Exception("User doesn't exists");
            }

            var paymentTransaction = new PaymentTransaction()
            {
                PaymentDate = DateTime.UtcNow,
                TransactionIdentifier = Guid.NewGuid(),
                PaidByUserId = addPaymentTransaction.PaidByUserId,
                PaidToUserId = addPaymentTransaction.PaidToUserId,
                PaymentMode = addPaymentTransaction.PaymentMode,
                Amount = addPaymentTransaction.Amount,
                GroupId = groupId,
            };

            var id = this.DB.Insert(paymentTransaction);
            paymentTransaction.Id = id;

            return paymentTransaction;
        }

        public IEnumerable<PaymentTransactionListItem> GetGroupPaymentTransactions(int groupId)
        {
            return this.DB.Fetch<PaymentTransactionListItem>("WHERE GroupId = @0", groupId);
        }
    }
}
