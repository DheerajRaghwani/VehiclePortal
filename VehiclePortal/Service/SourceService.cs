using VehiclePortal.Models;
using VehiclePortal.Interface;
using Microsoft.EntityFrameworkCore;
using VehiclePortal.CommandModel;
using VehiclePortal.QueryModel;

public class SourceService : ISourceService
{
    private readonly VehicleContext _context;

    public SourceService(VehicleContext context)
    {
        _context = context;
    }

    public async Task<VehicleDetailsQueryModel?> SearchByVehicleNoAsync(string vehicleNo)
    {
        if (string.IsNullOrWhiteSpace(vehicleNo))
            return null;

        var lowerNo = vehicleNo.ToLower();

        return await _context.Vehicleregistrations
            .Where(v => EF.Functions.Like(v.VehicleNo.ToLower(), $"%{lowerNo}%"))
            .Select(v => new VehicleDetailsQueryModel
            {
                VehicleNo = v.VehicleNo,
                VehicleType = v.VehicleType,
                DriverName = v.DriverName,
                SeatCapacity = v.SeatCapacity,
                DistrictName = v.District != null ? v.District.DistrictName : null,
                BlockName = v.Block != null ? v.Block.Blockname : null,
                VehicleNodalName = v.VehicleNodalName,  // ✅ from same table
                NodalMobileNo = v.NodalMobileNo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<SourceQueryModel> AddAsync(SourceCommandModel model)
    {
        var vehicle = await _context.Vehicleregistrations
            .FirstOrDefaultAsync(v => v.VehicleNo == model.VehicleNo);

        if (vehicle == null)
            throw new KeyNotFoundException("Vehicle number does not exist.");

        var entity = new Source
        {
            Id = Guid.NewGuid(),
            VehicleNo = model.VehicleNo,   // ✅ Added
            Pass = model.Pass,
            TotalPeople = model.TotalPeople,
            CurrentDate = DateTime.Now
        };

        _context.Sources.Add(entity);
        await _context.SaveChangesAsync();

        return new SourceQueryModel
        {
            Id = entity.Id,
            VehicleNo = entity.VehicleNo,
            Pass = entity.Pass,
            TotalPeople = entity.TotalPeople,
            CurrentDate = entity.CurrentDate
        };
    }

    public async Task<List<SourceQueryModel>> GetAllAsync()
    {
        return await _context.Sources
            .Select(c => new SourceQueryModel
            {
                Id = c.Id,
                VehicleNo = c.VehicleNo,
                Pass = c.Pass,
                TotalPeople = c.TotalPeople,
                CurrentDate = c.CurrentDate,
                VehicleNodalName = c.VehicleNoNavigation.VehicleNodalName,  // ✅ from same table
                NodalMobileNo = c.VehicleNoNavigation.NodalMobileNo
            })
            .ToListAsync();
    }

    public async Task<List<VehicleDetailsQueryModel>> GetAllVehiclesAsync()
    {
        var vehicles = await _context.Vehicleregistrations
            .Include(v => v.District)
            .Include(v => v.Block)
            .Where(v => !_context.Sources
                .Any(c => c.VehicleNo == v.VehicleNo)) // ✅ exclude vehicles already in Checkpost
            .Select(v => new VehicleDetailsQueryModel
            {
                VehicleNo = v.VehicleNo,
                DistrictName = v.District != null ? v.District.DistrictName : null,
                BlockName = v.Block != null ? v.Block.Blockname : null,
                VehicleType = v.VehicleType,
                SeatCapacity = v.SeatCapacity,
                VehicleNodalName = v.VehicleNodalName,
                NodalMobileNo = v.NodalMobileNo
            })
            .ToListAsync();

        return vehicles;
    }
    public async Task<List<SourceQueryModel>> SearchSourcesAsync(string vehicleNo)
    {
        if (string.IsNullOrWhiteSpace(vehicleNo))
            return new List<SourceQueryModel>();

        string searchTerm = vehicleNo.ToLower();

        var result = await _context.Sources
            .Where(c => EF.Functions.Like(c.VehicleNo.ToLower(), $"%{searchTerm}%"))
            .Select(c => new SourceQueryModel
            {
                Id = c.Id,
                VehicleNo = c.VehicleNo,
                Pass = c.Pass,
                TotalPeople = c.TotalPeople,
                CurrentDate = c.CurrentDate,
                VehicleNodalName = c.VehicleNoNavigation.VehicleNodalName,
                NodalMobileNo = c.VehicleNoNavigation.NodalMobileNo
            })
            .ToListAsync();

        return result;
    }

}
