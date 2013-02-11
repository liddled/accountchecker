using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        void Insert(IEnumerable<Transaction> entities);
        void Update(IEnumerable<Transaction> entities);

        IEnumerable<TransactionCategory> GetAllTransactionCategories();

        void Save(TransactionInfo transactionInfo);
        IEnumerable<TransactionInfo> GetAllTransactionInfos();
    }
}
