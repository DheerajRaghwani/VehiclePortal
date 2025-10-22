using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePortal.Interface;
using VehiclePortal.QueryModel;

[ApiController]
[Route("api/[controller]")]
public class CheckpostDashboardController : ControllerBase
{
    private readonly ICheckpostDashboard _dashboardService;

    public CheckpostDashboardController(ICheckpostDashboard dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // GET: api/CheckpostDashboard
    // Optional query parameter: ?districtName=DistrictA
    [HttpGet]
    [Authorize(Roles = "admin,user")]
    public async Task<IActionResult> GetDashboard([FromQuery] string? districtName = null)
    {
        // Get district-wise data
        var districtData = await _dashboardService.GetDashboardByCheckpostAsync(districtName);

        // Get overall total
        var overallPeople = await _dashboardService.GetOverallPeopleAsync(districtName);

        // Prepare response
        var response = new
        {
            DistrictData = districtData,
            OverallPeople = overallPeople
        };

        return Ok(response);
    }
}
