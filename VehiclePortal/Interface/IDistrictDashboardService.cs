using VehiclePortal.QueryModel;

namespace VehiclePortal.Interface
{
    public interface IDistrictDashboardService
    {
        Task<List<DistrictDashboardQueryModel>> GetDashboardByDistrictAsync(string? districtName = null);
        Task<int> GetOverallPeopleAsync(string districtName = null);
    }
}
