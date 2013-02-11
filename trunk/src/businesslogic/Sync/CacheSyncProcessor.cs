using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.BusinessLogic
{
    public class CacheSyncProcessor : IMessageProcessor
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICachedTypes _cachedTypes;
        private readonly ITransactionManagerFactory _transactionManagerFactory;

        private readonly GenericCache<Transaction> _transactionCache;
        private readonly GenericCache<Category> _categoryCache;

        public CacheSyncProcessor(ICachedTypes cachedTypes, ITransactionManagerFactory transactionManagerFactory)
        {
            _cachedTypes = cachedTypes;
            _transactionManagerFactory = transactionManagerFactory;

            _transactionCache = _cachedTypes.GetCache<Transaction>();
            _categoryCache = _cachedTypes.GetCache<Category>();
        }

        public void Process(IMessage message)
        {
            var transactions = SerializeHelper.ByteArrayToObject<IList<Transaction>>(message.Data);

            var insertTransactionList = new List<Transaction>();
            var updateTransactionList = new List<Transaction>();

            foreach (var t in transactions)
            {
                var existingTransaction = (t.TransactionId > 0) ? _transactionCache.GetItem(t.TransactionId) : _transactionCache.GetItem(i => i.UniqueId.Equals(t.UniqueId));
                if (existingTransaction != null)
                {
                    if (IsTransactionModified(t, existingTransaction))
                        updateTransactionList.Add(t);
                }
                else
                {
                    UpdateCategories(t);
                    insertTransactionList.Add(t);
                }
            }

            if (insertTransactionList.Count > 0)
            {
                Log.DebugFormat("Inserting transactions: {0}", insertTransactionList.Count);
                _transactionManagerFactory.Insert(insertTransactionList);
            }

            if (updateTransactionList.Count > 0)
            {
                Log.DebugFormat("Updating transactions: {0}", updateTransactionList.Count);
                _transactionManagerFactory.Update(updateTransactionList);
            }
        }

        public void UpdateCategories(Transaction transaction)
        {
            var findCategories = transaction.Description.Replace("'", String.Empty).SplitToList();

            var matchingCategories = _categoryCache.GetFilteredList<string, string>((c) => c.Status == Status.Active,
                                                                                    findCategories,
                                                                                    (c) => c.CategoryName.ToLower(),
                                                                                    (f) => f.ToLower(),
                                                                                    (c, f) => c);

            if (matchingCategories != null && matchingCategories.Count > 0)
            {
                transaction.Categories = matchingCategories;
            }
        }

        private static bool IsTransactionModified(Transaction transaction, Transaction existingTransaction)
        {
            var isModified = false;

            if (transaction.Date != existingTransaction.Date)
            {
                transaction.Date = existingTransaction.Date;
                isModified = true;
            }

            if (transaction.Description != existingTransaction.Description)
            {
                transaction.Description = existingTransaction.Description;
                isModified = true;
            }

            if (transaction.Amount != existingTransaction.Amount)
            {
                transaction.Amount = existingTransaction.Amount;
                isModified = true;
            }

            if (transaction.Status != Status.Active)
            {
                transaction.Status = Status.Active;
                isModified = true;
            }

            return isModified;
        }
    }
}
