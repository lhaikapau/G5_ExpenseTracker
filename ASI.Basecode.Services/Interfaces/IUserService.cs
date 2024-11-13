using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IUserService
    {
        LoginResult AuthenticateUser(string UserId, string password, ref User user);
        void AddUser(UserViewModel model);
        // Add the GetUserById method to the interface
        User GetUserByUsername(string username); // Add this method
    }
}
