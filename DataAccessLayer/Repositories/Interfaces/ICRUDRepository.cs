namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ICRUDRepository<TEntity>
    {
        public Task<List<TEntity>> GetUsersAsync();
        public Task<TEntity> GetUserByIdAsync(int id);
        public Task<bool> AddUserAsync(TEntity customer);
        public Task<bool> UpdateUserAsync(int id, TEntity customerNew);
        public Task<bool> DeleteUserAsync(int id);
    }
}
