using BusinessLogicLayer.Models.DTO;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICRUDService<TEntity>
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<bool> AddAsync(UserViewModel entity);
        Task<bool> UpdateAsync(int id, UserViewModel entityNew);
        Task<bool> DeleteAsync(int id);
    }
}
