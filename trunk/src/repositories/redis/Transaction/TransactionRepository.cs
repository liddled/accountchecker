using System;
using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using ServiceStack.Redis;

namespace DL.AccountChecker.Repositories.Redis
{
    public class TransactionRepository : ITransactionRepository
    {
        public IRedisClientsManager RedisManager { get; private set; }

        public TransactionRepository(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        public void Insert(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<Transaction> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<Transaction> entities)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Transaction> GetAll()
        {
            return RedisManager.ExecAs<Transaction>(a => a.GetAll());
        }

        public IEnumerable<TransactionCategory> GetAllTransactionCategories()
        {
            return RedisManager.ExecAs<TransactionCategory>(a => a.GetAll());
        }

        public void Save(TransactionInfo transactionInfo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransactionInfo> GetAllTransactionInfos()
        {
            return RedisManager.ExecAs<TransactionInfo>(a => a.GetAll());
        }
    }
}
