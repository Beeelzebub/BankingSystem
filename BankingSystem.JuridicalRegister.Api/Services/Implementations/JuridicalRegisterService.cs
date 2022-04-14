using BankingSystem.Common.DTO;
using BankingSystem.JuridicalRegister.Api.Dto;
using BankingSystem.JuridicalRegister.Api.Entities;
using BankingSystem.JuridicalRegister.Api.Repositories.Contracts;
using BankingSystem.JuridicalRegister.Api.Services.Contracts;

namespace BankingSystem.JuridicalRegister.Api.Services.Implementations
{
    public class JuridicalRegisterService : IJuridicalRegisterService
    {
        private readonly IJuridicalRegisterRepository _repository;

        public JuridicalRegisterService(IJuridicalRegisterRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResult> AddBankAccountNumber(string organisationName, string bankAccountNumber)
        {
            var legalPerson = await _repository.GetLegalPersonByOrganisationNameAsync(organisationName);

            if (legalPerson == null)
            {
                ApiResult.CreateFailedResult("Legal person not found.");
            }

            legalPerson.BankAccountNumbers.Add(new BankAccountNumber { Number = bankAccountNumber });

            await _repository.SaveChangesAsync();

            return ApiResult.CreateSuccessfulResult();
        }

        public async Task<ApiResult> CreateLegalPesron(CreateLegalPesronDto legalPersonDto)
        {
            var legalPerson = new LegalPerson
            {
                Description = legalPersonDto.Description,
                InterestRate = legalPersonDto.InterestRate,
                MonthlyIncome = legalPersonDto.MonthlyIncome,
                OrganisationName = legalPersonDto.OrganisationName
            };

            _repository.Add(legalPerson);
            await _repository.SaveChangesAsync();

            return ApiResult.CreateSuccessfulResult("Legal person created.");
        }

        public async Task<ApiResult<LegalPersonPaymentInfo>> GetLegalPersonPaymentInfo(string bankAccountNumber)
        {
            var legalPerson = await _repository.GetLegalPersonByBankAccountNumberAsync(bankAccountNumber);

            if (legalPerson == null)
            {
                return ApiResult<LegalPersonPaymentInfo>.CreateFailedResult("Legal person not found.");
            }

            var legalPersonPaymentInfo = new LegalPersonPaymentInfo()
            {
                OrganisationName = legalPerson.OrganisationName,
                InterestRate = legalPerson.InterestRate
            };

            return ApiResult<LegalPersonPaymentInfo>.CreateSuccessfulResult(legalPersonPaymentInfo);
        }
    }
}
