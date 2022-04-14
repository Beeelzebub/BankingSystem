using BankingSystem.Api.Entities;
using BankingSystem.Api.Repositories.Contracts;
using BankingSystem.Api.Services.Contracts;
using BankingSystem.Common;
using BankingSystem.Common.DTO;
using BankingSystem.Common.HttpClients;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Api.Services.Implementations
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _repository;
        private readonly JuridicalRegisterHttpClient _juridicalRegisterClient;
        private readonly TaxOfficeHttpClient _taxOfficeClient;
        UtilityServiceHttpClient _utilityServiceHttpClient;
        private const float LimitTransferWithNoTax = 500;

        public BankAccountService(IBankAccountRepository repository, 
            JuridicalRegisterHttpClient juridicalRegisterClient, 
            TaxOfficeHttpClient taxOfficeClient,
            UtilityServiceHttpClient utilityServiceHttpClient)
        {
            _repository = repository;
            _juridicalRegisterClient = juridicalRegisterClient;
            _taxOfficeClient = taxOfficeClient;
            _utilityServiceHttpClient = utilityServiceHttpClient;
        }


        public async Task<ApiResult> MakeTransaction(string senderBankAccountNumber, string receiverBankAccountNumber, float transactionAmount)
        {
            try
            {
                float legalPersonTax = 0;
                float transferTax = 0;
                float writeOffAmount = transactionAmount;

                var (senderAccount, receiverAccount) = await _repository.GetBankAccountsForTransactionAsync(senderBankAccountNumber, receiverBankAccountNumber);

                if (senderAccount == null || receiverAccount == null)
                {
                    return ApiResult.CreateFailedResult("Bank account not found.");
                }

                if (transactionAmount > LimitTransferWithNoTax)
                {
                    transferTax = await _taxOfficeClient.GetTransferTaxAsync(transactionAmount);
                    writeOffAmount = transactionAmount + ((transactionAmount * transferTax) / 100);
                }

                if (senderAccount.Balance < writeOffAmount)
                {
                    return ApiResult.CreateFailedResult("Insufficient account balance.");
                }

                if (receiverAccount.BankAccountType == BankAccountType.HCSAccount)
                {
                    var overPay = await _utilityServiceHttpClient.PayUtilityServiceAsync(senderBankAccountNumber, transactionAmount);
                    receiverAccount.Balance += transactionAmount;
                    senderAccount.Balance -= writeOffAmount - overPay;

                }
                else
                {
                    senderAccount.Balance -= writeOffAmount;

                    if (receiverAccount.BankAccountType == BankAccountType.LegalPersonAccount)
                    {
                        var legalPersonInfo = await _juridicalRegisterClient.GetLegalPersonInfoAsync(receiverAccount.BankAccountNumber);
                        legalPersonTax = (transactionAmount * legalPersonInfo.InterestRate) / 100;
                    }

                    receiverAccount.Balance += transactionAmount - legalPersonTax;
                }

                var transaction = Transaction.CreateBankTransaction(senderAccount, receiverAccount, transactionAmount, transferTax);

                _repository.AddBankTransaction(transaction);

                await _repository.SaveChangesAsync();

                return ApiResult.CreateSuccessfulResult();
            }
            catch (Exception exeption)
            {
                return ApiResult.CreateFailedResult(
                    "Something went wrong! Please try again later. If this error continues to occur, please contact our support center.");
            }
        }
        public async Task<ApiResult> CreateBankAccountAsync(CreateNaturalPersonAccountDto bankAccountDto)
        {
            await CreateBankAccountAsync(bankAccountDto.BankAccountNumber, bankAccountDto.Owner, BankAccountType.NaturalPersonAccount);

            await _repository.SaveChangesAsync();

            return ApiResult.CreateSuccessfulResult("Account created");
        }

        public async Task<ApiResult> CreateBankAccountAsync(CreateUtilityServiceAccountDto bankAccountDto)
        {
            await CreateBankAccountAsync(bankAccountDto.BankAccountNumber, bankAccountDto.Owner, BankAccountType.HCSAccount);

            var success = await _utilityServiceHttpClient.CreateUtilityServicePayment(bankAccountDto.BankAccountNumber);

            if (success)
            {
                return ApiResult.CreateFailedResult("Bank account creation failed.");
            }

            await _repository.SaveChangesAsync();

            return ApiResult.CreateSuccessfulResult("Account created");
        }

        public async Task<ApiResult> CreateBankAccountAsync(CreateLegalPersonAccountDto bankAccountDto)
        {
            await CreateBankAccountAsync(bankAccountDto.BankAccountNumber, bankAccountDto.Organisation, BankAccountType.LegalPersonAccount);

            var success = await _juridicalRegisterClient.AddBankNumberToOrganisationAsync(bankAccountDto);

            if (!success)
            {
                return ApiResult.CreateFailedResult("Legal peson not found");
            }

            await _repository.SaveChangesAsync();

            return ApiResult.CreateSuccessfulResult("Account created");
        }

        private async Task CreateBankAccountAsync(string bankAccountNumber, string owner, BankAccountType accountType)
        {
            var user = await _repository.GetBankUser(owner);

            var bankAccount = new BankAccount
            {
                Balance = 0,
                Owner = user ?? new BankUser { BankUserId = owner },
                BankAccountNumber = bankAccountNumber,
                BankAccountType = accountType
            };

            _repository.Add(bankAccount);
        }

        public async Task<ApiResult<List<BankAccountDto>>> GetBankAccountsAsync()
        {
            var result = new List<BankAccountDto>();

            var accaounts = await _repository.GetAll().Include(acc => acc.Owner).ToListAsync();
            
            foreach (var acc in accaounts)
            {
                result.Add(new BankAccountDto
                {
                    Balance = acc.Balance,
                    BankAccountNumber = acc.BankAccountNumber,
                    Owner = acc.Owner.BankUserId
                });
            }

            return ApiResult<List<BankAccountDto>>.CreateSuccessfulResult(result);
        }

        public Task<ApiResult<List<BankAccountDto>>> GetBankAccountsByUserAsync(string owner)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<BankAccountDto>>> GetBankAccountTransacionAsync(string bankAccountNumber)
        {
            throw new NotImplementedException();
        }
    }
}
