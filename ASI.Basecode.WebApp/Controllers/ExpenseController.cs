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
using System.Drawing.Printing;
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

        public IActionResult Index(int pageNumber = 1, int pageSize = 5)
        {
            BaseExpenseViewModel viewModel = new BaseExpenseViewModel();
            var data = _expenseService.RetrieveAll(UserId, pageNumber, pageSize);
            var totalExpenses = _expenseService.GetTotalExpenseCount(UserId);

            var totalPages = (int)Math.Ceiling((double)totalExpenses / pageSize);
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            viewModel.ListExpenseViewModel = data;
            viewModel.EntryForm = new ExpenseViewModel();

            return View(viewModel);
        }


        #region Get Methods
        [HttpGet]

        public IActionResult Create(int pageNumber = 1, int pageSize = 5)
        {
            BaseExpenseViewModel viewModel = new BaseExpenseViewModel();
            var data = _expenseService.RetrieveAll(UserId, pageNumber, pageSize);
            var totalExpenses = _expenseService.GetTotalExpenseCount(UserId);

            var totalPages = (int)Math.Ceiling((double)totalExpenses / pageSize);
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            viewModel.ListExpenseViewModel = data;
            viewModel.EntryForm = new ExpenseViewModel();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int ExpenseId, int pageNumber = 1, int pageSize = 5)
        {
            BaseExpenseViewModel viewModel = new BaseExpenseViewModel();
            var data = _expenseService.RetrieveAll(UserId, pageNumber, pageSize);
            var totalExpenses = _expenseService.GetTotalExpenseCount(UserId);

            var totalPages = (int)Math.Ceiling((double)totalExpenses / pageSize);
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            
            var existingExpense = _expenseService.RetrieveExpense(ExpenseId);
            viewModel.ListExpenseViewModel = data;
            viewModel.EntryForm = existingExpense;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int ExpenseId)
        {
            var data = _expenseService.RetrieveExpense(ExpenseId);
            return View(data);
        }

        [HttpGet]
        public IActionResult Search(string term, int pageNumber = 1)
        {
            try
            {
                int pageSize = 5;
                var results = _expenseService.FilterByTitle(UserId, term, pageNumber, pageSize);
                return Json(results);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        #endregion

        #region Post Methods
        [HttpPost]

        public IActionResult Create(ExpenseViewModel model)
        {
            BaseExpenseViewModel viewModel = new BaseExpenseViewModel();
            try
            {
                _expenseService.AddExpense(model, UserId);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                viewModel.EntryForm = model;
                return View("Index", viewModel); // Return the view with validation errors
            }
        }

        [HttpPost]
        public IActionResult Edit(ExpenseViewModel model, int pageNumber = 1, int pageSize = 5)
        {
            BaseExpenseViewModel viewModel = new BaseExpenseViewModel();

            if (!ModelState.IsValid)
            {
                // Log the validation errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                var data = _expenseService.RetrieveAll(UserId, pageNumber, pageSize);
                var totalExpenses = _expenseService.GetTotalExpenseCount(UserId);

                var totalPages = (int)Math.Ceiling((double)totalExpenses / pageSize);
                ViewData["CurrentPage"] = pageNumber;
                ViewData["TotalPages"] = totalPages;
                viewModel.ListExpenseViewModel = data;
                viewModel.EntryForm = model;
                return View("Index", viewModel); // Return the model with validation errors
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
                var data = _expenseService.RetrieveAll(UserId, pageNumber, pageSize);
                var totalExpenses = _expenseService.GetTotalExpenseCount(UserId);

                var totalPages = (int)Math.Ceiling((double)totalExpenses / pageSize);
                ViewData["CurrentPage"] = pageNumber;
                ViewData["TotalPages"] = totalPages;
                viewModel.ListExpenseViewModel = data;
                viewModel.EntryForm = model;
                return View("Index", viewModel);
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
