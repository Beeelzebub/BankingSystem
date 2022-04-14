using BankingSystem.Common.Repositories;
using BankingSystem.TaxOffice.Api.Entities;

namespace BankingSystem.TaxOffice.Api.Repositories.Contracts
{
    public interface ITransferTaxRepository : IRepositoryBase<TransferTax>
    {
        Task<float> GetTransferTax(float transferAmount);
        Task<List<TransferTax>> GetTransferTaxes();
    }
}
