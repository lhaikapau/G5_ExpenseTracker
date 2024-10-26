using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class ExpenseController : ControllerBase<ExpenseController>
    {
        private readonly IExpenseService _expenseService;
        /// Constructor
        /// </summary>       
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>        
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public ExpenseController(IExpenseService expenseService,
                                  IHttpContextAccessor httpContextAccessor,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration,
                                  IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _expenseService = expenseService;
        }

        public IActionResult Index()
        {
            var data = _expenseService.RetrieveAll(UserId);
            return View(data);
        }

        #region Get Methods
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int ExpenseId)
        {
            var data = _expenseService.RetrieveExpense(ExpenseId);
            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int ExpenseId)
        {
            var data = _expenseService.RetrieveExpense(ExpenseId);
            return View(data);
        }


        #endregion

        #region Post Methods
        [HttpPost]

        public IActionResult Create(ExpenseViewModel model)
        {
            try
            {
                _expenseService.AddExpense(model, UserId);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model); // Return the view with validation errors
            }
        }

        [HttpPost]
        public IActionResult Edit(ExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Log the validation errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model); // Return the model with validation errors
            }

            try
            {
                _expenseService.UpdateExpense(model, UserId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Unable to update expense. Please try again.");
                // Log the exception
                Console.WriteLine(ex.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PostDelete(int ExpenseId)
        {
            _expenseService.DeleteExpense(ExpenseId);
            return RedirectToAction("Index");
        }

        #endregion
    }
}
