using BusinessLogicLayer.Models.DTO;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> ChangePasswordAsync(int userId, string oldPwd, string newPwd);
        UserViewModel ValidateUser(string Email, string Password);
    }
}
