using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {

        }

        public IQueryable<User> GetUsers()
        {
            return this.GetDbSet<User>();
        }

        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user);
            UnitOfWork.SaveChanges();
        }
        public User GetUserByUsername(string username)
        {
            return this.GetDbSet<User>().FirstOrDefault(x => x.Username == username);
        }

        public void UpdateUser(User user)
        {
            var existingUser = this.GetDbSet<User>().Find(user.Id);
            if (existingUser != null)
            {
                this.Context.Entry(existingUser).CurrentValues.SetValues(user);
                UnitOfWork.SaveChanges();
            }
        }

    }
}
