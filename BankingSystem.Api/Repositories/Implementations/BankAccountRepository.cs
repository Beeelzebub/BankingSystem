using BankingSystem.Api.Data;
using BankingSystem.Api.Entities;
using BankingSystem.Api.Repositories.Contracts;
using BankingSystem.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Api.Repositories.Implementations
{
    public class BankAccountRepository : RepositoryBase<BankAccount, ApplicationContext>, IBankAccountRepository
    {
        public BankAccountRepository(ApplicationContext dbContext)
            : base(dbContext)
        {

        }

        public void AddBankTransaction(Transaction transaction)
        {
            _dbContext.Transaction.Add(transaction);
        }

        public async Task<BankAccount> GetBankAccountAsync(string bankAccountNumber)
        {
            return await _dbContext.BankAccount.SingleOrDefaultAsync();
        }

        public IQueryable<BankAccount> GetBankAccounts()
        {
            return _dbContext.BankAccount;
        }

        public async Task<(BankAccount senderBankAccount, BankAccount receiverBankAccount)> GetBankAccountsForTransactionAsync(string senderBankAccountNumber, 
            string receiverBankAccountNumber)
        {
            var accounts = await _dbContext.BankAccount
                .Where(acc => acc.BankAccountNumber == senderBankAccountNumber || acc.BankAccountNumber == receiverBankAccountNumber)
                .ToListAsync();
            var sender = accounts.SingleOrDefault(acc => acc.BankAccountNumber == senderBankAccountNumber);
            var receiver = accounts.SingleOrDefault(acc => acc.BankAccountNumber == receiverBankAccountNumber);

            return (sender, receiver);
        }

        public async Task<BankUser> GetBankUser(string id)
        {
            return await _dbContext.BankUser.FirstOrDefaultAsync(u => u.BankUserId == id);
        }
    }
}
