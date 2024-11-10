using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using ASI.Basecode.Data;
using ASI.Basecode.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5; // Number of items per page
            var data = _categoryService.RetrieveAll(UserId, pageNumber, pageSize);
            var totalCategories = _categoryService.GetTotalCategoryCount(UserId);

            var totalPages = (int)Math.Ceiling((double)totalCategories / pageSize);
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(data);
        }


        #region Get Methods
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RetrieveCategoriesFromUserId()
        {
            var categories = _categoryService.RetrieveCategoriesFromUserId(UserId);
            return Json(categories);
        }

        [HttpGet]
        public IActionResult Edit(int CategoryId)
        {
            var data = _categoryService.RetrieveCategory(CategoryId);
            return View(data);
        }

        [HttpGet]
        public IActionResult Delete(int CategoryId)
        {
            var data = _categoryService.RetrieveCategory(CategoryId);
            return View(data);
        }


        #endregion

        #region Post Methods
        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            _categoryService.AddCategory(model, UserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return the model with validation errors
            }

            _categoryService.UpdateCategory(model, UserId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PostDelete(int CategoryId)
        {
            _categoryService.DeleteCategory(CategoryId);
            return RedirectToAction("Index");
        }

        #endregion


    }
}