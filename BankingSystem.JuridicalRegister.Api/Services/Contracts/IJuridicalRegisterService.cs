using BankingSystem.Common.DTO;
using BankingSystem.JuridicalRegister.Api.Dto;

namespace BankingSystem.JuridicalRegister.Api.Services.Contracts
{
    public interface IJuridicalRegisterService
    {
        Task<ApiResult<LegalPersonPaymentInfo>> GetLegalPersonPaymentInfo(string bankAccountNumber);
        Task<ApiResult> AddBankAccountNumber(string organisationName, string bankAccountNumber);
        Task<ApiResult> CreateLegalPesron(CreateLegalPesronDto legalPersonDto);
    }
}
