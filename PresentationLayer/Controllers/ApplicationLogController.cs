using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ApplicationLogController : Controller
    {
        private readonly IApplicationLogService _logService;

        public ApplicationLogController(IApplicationLogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ByDateRange(DateTime startDate, DateTime endDate)
        {
            var logs = await _logService.GetLogsByDateRangeAsync(startDate, endDate);
            return PartialView("_ByDateRange", logs); 
        }

        [HttpGet]
        public async Task<IActionResult> ByMethodAndStatus(string? method = null, int? statusCode = null)
        {
            var logs = await _logService.GetLogsByMethodAndStatusAsync(method, statusCode);
            return PartialView("_ByMethodAndStatus", logs);
        }

        [HttpGet]
        public async Task<IActionResult> Paged(int pageNumber = 1, int pageSize = 50, string? pathFilter = null, int? minStatus = null)
        {
            var logs = await _logService.GetLogsPagedFilteredAsync(pathFilter, minStatus);
            return PartialView("_Filtered", logs);
        }
    }
}
