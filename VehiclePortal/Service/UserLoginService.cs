using Microsoft.EntityFrameworkCore;
using VehiclePortal.Interface;
using VehiclePortal.Models;

namespace VehiclePortal.Service
{
    public class UserLoginService : IUserLoginService
    {
        private readonly VehicleContext _context;

        public UserLoginService(VehicleContext context)
        {
            _context = context;
        }

        public async Task<Userlogin?> ValidateUserAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            return await _context.Userlogins
                .Include(u => u.District)
                .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
        }
    }
}
