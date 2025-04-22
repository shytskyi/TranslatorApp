using Domain;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IHttpLogRepository
    {
        Task AddAsync(ApplicationLog log);
    }
}
