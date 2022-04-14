using BankingSystem.UtilityService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.UtilityService.Api.Data
{
    public class UtilityServiceContext : DbContext
    {
        public DbSet<UtilityServicePayment> UtilityServicePayment { get; set; }

        public UtilityServiceContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
