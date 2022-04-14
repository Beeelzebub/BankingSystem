using BankingSystem.IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.IdentityServer.Data
{
    public class AuthorizationContext : IdentityDbContext<BankUser>
    {
        public AuthorizationContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
