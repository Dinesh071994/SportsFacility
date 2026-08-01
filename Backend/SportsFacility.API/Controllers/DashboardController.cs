using Microsoft.AspNetCore.Mvc;
using SportsFacility.Domain.Interface;
using System.Threading.Tasks;

namespace SportsFacility.API.Controllers
{
    // [Authorize] // Uncomment when authentication is fully hooked up
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            var data = await _dashboardService.GetDashboardDataAsync();
            return Ok(data);
        }
    }
}
