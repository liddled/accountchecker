using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    public interface ITransactionManagerFactory
    {
        void Insert(Transaction transaction);
        void Insert(IList<Transaction> transactions);
        void Update(Transaction transaction);
        void Update(IList<Transaction> transactions);
        void Exclude(long transactionId, bool exclude);

        Transaction GetTransaction(long transactionId);
        Transaction GetTransaction(string uniqueId);
        IList<Transaction> GetTransactions(long accountId);
        IList<Transaction> GetTransactions(long accountId, BusDate initialDate, DateView dateView, Status status);
        IList<Transaction> GetTransactions(long accountId, BusDate initialDate, DateView dateView, Status status, IList<Category> withCategories);
        IList<Transaction> GetTransactions(long accountId, BusDate startDate, BusDate endDate, Status status);
    }
}
