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
    }
}
