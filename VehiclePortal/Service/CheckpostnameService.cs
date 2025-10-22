using Microsoft.EntityFrameworkCore;
using VehiclePortal.CommandModel;
using VehiclePortal.Interface;
using VehiclePortal.Models;
using VehiclePortal.QueryModel;

namespace VehiclePortal.Service
{
    
        public class CheckpostnameService : ICheckpostname
        {
            private readonly VehicleContext _context;

            public CheckpostnameService(VehicleContext context)
            {
                _context = context;
            }

        public void add(CheckpostnameCommandModel model)
        {
            int nextSno = (_context.Checkpostnames.Max(b => (int?)b.CheckpostId) ?? 0) + 1;
             

            var check = new Checkpostname
            {
                CheckpostId = nextSno,
                CheckpostName=model.Checkpostname1
                
            };

            _context.Checkpostnames.Add(check);
            _context.SaveChanges();

        }
        public List<CheckpostnameQueryModel> List()
        {
            return _context.Checkpostnames
                .Select(b => new CheckpostnameQueryModel
                {
                    CheckpostId = b.CheckpostId,
                    CheckpostName = b.CheckpostName
                })
                .ToList();
        }
    }

    }
