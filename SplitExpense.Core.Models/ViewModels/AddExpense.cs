﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitExpense.Core.Models.ViewModels
{
    public class AddExpense
    {
        public int Id { get; set; }

        public int Name { get; set; }

        public int? GroupId { get; set; }

        public string Description { get; set; }
    }
}