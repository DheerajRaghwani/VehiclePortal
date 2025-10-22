using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Service
{
    public class DistrictService : IDistrict
    {
        private readonly VehicleContext _context;

        public DistrictService(VehicleContext context)
        {
            _context = context;
        }
            public List<DistrictQueryModel> List()
        {
            return _context.Districts
                .Select(b => new DistrictQueryModel
                {
                    DistrictId= b.DistrictId,
                    DistrictName= b.DistrictName
                })
                .ToList();
        }
    }
    }

