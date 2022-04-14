using BankingSystem.Common.DTO;
using BankingSystem.TaxOffice.Api.Repositories.Contracts;
using BankingSystem.TaxOffice.Api.Services.Contracts;

namespace BankingSystem.TaxOffice.Api.Services.Implementations
{
    public class TransferTaxService : ITransferTaxService
    {
        private readonly ITransferTaxRepository _repository;

        public async Task<ApiResult<float>> GetTransferTax(float transferAmount)
        {
            float tax = 0;

            var taxes = await _repository.GetTransferTaxes();

            foreach (var item in taxes)
            {
                if (item.BeginningWith > transferAmount)
                {
                    return ApiResult<float>.CreateSuccessfulResult(tax);
                }

                tax = item.BeginningWith;
            }

            return ApiResult<float>.CreateFailedResult("Not found.");
        }

        public async Task<ApiResult<List<TransferTaxDto>>> GetTransferTaxes()
        {
            var result = new List<TransferTaxDto>();

            var taxes = await _repository.GetTransferTaxes();

            foreach(var tax in taxes)
            {
                result.Add(new TransferTaxDto { BeginningWith = tax.BeginningWith, Tax = tax.Tax });
            }

            return ApiResult<List<TransferTaxDto>>.CreateSuccessfulResult(result);
        }
    }
}
