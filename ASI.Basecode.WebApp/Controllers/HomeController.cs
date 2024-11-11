using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Globalization;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    public class HomeController : ControllerBase<HomeController>
    {
        private readonly IExpenseService _expenseService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public HomeController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IMapper mapper,
                              IExpenseService expenseService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>
        // Controller
        public IActionResult Index()
        {
            try
            {
                var allExpenses = _expenseService.RetrieveAll(UserId, pageSize: int.MaxValue).ToList();


                // Calculate total amount using decimal for better precision
                decimal totalAmount = allExpenses.Sum(exp => (decimal)(exp.Amount ?? 0.0));
                ViewData["TotalAmount"] = decimal.Round(totalAmount, 2);

                // Calculate average monthly expense
                var expensesByMonth = allExpenses
                    .Where(exp => exp.DateCreated.HasValue)
                    .GroupBy(exp => new { exp.DateCreated.Value.Year, exp.DateCreated.Value.Month })
                    .ToList();

                var distinctMonths = expensesByMonth.Count;
                decimal averageMonthlyExpense = distinctMonths > 0 ? totalAmount / distinctMonths : 0;
                ViewData["AverageMonthlyExpense"] = decimal.Round(averageMonthlyExpense, 2);

                // Group by category - following the same pattern as your Report controller
                var categoryData = allExpenses
                    .GroupBy(e => e.Name)
                    .Select(g => new
                    {
                        name = g.Key,
                        amount = (decimal)g.Sum(e => e.Amount ?? 0.0),
                        percentage = totalAmount > 0
                            ? decimal.Round((decimal)g.Sum(e => e.Amount ?? 0.0) / totalAmount * 100M, 1)
                            : 0M
                    })
                    .OrderByDescending(x => x.amount)
                    .ToList();

                // Serialize using the same method as the Report controller
                ViewData["CategoryData"] = JsonSerializer.Serialize(categoryData);

                return View();
            }
            catch (Exception ex)
            {
                // Log the error
                return View("Error");
            }
        }





    }
}
