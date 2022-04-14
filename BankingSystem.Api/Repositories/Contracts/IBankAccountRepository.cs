using BankingSystem.Api.Entities;
using BankingSystem.Common.Repositories;

namespace BankingSystem.Api.Repositories.Contracts
{
    public interface IBankAccountRepository : IRepositoryBase<BankAccount>
    {
        Task<BankAccount> GetBankAccountAsync(string bankAccountNumber);
        Task<(BankAccount senderBankAccount, BankAccount receiverBankAccount)> GetBankAccountsForTransactionAsync(string senderBankAccountNumber, 
            string receiverBankAccountNumber);
        IQueryable<BankAccount> GetBankAccounts();
        void AddBankTransaction(Transaction transaction);
        Task<BankUser> GetBankUser(string id);
    }
}
