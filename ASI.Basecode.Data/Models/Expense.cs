﻿using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Expense
    {
        public int ExpenseId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public double? Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string Description { get; set; }
    }
}
