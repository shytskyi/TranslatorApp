using DataAccessLayer.Repositories.Interfaces;
using Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationLog>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ApplicationLogs
                .FromSqlInterpolated($@"EXEC dbo.GetLogsByDateRange @StartDate = {startDate}, @EndDate = {endDate}")
                .ToListAsync();
        }

        public async Task<List<ApplicationLogByMethodAndStatus>> GetLogsByMethodAndStatusAsync(string? method = null, int? statusCode = null)
        {
            var methodParam = new SqlParameter("@Method", string.IsNullOrEmpty(method) ? (object)DBNull.Value : method);

            var statusParam = new SqlParameter("@StatusCode", statusCode.HasValue ? statusCode.Value : (object)DBNull.Value);

            return await _context.ApplicationLogByMethodAndStatuses
                         .FromSqlRaw("EXEC dbo.GetLogsByMethodAndStatus @Method, @StatusCode", methodParam, statusParam)
                         .AsNoTracking()
                         .ToListAsync();
        }

        public async Task<List<ApplicationLogByPathFilter>> GetLogsPagedFilteredAsync(string? pathFilter = null, int? minStatus = null)
        {
            var pathParam = new SqlParameter("@PathFilter", pathFilter != null ? pathFilter : (object)DBNull.Value);
            var statusParam = new SqlParameter("@Status", minStatus.HasValue ? minStatus.Value : (object)DBNull.Value);

            return await _context.ApplicationLogByPathFilters
                .FromSqlRaw("EXEC dbo.GetLogsFiltered @PathFilter, @Status", pathParam, statusParam)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
