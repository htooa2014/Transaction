using Transaction.Context;
using Transaction.Models;


namespace Transaction.Service
{
    public class TransactionService : ITransactionService
    {
        public DatabaseContext _dbContext = null;

        public TransactionService(DatabaseContext dbContext)
        {
            _dbContext= dbContext; 
        }
        public List<Transactions> GetTransactions()
        {
            return _dbContext.transactions.ToList();
        }

        //public List<Transactions> GetTransactions(int id)
        //{
        //    List<Transactions> transaction = null;
        //    transaction= _dbContext.Find(id);
        //    if(transaction == null)
        //    {
        //        return NotFound();
        //    }
        //    return transaction;
        //}

        public List<Transactions> SaveTransactions(List<Transactions> transactions)
        {
            
            _dbContext.BulkInsert(transactions);

            return transactions;
        }
    }
}
