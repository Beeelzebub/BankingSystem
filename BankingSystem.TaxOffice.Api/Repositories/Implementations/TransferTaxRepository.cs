using BankingSystem.Common.Repositories;
using BankingSystem.TaxOffice.Api.Data;
using BankingSystem.TaxOffice.Api.Entities;
using BankingSystem.TaxOffice.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.TaxOffice.Api.Repositories.Implementations
{
    public class TransferTaxRepository : RepositoryBase<TransferTax, TaxOfficeContext>, ITransferTaxRepository
    {
        public TransferTaxRepository(TaxOfficeContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task<float> GetTransferTax(float transferAmount)
        {
            var transferTax = await _dbContext.TransferTax.OrderBy(t => t.BeginningWith).FirstOrDefaultAsync(t => t.BeginningWith > transferAmount);

            return transferTax.BeginningWith;
        }

        public async Task<List<TransferTax>> GetTransferTaxes()
        {
            return await GetAll().ToListAsync();
        }
    }
}
