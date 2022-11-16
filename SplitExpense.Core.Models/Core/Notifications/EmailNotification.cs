using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.Core.Notifications
{
    public class EmailNotification
    {
        public EmailNotification()
        {
            this.To = new List<string>();
            this.CC = new List<string>();
        }

        public List<string> To { get; set; }

        public List<string> CC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsHTML { get; set; }
    }
}
