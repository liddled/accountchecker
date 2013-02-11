using System;
using System.Collections.Generic;
using System.Linq;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using ServiceStack.Redis;

namespace DL.AccountChecker.Repositories.Redis
{
    public class AccountRepository : IAccountRepository
    {
        public IRedisClientsManager RedisManager { get; private set; }

        public AccountRepository(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        public void Insert(Account entity)
        {
            using (var redis = RedisManager.GetClient())
            {
                var redisAccounts = redis.As<Account>();
                redisAccounts.Store(entity);
            }
        }

        public void Insert(IEnumerable<Account> entities)
        {
            using (var redis = RedisManager.GetClient())
            {
                var redisAccounts = redis.As<Account>();
                redisAccounts.StoreAll(entities);
            }
        }

        public void Update(Account entity)
        {
            using (var redis = RedisManager.GetClient())
            {
                var redisAccounts = redis.As<Account>();

                var account = redisAccounts.GetById(entity.AccountId);

                if (account == null)
                    throw new NullReferenceException(String.Format("Account does not exist with AccountId {0}", entity.AccountId));

                redisAccounts.Store(entity);
            }
        }

        public IEnumerable<Account> GetAll()
        {
            return RedisManager.ExecAs<Account>(a => a.GetAll());
        }
    }
}
