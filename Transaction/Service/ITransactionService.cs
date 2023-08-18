using Transaction.Models;

namespace Transaction.Service
{
    public interface ITransactionService
    {
        List<Transactions> GetTransactions();
        List<Transactions> SaveTransactions(List<Transactions> transactions);
    }
}
