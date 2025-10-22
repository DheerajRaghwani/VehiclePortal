using Microsoft.AspNetCore.Mvc;
using VehiclePortal.Interface;
using VehiclePortal.CommandModel;
using VehiclePortal.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace VehiclePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly ISourceService _service;

        public SourceController(ISourceService service)
        {
            _service = service;
        }

        // GET: api/Foodstatus/Vehicle/{vehicleNo}
        [HttpGet("Vehicle/{vehicleNo}")]
        [Authorize(Roles = "source")]
        public async Task<IActionResult> GetByVehicleNo(string vehicleNo)
        {
            var result = await _service.SearchByVehicleNoAsync(vehicleNo);
            if (result == null) return NotFound("Vehicle not found.");
            return Ok(result);
        }
        [HttpPost("Add")]
        [Authorize(Roles = "source")]
        public async Task<IActionResult> Add([FromBody] SourceCommandModel model)
        {
            if (model == null) return BadRequest("Invalid data");

            try
            {
                var result = await _service.AddAsync(model);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("GetAll")]
        [Authorize(Roles = "source")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("vehicles/all")]
        [Authorize(Roles = "source")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _service.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("search")]
        [Authorize(Roles = "source")]
        public async Task<ActionResult<List<SourceQueryModel>>> Search([FromQuery] string vehicleNo)
        {
            if (string.IsNullOrWhiteSpace(vehicleNo))
                return BadRequest("Vehicle number is required.");

            var result = await _service.SearchSourcesAsync(vehicleNo);
            return Ok(result);
        }
    }
}
