using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
            return View();
        }

        #region Get Methods
        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        #endregion

        #region Post Methods
        [HttpPost]

        public IActionResult Create(ExpenseViewModel model)
        {
            _expenseService.AddExpense(model, UserId);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
