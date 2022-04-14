using BankingSystem.Common.DTO;

namespace BankingSystem.Api.Services.Contracts
{
    public interface IBankAccountService
    {
        Task<ApiResult> MakeTransaction(string senderBankAccountNumber, string receiverBankAccountNumber, float transactionsAmount);

        Task<ApiResult> CreateBankAccountAsync(CreateUtilityServiceAccountDto bankAccount);

        Task<ApiResult> CreateBankAccountAsync(CreateLegalPersonAccountDto bankAccount);

        Task<ApiResult> CreateBankAccountAsync(CreateNaturalPersonAccountDto bankAccount);

        Task<ApiResult<List<BankAccountDto>>> GetBankAccountsAsync();

        Task<ApiResult<List<BankAccountDto>>> GetBankAccountsByUserAsync(string owner);

        Task<ApiResult<List<BankAccountDto>>> GetBankAccountTransacionAsync(string bankAccountNumber);
    }
}
