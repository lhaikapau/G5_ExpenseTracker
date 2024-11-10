using System;
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

        [Required(ErrorMessage = "Required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required.")]
        public double? Amount { get; set; }

        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Required.")]
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
