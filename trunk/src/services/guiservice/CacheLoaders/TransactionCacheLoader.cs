using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Services.GuiService
{
    public class TransactionCacheLoader : ICacheLoader
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly Action<CacheUpdate> _transactionInfoCacheUpdate;
        private readonly Action<CacheUpdate> _transactionCategoryCacheUpdate;

        private readonly GenericCache<Category> _categoryCache;
        private readonly GenericCache<Transaction> _transactionCache;
        private readonly GenericCache<TransactionInfo> _transactionInfoCache;
        private readonly GenericCache<TransactionCategory> _transactionCategoryCache;
        private readonly ITransactionRepository _repository;

        public TransactionCacheLoader(ICachedTypes cachedTypes, ITransactionRepository repository)
        {
            _categoryCache = cachedTypes.GetCache<Category>();
            _transactionCache = cachedTypes.GetCache<Transaction>();
            _transactionInfoCache = cachedTypes.GetCache<TransactionInfo>();
            _transactionCategoryCache = cachedTypes.GetCache<TransactionCategory>();
            _repository = repository;

            _transactionInfoCacheUpdate = (cu) =>
            {
                foreach (var transactionInfo in cu.UpdatedItems.Select(u => u.UpdatedItem).Cast<TransactionInfo>())
                {
                    var transaction = _transactionCache.GetItem(transactionInfo.TransactionId);
                    transaction.Info = transactionInfo;
                }
            };

            _transactionCategoryCacheUpdate = (cu) =>
            {
                foreach (var transactionCategory in cu.UpdatedItems.Select(u => u.UpdatedItem).Cast<TransactionCategory>())
                {
                    var transaction = _transactionCache.GetItem(transactionCategory.TransactionId);
                    var category = _categoryCache.GetItem(transactionCategory.CategoryId);

                    var existingCategory = transaction.Categories.SingleOrDefault(c => c.CategoryId == category.CategoryId);
                    if (existingCategory != null)
                        transaction.Categories.Remove(existingCategory);
                    transaction.Categories.Add(category);

                    transaction.Categories = transaction.Categories.OrderBy(c => c.CategoryName).ToList();
                }
            };
        }

        public void Start()
        {
            _transactionInfoCache.AddEventHandler(_transactionInfoCacheUpdate);
            _transactionCategoryCache.AddEventHandler(_transactionCategoryCacheUpdate);

            Log.Info("Loading Transaction cache");
            var transactions = _repository.GetAll();
            _transactionCache.AddItems(transactions);

            var transactionCategories = _repository.GetAllTransactionCategories();
            _transactionCategoryCache.AddItems(transactionCategories);

            var transactionInfos = _repository.GetAllTransactionInfos();
            _transactionInfoCache.AddItems(transactionInfos);

            _transactionInfoCache.RemoveEventHandler(_transactionInfoCacheUpdate);
            _transactionCategoryCache.RemoveEventHandler(_transactionCategoryCacheUpdate);

            /*_categoryCache.AddEventHandler(new FilteredEventHandler((cu) =>
            {
                if (cu.Reason != EventAction.Update)
                    return;

                foreach (var category in cu.UpdatedItems.Select(u => u.UpdatedItem).Cast<Category>())
                {
                    _transactionCache.UpdateItems<Category>(category, (items) =>
                    {
                        return (from t in items
                                from c in t.Categories
                                where c.CategoryId == category.CategoryId
                                select c).ToList();
                    });
                }
            }));*/
        }

        public void Stop()
        {
        }
    }
}
