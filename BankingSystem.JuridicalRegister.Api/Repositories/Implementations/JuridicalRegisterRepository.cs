using BankingSystem.Common.Repositories;
using BankingSystem.JuridicalRegister.Api.Data;
using BankingSystem.JuridicalRegister.Api.Entities;
using BankingSystem.JuridicalRegister.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.JuridicalRegister.Api.Repositories.Implementations
{
    public class JuridicalRegisterRepository : RepositoryBase<LegalPerson, JuridicalRegisterContext>, IJuridicalRegisterRepository
    {
        public JuridicalRegisterRepository(JuridicalRegisterContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<LegalPerson> GetLegalPersonByBankAccountNumberAsync(string bankAccountNumber)
        {
            return await _dbContext.LegalPeson
                .Include(p => p.BankAccountNumbers)
                .SingleOrDefaultAsync(p => p.BankAccountNumbers.Any(n => n.Number == bankAccountNumber));
        }

        public async Task<LegalPerson> GetLegalPersonByOrganisationNameAsync(string organisationName)
        {
            return await _dbContext.LegalPeson
                .Include(p => p.BankAccountNumbers)
                .SingleOrDefaultAsync(p => p.OrganisationName == organisationName);
        }
    }
}
