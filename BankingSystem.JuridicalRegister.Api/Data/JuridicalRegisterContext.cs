using BankingSystem.JuridicalRegister.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.JuridicalRegister.Api.Data
{
    public class JuridicalRegisterContext : DbContext
    {
        public DbSet<LegalPerson> LegalPeson { get; set; }
        public DbSet<BankAccountNumber> BankAccountNumber { get; set; }

        public JuridicalRegisterContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<BankAccountNumber>().HasData(
                new BankAccountNumber[]
                {
                    new BankAccountNumber { BankAccountNumberId = 1, Number = "2222", LegalPersonId = 1 }
                });

            builder.Entity<LegalPerson>().HasData(
                new LegalPerson[]
                {
                    new LegalPerson { LegalPersonId = 1, OrganisationName = "VSU corp", Description = "corp desc", InterestRate = 10, MonthlyIncome = 20000 }
                });
        }
    }
}
