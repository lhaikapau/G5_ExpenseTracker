using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IExpenseService
    {
        void AddExpense(ExpenseViewModel model, string userId);

        List<ExpenseViewModel> RetrieveAll(string UserId);
        ExpenseViewModel RetrieveExpense(int ExpenseId);
        void UpdateExpense(ExpenseViewModel model, string userId);

        void DeleteExpense(int ExpenseId);
        List<ExpenseViewModel> RetrieveByMonth(string userId, int year, int month);
    }
}
