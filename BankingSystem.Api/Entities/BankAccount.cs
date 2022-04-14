using BankingSystem.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Api.Entities
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }
        public string BankAccountNumber { get; set; }
        public float Balance { get; set; }
        public BankAccountType BankAccountType { get; set; }

        public string BankUserId { get; set; }
        public BankUser Owner { get; set; }

        public List<Transaction> OutgoingTransactions { get; set; } = new();
        public List<Transaction> IncomingTransactions { get; set; } = new();
    }
}
