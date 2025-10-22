using Microsoft.EntityFrameworkCore;
using VehiclePortal.CommandModel;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Service
{
    public class BlockService : IBlock
    {
        private readonly VehicleContext _context;

        public BlockService(VehicleContext context)
        {
            _context = context;
        }

        public void Add(BlockCommandModel model)
        {
            // find district id based on district name
            var district = _context.Districts.FirstOrDefault(d => d.DistrictName == model.DistrictName);
            int nextSno = (_context.Blocks.Max(b => (int?)b.BlockId) ?? 0) + 1;
            if (district == null)
                throw new Exception($"District '{model.DistrictName}' not found");

            var block = new Block
            {
                BlockId=nextSno,
                Blockname = model.Blockname,
                DistrictId = district.DistrictId
            };

            _context.Blocks.Add(block);
            _context.SaveChanges();
        }
        public List<BlockQueryModel> GetAll()
        {
            return _context.Blocks
                .Include(b => b.District) // only if navigation exists
                .Select(b => new BlockQueryModel
                {
                    BlockId = b.BlockId,
                    Blockname = b.Blockname,
                    DistrictName = b.District != null ? b.District.DistrictName : "N/A"
                })
                .ToList();
        }

        public List<BlockQueryModel> GetByDistrictName(string districtName)
        {
            return _context.Blocks
                .Include(b => b.District) // to access DistrictName
                .Where(b => b.District != null && b.District.DistrictName.ToLower() == districtName.ToLower())
                .Select(b => new BlockQueryModel
                {
                    BlockId = b.BlockId,
                    Blockname = b.Blockname,
                    DistrictName = b.District.DistrictName
                })
                .ToList();
        }

    }
}
