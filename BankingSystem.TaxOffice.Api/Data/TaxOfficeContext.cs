using BankingSystem.TaxOffice.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.TaxOffice.Api.Data
{
    public class TaxOfficeContext : DbContext
    {
        public DbSet<TransferTax> TransferTax { get; set; }

        public TaxOfficeContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TransferTax>().HasData(new TransferTax[]
            {
                new TransferTax { TransferTaxId = 1, BeginningWith = 500, Tax = 1.5f },
                new TransferTax { TransferTaxId = 1, BeginningWith = 1500, Tax = 3f },
                new TransferTax { TransferTaxId = 1, BeginningWith = 3000, Tax = 5f }
            });
        }
    }
}
