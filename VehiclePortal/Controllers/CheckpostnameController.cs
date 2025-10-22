using Microsoft.AspNetCore.Mvc;
using VehiclePortal.CommandModel;
using VehiclePortal.QueryModel;
using VehiclePortal.Service;
using VehiclePortal.Interface;
using Microsoft.AspNetCore.Authorization;

namespace VehiclePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckpostnameController : ControllerBase
    {
        private readonly ICheckpostname _service;

        public CheckpostnameController(ICheckpostname service)
        {
            _service = service;
        }


        // ✅ POST: api/Checkpost/Add
        [HttpPost("Add")]
        [Authorize(Roles = "checkpost")]
        public IActionResult Add([FromBody] CheckpostnameCommandModel model)
        {
            if (model == null)
                return BadRequest("Invalid data.");

            _service.add(model);
            return Ok(new { message = "Checkpost name added successfully!" });
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "checkpost")]
        public ActionResult<List<CheckpostnameQueryModel>> GetAll()
        {
            var list = _service.List();
            if (list == null || list.Count == 0)
                return NotFound("No checkpost names found.");

            return Ok(list);
        }
    }
}
