using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Interfaces;
using AutoMapper;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Services.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public void AddExpense(ExpenseViewModel model, string userId)
        {
            var newExpense = new Expense();
            newExpense.CategoryId = model.CategoryId;
            newExpense.Title = model.Title;
            newExpense.Amount = model.Amount;
            newExpense.CreatedBy = userId;
            newExpense.DateCreated = DateTime.Now; 
            newExpense.DateUpdated = DateTime.Now;
            newExpense.Description = model.Description;
            _expenseRepository.AddExpense(newExpense);
        }
    }
}
