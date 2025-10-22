using Microsoft.EntityFrameworkCore;
using VehiclePortal.CommandModel;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Service
{
    public class CheckpostService : ICheckpostService
    {
        private readonly VehicleContext _context;

        public CheckpostService(VehicleContext context)
        {
            _context = context;
        }

        // Other CRUD methods...

        public async Task<List<VehicleSearchResult>> SearchByVehicleNoAsync(string vehicleNo)
        {
            if (string.IsNullOrWhiteSpace(vehicleNo))
                return new List<VehicleSearchResult>();

            var vehicles = await _context.Vehicleregistrations
                .Include(v => v.District)
                .Include(v => v.Block)
                .Where(v => EF.Functions.Like(v.VehicleNo.ToLower(), $"%{vehicleNo.ToLower()}%"))
                .Select(v => new VehicleSearchResult
                {
                    VehicleNo = v.VehicleNo,
                    DistrictName = v.District != null ? v.District.DistrictName : null,
                    BlockName = v.Block != null ? v.Block.Blockname : null,
                    VehicleType = v.VehicleType,
                    VehicleCapacity = v.SeatCapacity,
                    VehicleNodalName = v.VehicleNodalName,  // ✅ from same table
                    NodalMobileNo = v.NodalMobileNo         // ✅ from same table
                })
                .ToListAsync();

            return vehicles;
        }

        public async Task<CheckpostQueryModel> AddAsync(CheckpostCommandModel model)
        {
            // Ensure vehicle exists
            var vehicle = await _context.Vehicleregistrations
                .FirstOrDefaultAsync(v => v.VehicleNo == model.VehicleNo);

            if (vehicle == null)
                throw new KeyNotFoundException("Vehicle number does not exist.");

            // ✅ Capture full date + time
            var currentDateTime = DateTime.Now;

            var entity = new Checkpost
            {
                Id = Guid.NewGuid(),
                CheckpostId = model.CheckpostId,
                VehicleNo = model.VehicleNo,
                Pass = model.Pass,
                TotalPeople = model.TotalPeople,
                CurrentDate = currentDateTime   // ✅ Includes both date & time
            };

            _context.Checkposts.Add(entity);
            await _context.SaveChangesAsync();

            // ✅ Return full DateTime with time part to frontend
            return new CheckpostQueryModel
            {
                Id = entity.Id,
                CheckpostId = entity.CheckpostId,
                VehicleNo = entity.VehicleNo,
                Pass = entity.Pass,
                TotalPeople = entity.TotalPeople,
                CurrentDate = entity.CurrentDate   // ✅ will include time when serialized
            };
        }

        public async Task<List<CheckpostQueryModel>> GetAllAsync()
        {
            var result = await _context.Checkposts
                .Include(c => c.Checkpostname)
                .Join(
                    _context.Vehicleregistrations,       // ✅ Join with Vehicleregistrations
                    c => c.VehicleNo,                    // Match by VehicleNo in Checkposts
                    v => v.VehicleNo,                    // Match by VehicleNo in Vehicleregistrations
                    (c, v) => new { c, v }               // Combine both
                )
                .Select(x => new CheckpostQueryModel
                {
                    Id = x.c.Id,
                    CheckpostId = x.c.CheckpostId,
                    CheckpostName = x.c.Checkpostname != null ? x.c.Checkpostname.CheckpostName : null,
                    VehicleNo = x.c.VehicleNo,
                    Pass = x.c.Pass,
                    TotalPeople = x.c.TotalPeople,
                    CurrentDate = x.c.CurrentDate,

                    // ✅ Now fetch values from Vehicleregistrations table
                    VehicleNodalName = x.v.VehicleNodalName,
                    NodalMobileNo = x.v.NodalMobileNo
                })
                .ToListAsync();

            return result;
        }




        public async Task<bool> DeleteByVehicleNoAsync(string vehicleNo)
        {
            // Find the record by VehicleNo
            var checkpost = await _context.Checkposts.FirstOrDefaultAsync(c => c.VehicleNo == vehicleNo);

            // If not found, throw exception
            if (checkpost == null)
                throw new KeyNotFoundException("Checkpost record not found for this Vehicle Number.");

            // Remove record
            _context.Checkposts.Remove(checkpost);
            await _context.SaveChangesAsync();

            return true; // Indicate success
        }
        public async Task<List<CheckpostQueryModel>> SearchByDistrictNameAsync(string districtName)
        {
            var result = await (from c in _context.Checkposts
                                join d in _context.Districts on c.DistrictId equals d.DistrictId
                                join b in _context.Blocks on c.BlockId equals b.BlockId into blockJoin
                                from b in blockJoin.DefaultIfEmpty()
                                where EF.Functions.Like(d.DistrictName.ToLower(), $"%{districtName.ToLower()}%")
                                select new CheckpostQueryModel
                                {
                                    Id = c.Id,
                                    CheckpostId = c.CheckpostId,
                                    VehicleNo = c.VehicleNo,
                                    DistrictId = c.DistrictId,
                                    DistrictName = d.DistrictName, // ✅ Converted here
                                    BlockId = c.BlockId,
                                    BlockName = b.Blockname,       // ✅ Converted here (optional)
                                    Pass = c.Pass,
                                    TotalPeople = c.TotalPeople,
                                    CurrentDate = c.CurrentDate
                                })
                                .ToListAsync();

            return result;
        }
        public async Task<List<CheckpostQueryModel>> SearchByBlockNameAsync(string blockName)
        {
            var result = await (from c in _context.Checkposts
                                join n in _context.Checkpostnames
                                    on c.CheckpostId equals n.CheckpostId into nameJoin
                                from n in nameJoin.DefaultIfEmpty()
                                join b in _context.Blocks
                                    on c.BlockId equals b.BlockId into blockJoin
                                from b in blockJoin.DefaultIfEmpty()
                                where EF.Functions.Like(b.Blockname.ToLower(), $"%{blockName.ToLower()}%")
                                select new CheckpostQueryModel
                                {
                                    Id = c.Id,
                                    CheckpostId = c.CheckpostId,
                                    CheckpostName = n.CheckpostName,
                                    VehicleNo = c.VehicleNo,
                                    BlockId = c.BlockId,
                                    BlockName = b.Blockname,
                                    Pass = c.Pass,
                                    TotalPeople = c.TotalPeople,
                                    CurrentDate = c.CurrentDate
                                })
                                .ToListAsync();

            return result;
        }
        public async Task<List<VehicleSearchResult>> GetAllVehiclesAsync()
        {
            var vehicles = await _context.Vehicleregistrations
                .Include(v => v.District)
                .Include(v => v.Block)
                .Where(v => !_context.Checkposts
                    .Any(c => c.VehicleNo == v.VehicleNo)) // ❌ exclude vehicles already in Checkpost
                .Select(v => new VehicleSearchResult
                {
                    VehicleNo = v.VehicleNo,
                    DistrictName = v.District != null ? v.District.DistrictName : null,
                    BlockName = v.Block != null ? v.Block.Blockname : null,
                    VehicleType = v.VehicleType,
                    VehicleCapacity = v.SeatCapacity,
                    VehicleNodalName = v.VehicleNodalName,
                    NodalMobileNo = v.NodalMobileNo
                })
                .ToListAsync();

            return vehicles;
        }
        public async Task<List<CheckpostQueryModel>> SearchCheckpostsAsync(string vehicleNo)
        {
            if (string.IsNullOrWhiteSpace(vehicleNo))
                return new List<CheckpostQueryModel>();

            string searchTerm = vehicleNo.ToLower();

            var result = await _context.Checkposts
                .Include(c => c.Checkpostname)
                .Join(
                    _context.Vehicleregistrations,
                    c => c.VehicleNo,
                    v => v.VehicleNo,
                    (c, v) => new { c, v }
                )
                .Where(x => EF.Functions.Like(x.c.VehicleNo.ToLower(), $"%{searchTerm}%"))
                .Select(x => new CheckpostQueryModel
                {
                    Id = x.c.Id,
                    CheckpostId = x.c.CheckpostId,
                    CheckpostName = x.c.Checkpostname != null ? x.c.Checkpostname.CheckpostName : null,
                    VehicleNo = x.c.VehicleNo,
                    Pass = x.c.Pass,
                    TotalPeople = x.c.TotalPeople,
                    CurrentDate = x.c.CurrentDate,
                    VehicleNodalName = x.v.VehicleNodalName,
                    NodalMobileNo = x.v.NodalMobileNo
                })
                .ToListAsync();

            return result;
        }

    }


}