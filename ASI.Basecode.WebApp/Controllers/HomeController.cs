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
            var totalAmount = _expenseService.RetrieveAll(UserId).Sum(exp => exp.Amount ?? 0);
            ViewData["TotalAmount"] = totalAmount; // Pass the total amount to the view

            var categoryData = _expenseService.RetrieveAll(UserId)
            .GroupBy(e => e.Name)
            .Select(g => new
            {
                name = g.Key,
                amount = g.Sum(e => e.Amount ?? 0),
                percentage = (g.Sum(e => e.Amount ?? 0) / totalAmount) * 100 // Calculate percentage
            })
            .OrderByDescending(x => x.amount) // Sort by amount
            .ToList();

            ViewData["CategoryData"] = JsonSerializer.Serialize(categoryData);

           

            return View();

        }

       

    }
}
