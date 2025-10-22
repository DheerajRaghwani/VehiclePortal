using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiclePortal.CommandModel;
using VehiclePortal.Interface;
using VehiclePortal.QueryModel;
using VehiclePortal.Service;
using ClosedXML.Excel;

namespace VehiclePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleregistrationController : ControllerBase
    {
        private readonly IVehicleregistration _service;

        public VehicleregistrationController(IVehicleregistration service)
        {
            _service = service;
        }
        private UserLoginQueryModel GetCurrentUser()
        {
            try
            {
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                    return null;

                var userId = User.FindFirst("UserId")?.Value;
                var userName = User.FindFirst("UserName")?.Value;
                var role = User.FindFirst("Role")?.Value;
                var districtName = User.FindFirst("DistrictName")?.Value;

                return new UserLoginQueryModel
                {
                    UserId = string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId),
                    UserName = userName,
                    LoginRole = role,
                    DistrictName = districtName
                };
            }
            catch
            {
                return null; // If any claim missing or token invalid
            }
        }
        // POST: api/VehicleRegistration/Add
        [HttpPost("add")]
        [Authorize(Roles = "user,admin")]
        public IActionResult AddVehicle([FromBody] VehicleregistrationCommandModel model)
        {
            try
            {
                // ✅ Validate request body
                if (model == null)
                    return BadRequest(new { message = "Invalid request. Vehicle data is missing." });

                // ✅ Get logged-in user (from JWT/session)
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                    return Unauthorized(new { message = "User is not authenticated or session expired." });

                // ✅ Call service layer
                _service.Add(model, currentUser);

                // ✅ Return success response
                return Ok(new
                {
                    message = "Vehicle registered successfully.",
                    status = "success"
                });
            }
            catch (ArgumentException ex) // For validation errors (e.g., 10-digit check)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    status = "validation_error"
                });
            }
            catch (InvalidOperationException ex) // For not found or logical issues
            {
                return NotFound(new
                {
                    message = ex.Message,
                    status = "not_found"
                });
            }
            catch (Exception ex) // For all other unexpected exceptions
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred while registering the vehicle.",
                    details = ex.Message,
                    status = "error"
                });
            }
        }






        [HttpGet]
        [Authorize(Roles = "user,admin")]
        public IActionResult GetAll()
        {
            try
            {
                // ✅ Extract data from JWT token
                var districtName = User.FindFirst("DistrictName")?.Value;
                var loginRole = User.FindFirst("LoginRole")?.Value;

                if (string.IsNullOrEmpty(districtName))
                    return Unauthorized("District not found in token.");

                // ✅ Build UserLoginQueryModel for service
                var currentUser = new UserLoginQueryModel
                {
                    DistrictName = districtName,
                    LoginRole = loginRole
                };

                var result = _service.GetAll(currentUser);

                if (result == null || result.Count == 0)
                    return NotFound("No vehicle records found for this district.");

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }


        [HttpGet("search/{vehicleNo}")]
        [Authorize(Roles = "user,admin")] // Only authorized roles


        public IActionResult SearchVehicleByNo(string vehicleNo)
        {
            try
            {
                var districtName = User.FindFirst("DistrictName")?.Value;
                var loginRole = User.FindFirst("LoginRole")?.Value;

                if (string.IsNullOrEmpty(districtName))
                    return Unauthorized("District not found in token.");

                var currentUser = new UserLoginQueryModel
                {
                    DistrictName = districtName,
                    LoginRole = loginRole
                };

                var vehicle = _service.SearchVehicleByNo(vehicleNo, currentUser);

                if (vehicle == null)
                    return NotFound($"Vehicle '{vehicleNo}' not found in your district.");

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        // ✅ Search Vehicles by Block Name
        [HttpGet("SearchByBlock/{blockName}")]
        [Authorize(Roles = "user,admin")]
        
        public IActionResult SearchByBlock(string blockName)
        {
            try
            {
                var districtName = User.FindFirst("DistrictName")?.Value;
                var loginRole = User.FindFirst("LoginRole")?.Value;

                if (string.IsNullOrEmpty(districtName))
                    return Unauthorized("District not found in token.");

                var currentUser = new UserLoginQueryModel
                {
                    DistrictName = districtName,
                    LoginRole = loginRole
                };// custom logic

                var result = _service.SearchByBlock(blockName, currentUser);
                if (result == null)
                    return NotFound($"Vehicle not found in your district.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
        
        // ✅ Delete by Vehicle No
        [HttpDelete("DeleteByVehicleNo/{vehicleNo}")]
        [Authorize(Roles = "user,admin")]
        public IActionResult DeleteByVehicleNo(string vehicleNo)
        {
            try
            {
                _service.DeleteByVehicleNo(vehicleNo);
                return Ok(new { Message = $"Vehicle '{vehicleNo}' deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message }); // 409
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Unexpected error: " + ex.Message });
            }
        }

        // ✅ Update by Vehicle No
        [HttpPut("UpdateByVehicleNo/{vehicleNo}")]
        [Authorize(Roles = "user,admin")]
        public IActionResult UpdateByVehicleNo(string vehicleNo, [FromBody] VehicleregistrationCommandModel model)
            {
                try
                {
                    _service.UpdateByVehicleNo(vehicleNo, model);
                    return Ok(new { Message = $"Vehicle '{vehicleNo}' updated successfully." });
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }
        
        
          
        [HttpPost("ExportToExcel")]
        [Authorize(Roles = "user,admin")]
        public IActionResult ExportToExcel([FromBody] List<VehicleregistrationQueryModel> vehicleList)
        {
            Console.WriteLine($"Received {vehicleList?.Count ?? 0} vehicles for export");
            if (vehicleList == null || vehicleList.Count == 0)
                return BadRequest("No vehicle data received for export.");

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Vehicles");

                // Header row
                worksheet.Cell(1, 1).Value = "Vehicle No";
                worksheet.Cell(1, 2).Value = "Vehicle Type";
                worksheet.Cell(1, 3).Value = "Seat Capacity";
                worksheet.Cell(1, 4).Value = "Driver Name";
                worksheet.Cell(1, 5).Value = "Driver Mobile No";
                worksheet.Cell(1, 6).Value = "Vehicle Nodal Name";
                worksheet.Cell(1, 7).Value = "Nodal Mobile No";
                worksheet.Cell(1, 8).Value = "District";
                worksheet.Cell(1, 9).Value = "Block Name";
                worksheet.Cell(1, 10).Value = "GP Name";
                worksheet.Cell(1, 11).Value = "Remark";

                // Data rows
                for (int i = 0; i < vehicleList.Count; i++)
                {
                    var v = vehicleList[i];
                    worksheet.Cell(i + 2, 1).Value = v.VehicleNo;
                    worksheet.Cell(i + 2, 2).Value = v.VehicleType;
                    worksheet.Cell(i + 2, 3).Value = v.SeatCapacity;
                    worksheet.Cell(i + 2, 4).Value = v.DriverName;
                    worksheet.Cell(i + 2, 5).Value = v.DriverMobileNo;
                    worksheet.Cell(i + 2, 6).Value = v.VehicleNodalName;
                    worksheet.Cell(i + 2, 7).Value = v.NodalMobileNo;
                    worksheet.Cell(i + 2, 8).Value = v.District;
                    worksheet.Cell(i + 2, 9).Value = v.BlockName;
                    worksheet.Cell(i + 2, 10).Value = v.Gpname;
                    worksheet.Cell(i + 2, 11).Value = v.Remark;
                }

                // Auto adjust columns
                worksheet.Columns().AdjustToContents();

                // Save to memory stream
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var fileName = $"Vehicle_List_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName
                    );
                }
            }
        }

    }

}



