using Domain;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IHttpLogService
    {
        Task LogAsync(ApplicationLog log);
    }
}
