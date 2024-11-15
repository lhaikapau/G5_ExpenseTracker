using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public LoginResult AuthenticateUser(string userId, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().Where(x => x.UserId == userId &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            if (!_repository.UserExists(model.UserId))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }


        public User GetUserByUsername(string username)
        {
            return _repository.GetUserByUsername(username);
        }

        public void UpdateUserProfile(EditProfileViewModel model, string currentUsername)
        {
            var existingUser = _repository.GetUserByUsername(currentUsername);
            if (existingUser == null)
            {
                throw new InvalidDataException("User not found");
            }

            // Verify current password if provided
            if (!string.IsNullOrEmpty(model.CurrentPassword))
            {
                var currentPasswordHash = PasswordManager.EncryptPassword(model.CurrentPassword);
                if (existingUser.Password != currentPasswordHash)
                {
                    throw new InvalidDataException("Current password is incorrect");
                }
            }

            // Check if new UserId already exists (excluding current user)
            if (model.UserId != existingUser.UserId && _repository.UserExists(model.UserId))
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }

            // Update basic information
            existingUser.Username = model.Username;
            existingUser.UserId = model.UserId;
            existingUser.UpdatedTime = DateTime.Now;

            // Update password if provided
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    throw new InvalidDataException("New passwords do not match");
                }
                existingUser.Password = PasswordManager.EncryptPassword(model.NewPassword);
            }

            _repository.UpdateUser(existingUser);
        }
    }
}
