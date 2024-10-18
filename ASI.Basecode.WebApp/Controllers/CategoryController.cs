using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ASI.Basecode.WebApp.Controllers
{
    /// <summary>
   /// Book Controller
   /// </summary>
    public class CategoryController : ControllerBase<CategoryController>
    {
        private readonly ICategoryService _categoryService;
        /// Constructor
       /// </summary>       
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>        
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public CategoryController(ICategoryService categoryService,
                                  IHttpContextAccessor httpContextAccessor,
                                  ILoggerFactory loggerFactory,
                                  IConfiguration configuration,
                                  IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _categoryService = categoryService;
        }
        // GET: CategoryController
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
        public IActionResult Create(CategoryViewModel model) 
        {
            _categoryService.AddCategory(model, UserId);
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
