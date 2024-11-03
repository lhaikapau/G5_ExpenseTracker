using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IExpenseRepository
    {
        void AddExpense(Expense model);
        IEnumerable<Expense> RetrieveAll();
        void UpdateExpense(Expense model);
        void DeleteExpense(Expense model);

    }
}
