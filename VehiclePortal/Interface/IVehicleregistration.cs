using VehiclePortal.CommandModel;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Interface
{
    public interface IVehicleregistration
    {
        public void Add(VehicleregistrationCommandModel commandModel, UserLoginQueryModel currentUser);
        public List<VehicleregistrationQueryModel> GetAll(UserLoginQueryModel currentUser);

        public List<VehicleregistrationQueryModel> SearchVehicleByNo(string vehicleNo, UserLoginQueryModel currentUser);
        public List<VehicleregistrationQueryModel> GetAllVehicles();

        List<VehicleregistrationQueryModel> SearchByBlock(string blockName, UserLoginQueryModel currentUser);
        void DeleteByVehicleNo(string vehicleNo);
        void UpdateByVehicleNo(string vehicleNo, VehicleregistrationCommandModel commandModel);

    }
}
