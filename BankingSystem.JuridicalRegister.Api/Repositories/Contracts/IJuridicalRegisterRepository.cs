using BankingSystem.Common.Repositories;
using BankingSystem.JuridicalRegister.Api.Entities;

namespace BankingSystem.JuridicalRegister.Api.Repositories.Contracts
{
    public interface IJuridicalRegisterRepository : IRepositoryBase<LegalPerson>
    {
        Task<LegalPerson> GetLegalPersonByBankAccountNumberAsync(string bankAccountNumber);
        Task<LegalPerson> GetLegalPersonByOrganisationNameAsync(string organisationName);
    }
}
