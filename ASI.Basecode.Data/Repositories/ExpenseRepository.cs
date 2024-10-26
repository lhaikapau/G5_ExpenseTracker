using ASI.Basecode.Data.Interfaces;
using Basecode.Data.Repositories;
using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class ExpenseRepository : BaseRepository, IExpenseRepository
    {
        public ExpenseRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { } 

        public void AddExpense(Expense model)
        {
            this.GetDbSet<Expense>().Add(model);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Expense> RetrieveAll()
        {
            return this.GetDbSet<Expense>();
        }
        public void UpdateExpense(Expense model)
        {
            this.GetDbSet<Expense>().Update(model);
            UnitOfWork.SaveChanges();
        }
        public void DeleteExpense(Expense model)
        {
            this.GetDbSet<Expense>().Remove(model);
            UnitOfWork.SaveChanges();
        }
    }
}
