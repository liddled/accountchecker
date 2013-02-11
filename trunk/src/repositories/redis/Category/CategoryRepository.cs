using System;
using System.Collections.Generic;
using System.Linq;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using ServiceStack.Redis;

namespace DL.AccountChecker.Repositories.Redis
{
    public class CategoryRepository : ICategoryRepository
    {
        public IRedisClientsManager RedisManager { get; private set; }

        public CategoryRepository(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        public void Insert(Category entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(IEnumerable<Category> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAll()
        {
            return RedisManager.ExecAs<Category>(a => a.GetAll());
        }
    }
}
