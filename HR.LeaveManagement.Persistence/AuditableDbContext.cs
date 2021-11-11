using HR.LeaveManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence
{
    public abstract class AuditableDbContext : DbContext
    {
        public AuditableDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual async Task<int> SaveChangesAsync(string userName = "System")
        {
            foreach (var entry in base.ChangeTracker.Entries<BaseDomainEntity>()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                entry.Entity.LastModifiedDate = DateTime.Now;
                entry.Entity.LastModifiedBy = userName;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.Now;
                    entry.Entity.CreatedBy = userName;
                }
            }

            var result = await base.SaveChangesAsync();
            return result;
        }
    }
}
