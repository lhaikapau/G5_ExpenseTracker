using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class BaseExpenseViewModel
    {
        public ExpenseViewModel EntryForm { get; set; }
        public List<ExpenseViewModel> ListExpenseViewModel { get; set; }
    }
}
