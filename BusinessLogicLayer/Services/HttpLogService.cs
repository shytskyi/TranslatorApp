using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;
using Domain;

namespace BusinessLogicLayer.Services
{
    public class HttpLogService : IHttpLogService
    {
        private readonly IHttpLogRepository _httpLogRepository;
        public HttpLogService(IHttpLogRepository httpLogRepository) => _httpLogRepository = httpLogRepository;

        public async Task LogAsync(ApplicationLog log) => await _httpLogRepository.AddAsync(log);        
    }
}
