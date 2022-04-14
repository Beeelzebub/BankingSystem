namespace BankingSystem.JuridicalRegister.Api.Entities
{
    public class BankAccountNumber
    {
        public int BankAccountNumberId { get; set; }
        public string Number { get; set; }
        public int LegalPersonId { get; set; }

        public LegalPerson LegalPerson { get; set; }
    }
}
