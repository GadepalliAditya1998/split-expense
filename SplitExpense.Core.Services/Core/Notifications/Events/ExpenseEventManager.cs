using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Services.Core.Notifications.Events
{
    public partial class EventManager
    {
        public DatabaseContext DB { get; set; }

        public NotificationService NotificationService { get; set; }

        public EventManager(DatabaseContext database, NotificationService notificationService)
        {
            this.DB = database;
            this.NotificationService = notificationService;
        }

        public async void TriggerExpenseAddedNotification()
        {

        }
    }
}
