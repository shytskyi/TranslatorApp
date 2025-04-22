using AutoMapper;
using BusinessLogicLayer.Models.DTO;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;
using Domain;

namespace BusinessLogicLayer.Services
{
    public class UserService : ICRUDService<User>, IUserService
    {
        private readonly ICRUDRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public UserService(ICRUDRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public UserViewModel ValidateUser(string Email, string Password)
        {
            try
            {
                var user = GetAllAsync().Result.FirstOrDefault(u => u.Email == Email && u.Password == Password);
                if (user == null)
                {
                    throw new Exception("Invalid credentials");
                }
                return  _mapper.Map<UserViewModel>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during authorization. {ex.Message}");
            }
        }

        public Task<bool> ChangePasswordAsync(int userId, string oldPwd, string newPwd)
        {
            try
            {
                var user = GetByIdAsync(userId);
                if (user.Result.Password != oldPwd)
                {
                    throw new Exception($"Wrong password.");
                }

                UserViewModel PassForUser = new UserViewModel
                {
                    UserId = user.Result.Id,
                    Email = user.Result.Email,
                    Password = newPwd,
                    RoleId = user.Result.RoleId
                };
                var newPassForUser = _mapper.Map<UserViewModel>(PassForUser);
                var result = UpdateAsync(userId, newPassForUser);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Password change is not possible. {ex.Message}");
            }
        }

        public Task<bool> AddAsync(UserViewModel entity)
        {
            var user = _mapper.Map<User>(entity);
            return _userRepository.AddUserAsync(user);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _userRepository.DeleteUserAsync(id);
        }
        public Task<List<User>> GetAllAsync()
        {
            return _userRepository.GetUsersAsync();
        }
        public Task<User> GetByIdAsync(int id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }
        public Task<bool> UpdateAsync(int id, UserViewModel entityNew)
        {
            var user = _mapper.Map<User>(entityNew);
            return _userRepository.UpdateUserAsync(id, user);
        }
    }
}
