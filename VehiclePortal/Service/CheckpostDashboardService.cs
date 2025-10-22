using Microsoft.EntityFrameworkCore;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

public class CheckpostDashboardService : ICheckpostDashboard
{
    private readonly VehicleContext _context;

    public CheckpostDashboardService(VehicleContext context)
    {
        _context = context;
    }

    public async Task<List<CheckpostDashboardQueryModel>> GetDashboardByCheckpostAsync(string? districtName = null)
    {
        // Join Checkpost with Vehicleregistration
        var query = from f in _context.Checkposts
                    join v in _context.Vehicleregistrations
                        on f.VehicleNo equals v.VehicleNo
                    select new { f, v };

        // Apply optional district filter
        if (!string.IsNullOrWhiteSpace(districtName))
            query = query.Where(x => x.v.District.DistrictName == districtName);

        // Group by District + Block
        var data = await query
            .GroupBy(x => new {
                x.v.District.DistrictName,
                x.v.Block.Blockname,
            })
            .Select(g => new CheckpostDashboardQueryModel
            {
                District = g.Key.DistrictName,
                BlockName= g.Key.Blockname,
                NoOfBuses = g.Count(x => x.v.VehicleType == "Bus"),
                TotalPeopleInBuses = g.Where(x => x.v.VehicleType == "Bus").Sum(x => x.f.TotalPeople ?? 0),
                NoOfSmallVehicles = g.Count(x => x.v.VehicleType != "Bus"),
                TotalPeopleSmallVehicles = g.Where(x => x.v.VehicleType != "Bus").Sum(x => x.f.TotalPeople ?? 0),
                TotalPeopleInDistrict = g.Sum(x => x.f.TotalPeople ?? 0)
            })
            .OrderBy(x => x.District)
            .ThenBy(x => x.BlockName)
            .ToListAsync();

        return data;
    }


    public async Task<int> GetOverallPeopleAsync(string? districtName = null)
    {
        var dashboard = await GetDashboardByCheckpostAsync(districtName);
        return dashboard.Sum(d => d.TotalPeopleInDistrict);
    }
}
