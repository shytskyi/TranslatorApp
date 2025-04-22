using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;
using Domain;

namespace BusinessLogicLayer.Services
{
    public class ApplicationLogService : IApplicationLogService
    {
        private readonly IApplicationLogRepository _applicationLogRepository;
        public ApplicationLogService(IApplicationLogRepository applicationLogRepository)
        {
            _applicationLogRepository = applicationLogRepository;
        }
        public async Task<List<ApplicationLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _applicationLogRepository.GetLogsByDateRangeAsync(startDate, endDate);
        }
        public async Task<List<ApplicationLogByMethodAndStatus>> GetLogsByMethodAndStatusAsync(string? method = null, int? statusCode = null)
        {
            return await _applicationLogRepository.GetLogsByMethodAndStatusAsync(method, statusCode);
        }
        public async Task<List<ApplicationLogByPathFilter>> GetLogsPagedFilteredAsync(string? pathFilter = null, int? minStatus = null)
        {
            return await _applicationLogRepository.GetLogsPagedFilteredAsync(pathFilter, minStatus);
        }
    }
}
