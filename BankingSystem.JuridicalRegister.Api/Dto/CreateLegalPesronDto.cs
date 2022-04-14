namespace BankingSystem.JuridicalRegister.Api.Dto
{
    public class CreateLegalPesronDto
    {
        public string OrganisationName { get; set; }
        public string Description { get; set; }
        public float MonthlyIncome { get; set; }
        public float InterestRate { get; set; }
    }
}
