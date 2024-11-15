using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Text.Json;
using System.Globalization;

namespace ASI.Basecode.WebApp.Controllers
{
    public class ReportController : ControllerBase<ReportController>
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
        /// 

        public ReportController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IMapper mapper,
                              IExpenseService expenseService) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _expenseService = expenseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Trend(int? year, int? month)
        {
            year = year ?? DateTime.Now.Year;
            month = month ?? 0; // 0 means no filter, so display all months

            // Retrieve yearly data with all months and categories
            var yearlyCategoryData = Enumerable.Range(1, 12).Select(monthIndex =>
            {
                var monthlyExpenses = _expenseService.RetrieveByMonth(UserId, year.Value, monthIndex);
                return new
                {
                    month = monthIndex,
                    monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthIndex),
                    categories = monthlyExpenses
                        .GroupBy(e => e.Name)
                        .Select(g => new { name = g.Key, amount = g.Sum(e => e.Amount ?? 0) })
                        .ToList()
                };
            }).ToList();

            // Filter data based on selected month (if any)
            if (month > 0)
            {
                yearlyCategoryData = yearlyCategoryData.Where(data => data.month == month).ToList();
            }

            ViewData["YearlyCategoryData"] = JsonSerializer.Serialize(yearlyCategoryData);
            ViewData["CurrentYear"] = year;
            ViewData["CurrentMonth"] = month;

            return View();
        }


        [HttpGet]
        public IActionResult MonthVsMonth(int? year, int? month, int? compareMonth)
        {
            var totalAmount = _expenseService.RetrieveAll(UserId).Sum(exp => exp.Amount ?? 0);
            ViewData["TotalAmount"] = totalAmount;


            var categoryData = _expenseService.RetrieveAll(UserId)
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

            // Handle Monthly Report logic
            year = year ?? DateTime.Now.Year;
            month = month ?? DateTime.Now.Month;
            compareMonth = compareMonth ?? ((month == 1) ? 12 : month - 1);

            var monthlyExpenses = _expenseService.RetrieveByMonth(UserId, year.Value, month.Value);
            var compareMonthlyExpenses = _expenseService.RetrieveByMonth(UserId,
                compareMonth == 12 && month == 1 ? year.Value - 1 : year.Value,
                compareMonth.Value);

            var monthlyTotalAmount = monthlyExpenses.Sum(exp => exp.Amount ?? 0);
            var compareMonthlyTotalAmount = compareMonthlyExpenses.Sum(exp => exp.Amount ?? 0);

            ViewData["MonthlyTotalAmount"] = monthlyTotalAmount;
            ViewData["CompareMonthlyTotalAmount"] = compareMonthlyTotalAmount;
            ViewData["CurrentYear"] = year;
            ViewData["CurrentMonth"] = month;
            ViewData["CompareMonth"] = compareMonth;

            // Get all unique category names from both months
            var allCategories = monthlyExpenses.Select(e => e.Name)
                .Union(compareMonthlyExpenses.Select(e => e.Name))
                .Distinct()
                .ToList();

            var comparisonData = allCategories.Select(category => new
            {
                name = category,
                currentAmount = monthlyExpenses.Where(e => e.Name == category).Sum(e => e.Amount ?? 0),
                compareAmount = compareMonthlyExpenses.Where(e => e.Name == category).Sum(e => e.Amount ?? 0)
            }).ToList();

            ViewData["ComparisonData"] = JsonSerializer.Serialize(comparisonData);

            var monthlyCategoryData = monthlyExpenses
                .GroupBy(e => e.Name)
                .Select(g => new
                {
                    name = g.Key,
                    amount = g.Sum(e => e.Amount ?? 0)
                })
                .OrderByDescending(x => x.amount)
                .ToList();

            ViewData["MonthlyCategoryData"] = JsonSerializer.Serialize(monthlyCategoryData);

            return View();
        }

        [HttpGet]

        public IActionResult Breakdown(int? month, int? year)
        {
            month = month ?? DateTime.Now.Month;
            year = year ?? DateTime.Now.Year;

            var monthlyExpenses = _expenseService.RetrieveByMonth(UserId, year.Value, month.Value);
            decimal totalAmount = monthlyExpenses.Sum(exp => (decimal)(exp.Amount ?? 0.0));  // Convert double? to decimal

            var categoryData = monthlyExpenses

                .GroupBy(e => e.Name)
                .Select(g => new
                {
                    name = g.Key,

                    amount = (decimal)g.Sum(e => e.Amount ?? 0.0),  // Convert double? to decimal
                    percentage = totalAmount > 0
                        ? decimal.Round((decimal)g.Sum(e => e.Amount ?? 0.0) / totalAmount * 100M, 1)
                        : 0M

                })
                .OrderByDescending(x => x.amount)
                .ToList();

            ViewData["CategoryData"] = JsonSerializer.Serialize(categoryData);
            ViewData["TotalAmount"] = totalAmount;
            ViewData["CurrentMonth"] = month;
            ViewData["CurrentYear"] = year;

            return View();
        }

        
    }
}