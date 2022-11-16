using SplitExpense.Core.Models;
using SplitExpense.Core.Models.Core;
using SplitExpense.Core.Models.Core.Notifications;
using SplitExpense.Core.Models.ViewModels.Notifications;
using SplitExpense.Core.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Services.Core.Notifications.Events
{
    public partial class EventManager
    {
        public async void TriggerUserAddedToGroupNotification(int groupId, List<int> userIds)
        {
            var group = this.DB.SingleOrDefault<ExpenseGroup>("WHERE Id = @0 AND IsDeleted = @1", groupId, false);
            if(group == null)
            {
                return;
            }

            var existingGroupUsers = this.DB.Fetch<ExpenseGroupUser>("WHERE GroupId = @0 AND UserId NOT IN (@1) AND IsDeleted = @2", groupId, userIds.ToCSV() ,false);
            if(existingGroupUsers.Any())
            {
                var allUserIds = existingGroupUsers.Select(e => e.UserId).Union(userIds);

                var users = this.DB.Fetch<UserEmailListItem>("WHERE Id IN ("+ allUserIds.ToCSV() + ")");
                if(users.Any())
                {
                    var existingUserMap = existingGroupUsers.Select(e => e.UserId).ToHashSet<int>();

                    var newUsers = users.Where(user => userIds.Contains(user.Id)).ToList();
                    var existingUsers = users.Where(user => existingUserMap.Contains(user.Id)).ToList();

                    var emailNotifications = new List<EmailNotification>();
                    if(existingUsers.Any())
                    {
                        emailNotifications.Add(new EmailNotification()
                        {
                            To = existingUsers.Select(u => u.Email).ToList(),
                            Subject = "User added to Expense Group",
                            Body = String.Format("{0} are added to the expense group: {1}", newUsers.Select(u=>u.Name).ToCSV(), group.Name),
                        });
                    }

                    if(newUsers.Any())
                    {
                        emailNotifications.Add(new EmailNotification()
                        {
                            To = newUsers.Select(u => u.Email).ToList(),
                            Subject = "Expense Group Added",
                            Body = $"You are added to the group {group.Name}",
                        });
                    }

                    this.NotificationService.TriggerNotifications(emailNotifications);
                }
            }
        }
    }
}
