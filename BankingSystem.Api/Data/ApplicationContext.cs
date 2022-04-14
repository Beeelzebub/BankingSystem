using BankingSystem.Api.Entities;
using BankingSystem.Common;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<BankUser> BankUser { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        public ApplicationContext(DbContextOptions options) 
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Transaction>()
                .HasOne(x => x.SenderAccount)
                .WithMany(x => x.OutgoingTransactions)
                .HasForeignKey(x => x.SenderAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Transaction>()
                .HasOne(x => x.ReceiverAccount)
                .WithMany(x => x.IncomingTransactions)
                .HasForeignKey(x => x.ReceiverAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<BankAccount>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.BankAccounts)
                .HasForeignKey(x => x.BankUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<BankUser>().HasData(
                new BankUser[]
                {
                    new BankUser { BankUserId = "1 1" },
                    new BankUser { BankUserId = "2 2" }
                });

            builder.Entity<BankAccount>().HasData(
                new BankAccount[]
                {
                    new BankAccount { BankUserId = "1 1", Balance = 1000, BankAccountId = 1,
                        BankAccountNumber = "1111", BankAccountType = BankAccountType.NaturalPersonAccount },
                    new BankAccount { BankUserId = "2 2", Balance = 1000, BankAccountId = 2,
                        BankAccountNumber = "2222", BankAccountType = BankAccountType.LegalPersonAccount }
                });
        }
    }
}

