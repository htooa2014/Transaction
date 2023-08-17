namespace Transaction.Models
{
    public class Transactions
    {
        public string TransactionId { get; set; } 
        public DateTime TransactionDate { get; set; }
        public Decimal Amount { get; set; } = 0.0m;

        public string CurrencyCode { get; set; } = "";

        public string Status { get; set; } = "";
    }
}
