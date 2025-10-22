using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePortal.Interface;
using VehiclePortal.QueryModel;

[ApiController]
[Route("api/[controller]")]
public class DistrictDashboardController : ControllerBase
{
    private readonly IDistrictDashboardService _dashboardService;

    public DistrictDashboardController(IDistrictDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // GET: api/DistrictDashboard
    // Optional query: ?districtName=DistrictA
    [HttpGet]
    [Authorize(Roles = "admin,user")]
    public async Task<IActionResult> GetDashboard([FromQuery] string? districtName = null)
    {
        // Get district-wise data
        var districtData = await _dashboardService.GetDashboardByDistrictAsync(districtName);

        // Get overall total
        var overallPeople = await _dashboardService.GetOverallPeopleAsync(districtName);

        // Return structured response
        var response = new
        {
            DistrictData = districtData,
            OverallPeople = overallPeople
        };

        return Ok(response);
    }
}
