using VehiclePortal.CommandModel;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Interface
{
    public interface IBlock
    {
        void Add(BlockCommandModel model);
        List<BlockQueryModel> GetAll();
        List<BlockQueryModel> GetByDistrictName(string districtName);
    }
}
