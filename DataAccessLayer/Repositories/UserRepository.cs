using DataAccessLayer.Repositories.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : ICRUDRepository<User>
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.OrderBy(x => x.Id).Include(x => x.Roles).ToListAsync();
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.OrderBy(x => x.Id).Include(x => x.Roles).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception($"User with ID {id} not found.");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the user: {ex.Message}");
            }
        }
        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the user: {ex.Message}");
            }
        }
        public async Task<bool> UpdateUserAsync(int id, User userNew)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    throw new Exception($"User with ID {id} not found.");
                }

                user.Email = userNew.Email;
                user.Password = userNew.Password;
                user.RoleId = userNew.RoleId;

                return await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user: {ex.Message}");
            }
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    throw new Exception($"User with ID {id} not found.");
                }
                _context.Users.Remove(user);

                return await SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the user: {ex.Message}");
            }
        }

        private async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
