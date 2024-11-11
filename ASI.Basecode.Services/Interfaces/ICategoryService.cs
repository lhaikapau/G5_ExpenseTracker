using ASI.Basecode.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Interfaces
{
    public interface ICategoryService
    {
        void AddCategory(CategoryViewModel model, string userId);

        List<CategoryViewModel> RetrieveAll(string userId, int pageNumber = 1, int pageSize = 5);
        int GetTotalCategoryCount(string userId);
        CategoryViewModel RetrieveCategory(int CategoryId);

        void UpdateCategory(CategoryViewModel model, string userId);

        void DeleteCategory(int CategoryId);
        List<CategoryViewModel> RetrieveCategoriesFromUserId(string UserId);

    }
}
