using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Data.EFCore
{
    public class SeedHelper
    {
        private readonly ProjectXDbContext _context;

        public SeedHelper(ProjectXDbContext context)
        {
            _context = context;
        }

        public void CreateData()
        {
            if(!_context.Tenants.Any())
            {
                _context.Tenants.Add(new Core.Tenants.Tenant() { Code = "D01", Name = "Default" });
                _context.SaveChanges();
            }
        }
    }
}
