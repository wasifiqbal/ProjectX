using Microsoft.EntityFrameworkCore;
using ProjectX.Core.Tenants;

namespace ProjectX.Data.EFCore
{
    public class ProjectXDbContext : DbContext
    {
        public ProjectXDbContext() { }
        public ProjectXDbContext(DbContextOptions options) : base(options) { }


        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
