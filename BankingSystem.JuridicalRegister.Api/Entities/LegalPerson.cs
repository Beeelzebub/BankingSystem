using System.ComponentModel.DataAnnotations;

namespace BankingSystem.JuridicalRegister.Api.Entities
{
    public class LegalPerson
    {
        public int LegalPersonId { get; set; }
        public string OrganisationName { get; set; }
        public string Description { get; set; }
        public float MonthlyIncome { get; set; }

        [Range(0, float.MaxValue)]
        public float InterestRate { get; set; }
        
        public List<BankAccountNumber> BankAccountNumbers { get; set; }
    }
}
