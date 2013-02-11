using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.BusinessLogic
{
	public class TransactionManagerFactory : ITransactionManagerFactory
	{
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICachedTypes _cachedTypes;
        private readonly GenericCache<Category> _categoryCache;
        private readonly GenericCache<Transaction> _transactionCache;
        private readonly GenericCache<TransactionInfo> _transactionInfoCache;
        private readonly GenericCache<TransactionCategory> _transactionCategoryCache;

        private readonly ITransactionManager _transactionManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IIdAllocationManager _idAllocationManager;

        public TransactionManagerFactory(ICachedTypes cachedTypes, ITransactionManager transactionManager, ICategoryManager categoryManager, IIdAllocationManager idAllocationManager)
        {
            _cachedTypes = cachedTypes;
            _categoryCache = _cachedTypes.GetCache<Category>();
            _transactionCache = _cachedTypes.GetCache<Transaction>();
            _transactionInfoCache = _cachedTypes.GetCache<TransactionInfo>();
            _transactionCategoryCache = _cachedTypes.GetCache<TransactionCategory>();

            _transactionManager = transactionManager;
            _categoryManager = categoryManager;
            _idAllocationManager = idAllocationManager;
        }

        public void Insert(Transaction transaction)
        {
            var newCategoryList = transaction.Categories.Where(c => c.CategoryId == 0).ToList();

            var transactionId = _idAllocationManager.GetNextId((long)EEntityType.Transaction);
            var categoryIds = _idAllocationManager.GetNextId(newCategoryList.Count, (long)EEntityType.Category);

            transaction.TransactionId = transactionId;

            for (var i = 0; i < categoryIds.Length; i++)
                newCategoryList[i].CategoryId = categoryIds[i];

            _categoryManager.Insert(newCategoryList);
            _categoryCache.AddItems(newCategoryList);

            _transactionManager.Insert(transaction);
            _transactionCache.AddItem(transaction);
        }

        public void Insert(IList<Transaction> transactions)
        {
            var transactionIdList = _idAllocationManager.GetNextId(transactions.Count, (long)EEntityType.Transaction);

            for (int i = 0; i < transactionIdList.Length; i++)
            {
                transactions[i].TransactionId = transactionIdList[i];
            }

            _transactionManager.Insert(transactions);
            _transactionCache.AddItems(transactions);
        }

        public void Update(Transaction transaction)
        {
            var newCategoryList = transaction.Categories.Where(c => c.CategoryId == 0).ToList();
            var categoryIds = _idAllocationManager.GetNextId(newCategoryList.Count, (long)EEntityType.Category);

            for (var i = 0; i < categoryIds.Length; i++)
                newCategoryList[i].CategoryId = categoryIds[i];

            var transactionCategoryList = transaction.Categories.Select(c => new TransactionCategory { TransactionId = transaction.TransactionId, CategoryId = c.CategoryId }).ToList();

            _categoryManager.Insert(newCategoryList);
            _categoryCache.AddItems(newCategoryList);

            var existingTransaction = _transactionCache.GetItem(transaction.Key);
            var existingTransactionCategoryList = existingTransaction.Categories.Select(c => new TransactionCategory { TransactionId = transaction.TransactionId, CategoryId = c.CategoryId }).ToList();
            _transactionCategoryCache.RemoveItems(existingTransactionCategoryList);

            _transactionManager.Update(transaction);
            _transactionCache.UpdateItem(transaction);
            //need to add transaction categories to the cache
        }

        public void Update(IList<Transaction> transactions)
        {
        }

        public void Exclude(long transactionId, bool exclude)
        {
            var transaction = _transactionCache.GetItem(transactionId);

            var transactionInfo = transaction.Info ?? new TransactionInfo();
            transactionInfo.TransactionId = transaction.TransactionId;
            transactionInfo.Excluded = exclude;

            var exists = transaction.Info.TransactionId > 0;

            _transactionManager.Save(transactionInfo);

            if (exists)
                _transactionInfoCache.UpdateItem(transactionInfo);
            else
                _transactionInfoCache.AddItem(transactionInfo);
        }

        public Transaction GetTransaction(long transactionId)
        {
            return _transactionCache.GetItem(transactionId);
        }

        public Transaction GetTransaction(string uniqueId)
        {
            return _transactionCache.GetItem(t => t.UniqueId.Equals(uniqueId, System.StringComparison.OrdinalIgnoreCase));
        }

        public IList<Transaction> GetTransactions(long accountId)
        {
            return _transactionCache.GetFilteredList(t => t.AccountId == accountId);
        }

        public IList<Transaction> GetTransactions(long accountId, BusDate initialDate, DateView dateView, Status status)
        {
            return GetTransactions(accountId, initialDate, dateView, status, null);
        }

        public IList<Transaction> GetTransactions(long accountId, BusDate initialDate, DateView dateView, Status status, IList<Category> withCategories)
        {
            var transactions = _transactionCache.GetFilteredList(TransactionFilter.WithDateBetween(accountId, initialDate, dateView, status));

            if (withCategories == null || withCategories.Count == 0)
            {
                return transactions.OrderByDescending(t => t.Date)
                    .ThenByDescending(t => t.TransactionId)
                    .ToList();
            }

            var filteredList = new List<Transaction>();

            System.Threading.Tasks.Parallel.ForEach(transactions, (t) =>
            {
                var matchingCategories = (from tc in t.Categories
                                          join wc in withCategories on tc.CategoryId equals wc.CategoryId
                                          select tc).Count();
                if (matchingCategories >=  withCategories.Count)
                    filteredList.Add(t);
            });

            return filteredList.OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.TransactionId)
                .ToList();
        }

        public IList<Transaction> GetTransactions(long accountId, BusDate startDate, BusDate endDate, Status status)
        {
            return _transactionCache.GetFilteredList(TransactionFilter.WithDateBetween(accountId, startDate, endDate, status));
        }
    }
}
