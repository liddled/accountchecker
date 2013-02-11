using System.Collections.Generic;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Framework
{
    public interface ITransactionManager
    {
        void Insert(Transaction transaction);
        void Insert(IList<Transaction> transactions);
        void Update(Transaction transaction);
        void Update(IList<Transaction> transactions);
        IEnumerable<Transaction> GetAllItems();

        IEnumerable<TransactionCategory> GetAllTransactionCategories();

        void Save(TransactionInfo transactionInfo);
        IEnumerable<TransactionInfo> GetAllTransactionInfos();
    }
}
