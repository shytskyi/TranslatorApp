using DataAccessLayer.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Tests
{
    public class HttpLogRepositoryTests
    {
        [Fact]
        public async Task AddAsync_PersistsApplicationLog()
        {
            using var ctx = InMemoryDbContextFactory.Create();
            var repo = new HttpLogRepository(ctx);

            var log = new ApplicationLog { Method = "GET", Path = "/test", StatusCode = 200 };
            await repo.AddAsync(log);

            var saved = await ctx.ApplicationLogs.SingleAsync();
            Assert.Equal("GET", saved.Method);
            Assert.Equal("/test", saved.Path);
            Assert.Equal(200, saved.StatusCode);
        }
    }
}
