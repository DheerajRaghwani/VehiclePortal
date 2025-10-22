using Microsoft.AspNetCore.Mvc;
using VehiclePortal.CommandModel;
using VehiclePortal.Interface;
using VehiclePortal.Models;

namespace VehiclePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IBlock _service;

        public BlockController(IBlock service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public IActionResult Add(BlockCommandModel model)
        {
            try
            {
                _service.Add(model);
                return Ok("Block added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
        [HttpGet("by-district-name/{districtName}")]
        public IActionResult GetBlocksByDistrictName(string districtName)
        {
            var blocks = _service.GetByDistrictName(districtName);
            return Ok(blocks);
        }
    }
}
