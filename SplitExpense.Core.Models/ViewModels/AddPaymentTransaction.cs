using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.ViewModels
{
    public class AddPaymentTransaction
    {
        public int GroupId { get; set; }

        public int PaidByUserId { get; set; }

        public int PaidToUserId { get; set; }

        public double Amount { get; set; }

        public PaymentMode PaymentMode { get; set; }
    }
}
