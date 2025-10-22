    using VehiclePortal.CommandModel;
    using VehiclePortal.Models;
    using VehiclePortal.QueryModel;

    namespace VehiclePortal.Interface
    {
        public interface ISourceService
        {
            Task<VehicleDetailsQueryModel?> SearchByVehicleNoAsync(string vehicleNo);
            Task<SourceQueryModel> AddAsync(SourceCommandModel model);
            Task<List<SourceQueryModel>> GetAllAsync();
         
            Task<List<VehicleDetailsQueryModel>> GetAllVehiclesAsync();
        Task<List<SourceQueryModel>> SearchSourcesAsync(string vehicleNo);

        }
        public class VehicleDetailsQueryModel
        {
            public string VehicleNo { get; set; }
            public string VehicleType { get; set; }
            public string DriverName { get; set; }
            public int SeatCapacity { get; set; }
            public string? DistrictName { get; set; }  // only names
            public string? BlockName { get; set; }
            public string? VehicleNodalName { get; set; }
            public string? NodalMobileNo { get; set; }

    }

    }
