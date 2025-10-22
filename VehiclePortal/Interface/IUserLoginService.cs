using VehiclePortal.Models;

namespace VehiclePortal.Interface
{
    public interface IUserLoginService
    {
        Task<Userlogin?> ValidateUserAsync(string userName, string password);
    }
}
