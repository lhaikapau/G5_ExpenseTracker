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
        public IActionResult Index(int? year, int? month)
        {
            var allExpenses = _expenseService.RetrieveAll(UserId);
            var totalAmount = allExpenses.Sum(exp => exp.Amount ?? 0);
            ViewData["TotalAmount"] = Math.Round(totalAmount, 2);  // Round to 2 decimal places

            // Calculate average monthly expenses using DateCreated
            var distinctMonths = allExpenses
                .Where(exp => exp.DateCreated != null)
                .Select(exp => new { exp.DateCreated.Value.Year, exp.DateCreated.Value.Month })
                .Distinct()
                .Count();
            var averageMonthlyExpense = distinctMonths > 0 ? totalAmount / distinctMonths : 0;
            ViewData["AverageMonthlyExpense"] = Math.Round(averageMonthlyExpense, 2);  // Round to 2 decimal places

            var categoryData = allExpenses
                .GroupBy(e => e.Name)
                .Select(g => new
                {
                    name = g.Key,
                    amount = g.Sum(e => e.Amount ?? 0),
                    percentage = (g.Sum(e => e.Amount ?? 0) / totalAmount) * 100
                })
                .OrderByDescending(x => x.amount)
                .ToList();

            ViewData["CategoryData"] = JsonSerializer.Serialize(categoryData);

            return View();
        }





    }
}
