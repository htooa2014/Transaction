using Transaction.Context;
using Transaction.Models;


namespace Transaction.Service
{
    public class TransactionService : ITransactionService
    {
        DatabaseContext _dbContext = null;

        public TransactionService(DatabaseContext dbContext)
        {
            _dbContext= dbContext; 
        }
        public List<Transactions> GetTransactions()
        {
            return _dbContext.transactions.ToList();
        }

        public List<Transactions> SaveTransactions(List<Transactions> transactions)
        {
            
            _dbContext.BulkInsert(transactions);

            return transactions;
        }
    }
}
