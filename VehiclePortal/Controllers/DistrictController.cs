using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrict _service;
        public DistrictController(IDistrict service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        
        public ActionResult<List<DistrictQueryModel>> List()
        {
            try
            {
                var result = _service.List();

                if (result == null || !result.Any())
                    return NotFound("⚠️ No districts found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
