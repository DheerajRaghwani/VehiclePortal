using Microsoft.EntityFrameworkCore;
using VehiclePortal.CommandModel;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Service
{
    public class VehicleregistrationService:IVehicleregistration
    {
        private readonly VehicleContext _context;
        public VehicleregistrationService(VehicleContext context)
        {
            _context = context;
        }

        public void Add(VehicleregistrationCommandModel commandModel, UserLoginQueryModel currentUser)
        {
            // ✅ Validate mobile numbers
            if (commandModel.DriverMobileNo.Length != 10 || commandModel.NodalMobileNo.Length != 10)
                throw new ArgumentException("Mobile number must be exactly 10 digits.");

            // ✅ Fetch district from login info
            var district = _context.Districts.FirstOrDefault(d => d.DistrictName == currentUser.DistrictName);
            if (district == null)
                throw new Exception("District not found for the logged-in user.");

            int districtId = district.DistrictId;
            string districtName = district.DistrictName;

            // ✅ Find BlockId based on Block Name and District
            var block = _context.Blocks.FirstOrDefault(b => b.Blockname == commandModel.BlockName && b.DistrictId == districtId);
            if (block == null)
                throw new Exception($"Block '{commandModel.BlockName}' not found under district '{districtName}'.");

            // ✅ Create new entity
            var entity = new Vehicleregistration
            {
                VehicleNo = commandModel.VehicleNo,
                VehicleType = commandModel.VehicleType,
                SeatCapacity = commandModel.SeatCapacity,
                DriverName = commandModel.DriverName,
                DriverMobileNo = commandModel.DriverMobileNo,
                VehicleNodalName = commandModel.VehicleNodalName,
                NodalMobileNo = commandModel.NodalMobileNo,
                DistrictId = districtId,
                BlockId = block.BlockId,
                Gpname = commandModel.Gpname,
                Remark = commandModel.Remark
            };

            _context.Vehicleregistrations.Add(entity);
            _context.SaveChanges();
        }


        public List<VehicleregistrationQueryModel> GetAll(UserLoginQueryModel currentUser)
        {
            if (currentUser == null)
                throw new UnauthorizedAccessException("User not found or unauthorized.");

            // 🔹 Filter by DistrictId instead of DistrictName
            var userDistrict = _context.Districts
            .FirstOrDefault(d => d.DistrictName.Trim().ToLower() == currentUser.DistrictName.Trim().ToLower());

            if (userDistrict == null)
                throw new Exception($"District '{currentUser.DistrictName}' not found for current user");

            return _context.Vehicleregistrations
                .Where(v => v.DistrictId == userDistrict.DistrictId)
                .Select(v => new VehicleregistrationQueryModel
                {
                    VehicleNo = v.VehicleNo,
                    VehicleType = v.VehicleType,
                    SeatCapacity = v.SeatCapacity,
                    DriverName = v.DriverName,
                    DriverMobileNo = v.DriverMobileNo,
                    VehicleNodalName = v.VehicleNodalName,
                    NodalMobileNo = v.NodalMobileNo,
                    District = _context.Districts
                                .Where(d => d.DistrictId == v.DistrictId)
                                .Select(d => d.DistrictName)
                                .FirstOrDefault(),
                    BlockName = _context.Blocks
                                .Where(b => b.BlockId == v.BlockId)
                                .Select(b => b.Blockname)
                                .FirstOrDefault(),
                    Gpname = v.Gpname,
                    Remark = v.Remark
                })
                .ToList();
        }


        public List<VehicleregistrationQueryModel> SearchVehicleByNo(string vehicleNo, UserLoginQueryModel currentUser)
        {
            if (string.IsNullOrWhiteSpace(vehicleNo))
                throw new ArgumentException("Please enter a vehicle number or part of it.");

            // ✅ Get district from login user
            var district = _context.Districts.FirstOrDefault(d => d.DistrictName == currentUser.DistrictName);
            if (district == null)
                throw new Exception("District not found for logged-in user.");

            int districtId = district.DistrictId;

            // ✅ Partial match search using Contains (case-insensitive)
            var vehicles = _context.Vehicleregistrations
                .Where(v => v.DistrictId == districtId &&
                            EF.Functions.Like(v.VehicleNo.ToLower(), $"%{vehicleNo.ToLower()}%"))
                .Select(v => new VehicleregistrationQueryModel
                {
                    VehicleNo = v.VehicleNo,
                    VehicleType = v.VehicleType,
                    SeatCapacity = v.SeatCapacity,
                    DriverName = v.DriverName,
                    DriverMobileNo = v.DriverMobileNo,
                    VehicleNodalName = v.VehicleNodalName,
                    NodalMobileNo = v.NodalMobileNo,
                    District = _context.Districts
                                .Where(d => d.DistrictId == v.DistrictId)
                                .Select(d => d.DistrictName)
                                .FirstOrDefault(),
                    BlockName = _context.Blocks
                                .Where(b => b.BlockId == v.BlockId)
                                .Select(b => b.Blockname)
                                .FirstOrDefault(),
                    Gpname = v.Gpname,
                    Remark = v.Remark
                })
                .ToList();

            return vehicles;
        }


        public List<VehicleregistrationQueryModel> GetAllVehicles()
        {
            return _context.Vehicleregistrations
                .Select(v => new VehicleregistrationQueryModel
                {
                    VehicleNo = v.VehicleNo,
                    VehicleType = v.VehicleType,
                    SeatCapacity = v.SeatCapacity,
                    DriverName = v.DriverName,
                    DriverMobileNo = v.DriverMobileNo,
                    VehicleNodalName = v.VehicleNodalName,
                    NodalMobileNo = v.NodalMobileNo,

                    // Get actual names from navigation properties
                    District = v.District != null ? v.District.DistrictName : "N/A",
                    BlockName = v.Block != null ? v.Block.Blockname : "N/A",

                    Gpname = v.Gpname,
                    Remark = v.Remark
                })
                .ToList();
        }


        public List<VehicleregistrationQueryModel> SearchByBlock(string blockName, UserLoginQueryModel currentUser)
        {
            if (string.IsNullOrWhiteSpace(blockName))
                throw new ArgumentException("Block name is required.");

            var district = _context.Districts.FirstOrDefault(d => d.DistrictName == currentUser.DistrictName);
            if (district == null)
                throw new Exception("District not found for logged-in user.");

            var vehicles = _context.Vehicleregistrations
                .Where(v => v.Block.Blockname == blockName && v.DistrictId == district.DistrictId)
                .Select(v => new VehicleregistrationQueryModel
                {
                    VehicleNo = v.VehicleNo,
                    VehicleType = v.VehicleType,
                    SeatCapacity = v.SeatCapacity,
                    DriverName = v.DriverName,
                    DriverMobileNo = v.DriverMobileNo,
                    VehicleNodalName = v.VehicleNodalName,
                    NodalMobileNo = v.NodalMobileNo,
                    District = v.District.DistrictName,
                    BlockName = v.Block.Blockname,
                    Gpname = v.Gpname,
                    Remark = v.Remark
                })
                .ToList();

            return vehicles;
        }

        // ✅ 2. Delete Vehicle by Vehicle Number
        public void DeleteByVehicleNo(string vehicleNo)
        {
            if (string.IsNullOrWhiteSpace(vehicleNo))
                throw new ArgumentException("Vehicle number is required.");

            var vehicle = _context.Vehicleregistrations
                .FirstOrDefault(v => v.VehicleNo == vehicleNo);

            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle '{vehicleNo}' not found.");

            try
            {
                _context.Vehicleregistrations.Remove(vehicle);
                _context.SaveChanges();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                // Thrown if the vehicle is used in other tables (FK constraint)
                throw new InvalidOperationException(
                    $"Cannot delete vehicle '{vehicleNo}' because it is used in other records."
                );
            }
        }

        // ✅ 3. Update Vehicle by Vehicle Number
        public void UpdateByVehicleNo(string vehicleNo, VehicleregistrationCommandModel commandModel)
        {
            var vehicle = _context.Vehicleregistrations.FirstOrDefault(v => v.VehicleNo == vehicleNo);
            if (vehicle == null)
                throw new KeyNotFoundException($"Vehicle '{vehicleNo}' not found.");

            // Update fields
            vehicle.VehicleType = commandModel.VehicleType;
            vehicle.SeatCapacity = commandModel.SeatCapacity;
            vehicle.DriverName = commandModel.DriverName;
            vehicle.DriverMobileNo = commandModel.DriverMobileNo;
            vehicle.VehicleNodalName = commandModel.VehicleNodalName;
            vehicle.NodalMobileNo = commandModel.NodalMobileNo;
            if (!string.IsNullOrEmpty(commandModel.District))
            {
                var district = _context.Districts.FirstOrDefault(b => b.DistrictName == commandModel.District);
                if (district != null)
                    vehicle.DistrictId = district.DistrictId;
            }
            if (!string.IsNullOrEmpty(commandModel.BlockName))
            {
                var block = _context.Blocks.FirstOrDefault(b => b.Blockname == commandModel.BlockName);
                if (block != null)
                    vehicle.BlockId = block.BlockId;
            }
            vehicle.Gpname = commandModel.Gpname;
            vehicle.Remark = commandModel.Remark;

            // Optional: Update Block if provided
            

            _context.SaveChanges();
        }
    }

}
