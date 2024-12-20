﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ExpenseViewModel
    {
        public int ExpenseId { get; set; }
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Required Expense Name.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Required Amount input.")]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative number.")]
        public double? Amount { get; set; }

        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Required Date.")]
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }

        [Required(ErrorMessage = "Required Expense Description.")]
        public string Description { get; set; }

        public string Name { get; set; }
    }
}
