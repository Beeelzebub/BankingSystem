namespace BankingSystem.UtilityService.Api.Models
{
    public class UtilityServicePayment
    {
        public int UtilityServicePaymentId { get; set; }
        public string BankAccountNumber { get; set; }
        public float Amount { get; set; }
    }
}
