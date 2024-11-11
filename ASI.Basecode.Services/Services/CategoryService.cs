using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.Extensions.Configuration;

namespace ASI.Basecode.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _config = configuration;
        }

        public void AddCategory(CategoryViewModel model, string userId)
        {
            var newCategory = new Category();
            newCategory.Name = model.Name;
            newCategory.Description = model.Description;
            newCategory.CreatedBy = userId;
            newCategory.DateCreated = DateTime.Now;
            newCategory.DateUpdated = DateTime.Now;
            _categoryRepository.AddCategory(newCategory);
        }

        public List<CategoryViewModel> RetrieveAll(string UserId, int pageNumber = 1, int pageSize = 5)
        {
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _categoryRepository
                .RetrieveAll()
                .Where(c => c.CreatedBy == UserId) // Filter by userId
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new CategoryViewModel
                {
                    CategoryId = s.CategoryId,
                    Name = s.Name,
                    Description = s.Description
                })
                .ToList();

            return data;
        }

        public List<CategoryViewModel> FilterAll(string UserId, int pageNumber = 1, int pageSize = 5)
        {
            var serverUrl = _config.GetValue<string>("ServerUrl");
            var data = _categoryRepository
                .RetrieveAll()
                .Where(c => c.CreatedBy == UserId) // Filter by userId
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new CategoryViewModel
                {
                    CategoryId = s.CategoryId,
                    Name = s.Name,
                    Description = s.Description
                })
                .ToList();

            return data;
        }

        public int GetTotalCategoryCount(string userId)
        {
            return _categoryRepository.RetrieveAll().Count(c => c.CreatedBy == userId);
        }

        public List<CategoryViewModel> RetrieveCategoriesFromUserId(string UserId)
        {
            var data = _categoryRepository.RetrieveAll().Where(x => x.CreatedBy == UserId)
                .Select(s => new CategoryViewModel
                {
                    CategoryId = s.CategoryId,
                    Name = s.Name,
                    Description = s.Description
                }).ToList();

            return data;
        }

        public CategoryViewModel RetrieveCategory(int CategoryId)
        {
            var category = _categoryRepository.RetrieveAll().Where(x => x.CategoryId.Equals(CategoryId)).Select(s => new CategoryViewModel
            {
                CategoryId = s.CategoryId,
                Name = s.Name,
                Description = s.Description,
            }).FirstOrDefault();

            return category;
        }

        public void UpdateCategory(CategoryViewModel model, string userId)
        {
            var category = _categoryRepository.RetrieveAll().Where(x => x.CategoryId.Equals(model.CategoryId)).FirstOrDefault();
            if (category != null)
            {
                category.Name = model.Name;
                category.Description = model.Description;
                category.DateUpdated = DateTime.Now;
                category.CreatedBy = userId;

                _categoryRepository.UpdateCategory(category);
            }
        }

        public void DeleteCategory(int CategoryId)
        {
            var category = _categoryRepository.RetrieveAll().Where(x => x.CategoryId.Equals(CategoryId)).FirstOrDefault();
            if (category != null)
            {
                _categoryRepository.DeleteCategory(category);
            }
        }
    }
}
