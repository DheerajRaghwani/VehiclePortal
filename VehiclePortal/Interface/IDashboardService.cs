using VehiclePortal.QueryModel;

namespace VehiclePortal.Interface
{
    public interface IDashboardService
    {
        Task<List<DashboardQueryModel>> GetDashboardByDistrictAsync(string districtName);
        Task<int> GetOverallCapacityAsync(string districtName = null);
    }
}
