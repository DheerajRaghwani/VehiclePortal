using VehiclePortal.QueryModel;

namespace VehiclePortal.Interface
{
    public interface ICheckpostDashboard
    {
        Task<List<CheckpostDashboardQueryModel>> GetDashboardByCheckpostAsync(string? districtName = null);
        Task<int> GetOverallPeopleAsync(string districtName = null);
    }
}
