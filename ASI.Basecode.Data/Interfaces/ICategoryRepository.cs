using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Interfaces
{
    public interface ICategoryRepository
    {
        void AddCategory(Category model);

        IEnumerable<Category> RetrieveAll();

        void UpdateCategory(Category model);

        void DeleteCategory(Category model);

    }
}
