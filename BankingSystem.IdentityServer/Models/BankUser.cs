using Microsoft.AspNetCore.Identity;

namespace BankingSystem.IdentityServer.Models
{
    public class BankUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
