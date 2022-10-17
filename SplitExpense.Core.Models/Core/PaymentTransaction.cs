using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.Core
{
    public class PaymentTransaction
    {
        public int Id { get; set; }

        public Guid TransactionIdentifier { get; set; }

        public int GroupId { get; set; }

        public int PaidByUserId { get; set; }

        public int PaidToUserId { get; set; }

        public double Amount { get; set; }

        public PaymentMode PaymentMode { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
