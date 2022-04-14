using BankingSystem.Common.DTO;

namespace BankingSystem.TaxOffice.Api.Services.Contracts
{
    public interface ITransferTaxService
    {
        Task<ApiResult<float>> GetTransferTax(float transferAmount);
        Task<ApiResult<List<TransferTaxDto>>> GetTransferTaxes();
    }
}
