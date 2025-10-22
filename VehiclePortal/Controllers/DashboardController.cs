using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePortal.Interface;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("GetByDistrict")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> GetByDistrict([FromQuery] string? districtName = null)
        {
            // Call service (handles null or empty automatically)
            var districtData = await _dashboardService.GetDashboardByDistrictAsync(districtName);
            var overallCapacity = await _dashboardService.GetOverallCapacityAsync(districtName);

            var result = new
            {
                OverallCapacity = overallCapacity,
                DistrictData = districtData
            };

            return Ok(result);
        }
    }

}

