﻿using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Interfaces;
using AutoMapper;
using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace ASI.Basecode.Services.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ICategoryRepository _categoryRepository;

        public ExpenseService(IExpenseRepository expenseRepository, ICategoryRepository categoryRepository, IMapper mapper, IConfiguration configuration)
        {
            _expenseRepository = expenseRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _config = configuration;
        }

        public void AddExpense(ExpenseViewModel model, string userId)
        {
            // Check if required fields are empty
            if (string.IsNullOrWhiteSpace(model.Title) ||
                model.Amount <= 0 ||
                model.CategoryId <= 0)
            {
                throw new ArgumentException("Please fill in all required fields.");
            }

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

        public List<ExpenseViewModel> RetrieveAll(string UserId)
        {
            var categories = _categoryRepository.RetrieveAll().ToDictionary(c => c.CategoryId, c => c.Name);
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _expenseRepository
                .RetrieveAll()
                .Where(c => c.CreatedBy == UserId) // Filter by userId
                .Select(s => new ExpenseViewModel
                {
                    ExpenseId = s.ExpenseId,                  
                    Title = s.Title,
                    Description = s.Description,
                    Amount = s.Amount,
                    CategoryId = s.CategoryId,
                    Name = s.CategoryId.HasValue && categories.ContainsKey(s.CategoryId.Value)
                        ? categories[s.CategoryId.Value]
                        : "Unknown"  // Display "Unknown" if the category doesn't exist

                })
                .ToList();

            return data;
        }

        public ExpenseViewModel RetrieveExpense(int ExpenseId)
        {
            var expense = _expenseRepository.RetrieveAll().Where(x => x.ExpenseId.Equals(ExpenseId)).Select(s => new ExpenseViewModel
            {
                ExpenseId = s.ExpenseId,
                Title = s.Title,
                Description = s.Description,
                Amount = s.Amount,
                CategoryId = s.CategoryId,
                Name = s.CategoryId.HasValue
                   ? _categoryRepository.RetrieveAll()
                       .FirstOrDefault(c => c.CategoryId == s.CategoryId)?.Name
                   : "Unknown"
            }).FirstOrDefault();

            return expense;
        }
        public void UpdateExpense(ExpenseViewModel model, string userId)
        {
            try
            {
                var expense = _expenseRepository.RetrieveAll().Where(x => x.ExpenseId.Equals(model.ExpenseId)).FirstOrDefault();
                if (expense != null)
                {
                    // Retrieve category name based on CategoryId
                    var categories = model.CategoryId.HasValue
                        ? _categoryRepository.RetrieveAll().FirstOrDefault(c => c.CategoryId == model.CategoryId)?.Name
                        : null;

                    expense.Title = model.Title;
                    expense.Description = model.Description;
                    expense.Amount = model.Amount;
                    expense.CategoryId = model.CategoryId;
                    expense.DateUpdated = DateTime.Now;
                    

                    _expenseRepository.UpdateExpense(expense);
                } 


            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error updating expense: {ex.Message}");
                throw; // Optionally rethrow the exception
            }
        }
        public void DeleteExpense(int ExpenseId)
        {
            var expense = _expenseRepository.RetrieveAll().Where(x => x.ExpenseId.Equals(ExpenseId)).FirstOrDefault();
            if (expense != null)
            {
                _expenseRepository.DeleteExpense(expense);
            }
        }

    }
}
