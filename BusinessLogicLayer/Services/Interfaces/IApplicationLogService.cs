using Domain;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IApplicationLogService
    {
        Task<List<ApplicationLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<ApplicationLogByMethodAndStatus>> GetLogsByMethodAndStatusAsync(string? method = null, int? statusCode = null);
        Task<List<ApplicationLogByPathFilter>> GetLogsPagedFilteredAsync(string? pathFilter = null, int? minStatus = null);
    }
}
