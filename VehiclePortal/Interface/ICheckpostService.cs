using VehiclePortal.CommandModel;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Interface
{
    public interface ICheckpostService
    {
        Task<List<VehicleSearchResult>> SearchByVehicleNoAsync(string vehicleNo);
        Task<CheckpostQueryModel> AddAsync(CheckpostCommandModel model);
        Task<List<CheckpostQueryModel>> GetAllAsync();
        Task<bool> DeleteByVehicleNoAsync(string vehicleNo);
        Task<List<CheckpostQueryModel>> SearchByBlockNameAsync(string blockName);
        Task<List<CheckpostQueryModel>> SearchByDistrictNameAsync(string districtName);
        Task<List<VehicleSearchResult>> GetAllVehiclesAsync();
        Task<List<CheckpostQueryModel>> SearchCheckpostsAsync(string vehicleNo);
    }
    public class VehicleSearchResult 
    {
        public string VehicleNo { get; set; } = null!;
        public string? DistrictName { get; set; }
        public string? BlockName { get; set; }
        public string? VehicleType { get; set; }
        public int? VehicleCapacity { get; set; }
        public string? VehicleNodalName { get; set; }
        public string? NodalMobileNo { get; set; }
    }


}
