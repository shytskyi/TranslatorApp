using DataAccessLayer.Repositories.Interfaces;
using Domain;

namespace DataAccessLayer.Repositories
{
    public class HttpLogRepository : IHttpLogRepository
    {
        private readonly ApplicationDbContext _context;
        public HttpLogRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(ApplicationLog log)
        {
            await _context.ApplicationLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
