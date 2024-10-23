using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public void AddCategory(Category model)
        {
            this.GetDbSet<Category>().Add(model);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<Category> RetrieveAll()
        {
            return this.GetDbSet<Category>();
        }

        public void UpdateCategory(Category model)
        {
            this.GetDbSet<Category>().Update(model);
            UnitOfWork.SaveChanges();
        }

        public void DeleteCategory(Category model)
        {
            this.GetDbSet<Category>().Remove(model);
            UnitOfWork.SaveChanges();
        }
    }
}
