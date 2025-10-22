using Microsoft.EntityFrameworkCore;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

public class DashboardService : IDashboardService
{
    private readonly VehicleContext _context;

    public DashboardService(VehicleContext context)
    {
        _context = context;
    }

    public async Task<List<DashboardQueryModel>> GetDashboardByDistrictAsync(string? districtName = null)
    {
        var query = _context.Vehicleregistrations.AsQueryable();

        // Apply district filter only if provided
        if (!string.IsNullOrWhiteSpace(districtName))
            query = query.Where(v => v.District.DistrictName == districtName);

        var data = await query
            .GroupBy(v => new { v.BlockId, v.Block.Blockname, v.DistrictId, v.District.DistrictName })
            .Select(g => new DashboardQueryModel
            {
                BlockId = g.Key.BlockId,
                BlockName = g.Key.Blockname,
                DistrictId = g.Key.DistrictId,
                DistrictName = g.Key.DistrictName,
                NumberOfBuses = g.Count(v => v.VehicleType == "Bus"),
                TotalBusCapacity = g.Where(v => v.VehicleType == "Bus").Sum(v => v.SeatCapacity),
                NumberOfSmallVehicles = g.Count(v => v.VehicleType != "Bus"),
                TotalSmallVehicleCapacity = g.Where(v => v.VehicleType != "Bus").Sum(v => v.SeatCapacity),
                TotalCapacityPerDistrict = g.Sum(v => v.SeatCapacity)
            })
            .OrderBy(x => x.DistrictName)
            .ThenBy(x => x.BlockName)
            .ToListAsync();

        return data;
    }



    public async Task<int> GetOverallCapacityAsync(string districtName = null)
    {
        var dashboard = await GetDashboardByDistrictAsync(districtName);
        return dashboard.Sum(d => d.TotalCapacityPerDistrict);
    }
}
