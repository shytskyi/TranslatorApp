using Domain;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IApplicationLogRepository
    {
        Task<List<ApplicationLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<ApplicationLogByMethodAndStatus>> GetLogsByMethodAndStatusAsync(string? method = null, int? statusCode = null);
        Task<List<ApplicationLogByPathFilter>> GetLogsPagedFilteredAsync(string? pathFilter = null, int? minStatus = null);
    }
}
