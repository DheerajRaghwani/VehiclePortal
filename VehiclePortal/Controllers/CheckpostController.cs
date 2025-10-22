using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePortal.CommandModel;
using VehiclePortal.Interface;
using VehiclePortal.QueryModel;
using VehiclePortal.Service;

namespace VehiclePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckpostController : ControllerBase
    {
        private readonly ICheckpostService _service;

        public CheckpostController(ICheckpostService service)
        {
            _service = service;
        }

        [HttpGet("Search/{vehicleNo}")]
        [Authorize(Roles = "checkpost")]
        public async Task<IActionResult> Search(string vehicleNo)
        {
            if (string.IsNullOrWhiteSpace(vehicleNo))
                return BadRequest(new { Message = "Vehicle number is required" });

            var result = await _service.SearchByVehicleNoAsync(vehicleNo);
            if (result == null) return NotFound(new { Message = "Vehicle not found" });

            return Ok(result);
        }
        [HttpPost("Add")]
        [Authorize(Roles = "checkpost")]
        public async Task<IActionResult> Add([FromBody] CheckpostCommandModel model)
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
        [Authorize(Roles = "checkpost")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpDelete("DeleteByVehicleNo/{vehicleNo}")]
        [Authorize(Roles = "checkpost")]
        public async Task<IActionResult> DeleteByVehicleNo(string vehicleNo)
        {
            try
            {
                bool result = await _service.DeleteByVehicleNoAsync(vehicleNo);
                if (result)
                    return Ok(new { message = "Checkpost record deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

            return BadRequest(new { message = "Failed to delete record." });
        }

        [HttpGet("search-by-district")]
        [Authorize(Roles = "checkpost")]
        public async Task<IActionResult> SearchByDistrict([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("District name is required.");

            var result = await _service.SearchByDistrictNameAsync(name);
            return Ok(result);
        }
        [HttpGet("search-by-block")]
        [Authorize(Roles = "checkpost")]
        public async Task<IActionResult> SearchByBlock([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Block name is required.");

            var result = await _service.SearchByBlockNameAsync(name);
            return Ok(result);
        }

        [HttpGet("vehicles/all")]

        [Authorize(Roles = "checkpost")]

        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _service.GetAllVehiclesAsync();
            return Ok(vehicles);
        }
        [HttpGet("search")]
        [Authorize(Roles = "checkpost")]
        public async Task<ActionResult<List<CheckpostQueryModel>>> SearchCheckpost([FromQuery] string vehicleNo)
        {
            if (string.IsNullOrWhiteSpace(vehicleNo))
                return BadRequest("Vehicle number is required.");

            var result = await _service.SearchCheckpostsAsync(vehicleNo);
            return Ok(result);
        }

    }
}
