using BankingSystem.Common;

namespace BankingSystem.Api.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTimeOffset MadeOn { get; set; }
        public string Details { get; set; }
        public float Amount { get; set; }
        public float Tax { get; set; }
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }

        public BankAccount SenderAccount { get; set; }
        public BankAccount ReceiverAccount { get; set; }

        public static Transaction CreateBankTransaction(BankAccount senderAccount, BankAccount receiverAccount, float transactionAmount, float tax) =>
            new Transaction()
            {
                Amount = transactionAmount,
                Tax = tax,
                MadeOn = DateTimeOffset.UtcNow,
                Details = $"Money transfer from {receiverAccount.BankAccountNumber}",
                TransactionType = receiverAccount.BankAccountType switch
                {
                    BankAccountType.NaturalPersonAccount => TransactionType.MoneyTransferToNaturalPerson,
                    BankAccountType.LegalPersonAccount => TransactionType.MoneyTransferToLegalPerson,
                    BankAccountType.HCSAccount => TransactionType.PaymentOfUtilityServices
                },
                SenderAccount = senderAccount,
                ReceiverAccount = receiverAccount
            };
    }
}
