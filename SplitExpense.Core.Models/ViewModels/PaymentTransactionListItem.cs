using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.ViewModels
{
    public class PaymentTransactionListItem
    {
        public int Id { get; set; }

        public Guid TransactionIdentifier { get; set; }

        public int PaidByUserId { get; set; }

        public string PaidByName { get; set; }

        public int PaidToUserId { get; set; }

        public string PaidToName { get; set; }

        public double Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentMode PaymentMode { get; set; }
    }
}
