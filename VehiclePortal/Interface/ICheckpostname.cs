using VehiclePortal.CommandModel;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Interface
{
    public interface ICheckpostname
    {
         void add(CheckpostnameCommandModel model);
        List<CheckpostnameQueryModel> List();
    }
}
