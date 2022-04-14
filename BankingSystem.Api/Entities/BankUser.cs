using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Entities
{
    public class BankUser
    {
        public string BankUserId { get; set; }

        public List<BankAccount> BankAccounts { get; set; }
    }
}
