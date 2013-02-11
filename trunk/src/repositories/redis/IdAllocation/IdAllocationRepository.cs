using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using ServiceStack.Redis;

namespace DL.AccountChecker.Repositories.Redis
{
	public class IdAllocationRepository : IIdAllocationRepository
	{
        public IRedisClientsManager RedisManager { get; private set; }

        public IdAllocationRepository(IRedisClientsManager redisManager)
        {
            RedisManager = redisManager;
        }

        public long[] GetNextIds(int idCount, long entityTypeId)
        {
            var ids = new long[idCount];

            long lastId = 0;
            switch ((EEntityType)entityTypeId)
            {
                case EEntityType.Account:
                    RedisManager.ExecAs<Account>((c) =>
                    {
                        lastId = c.GetNextSequence(idCount);
                    });
                    break;
                case EEntityType.Category:
                    RedisManager.ExecAs<Category>((c) =>
                    {
                        lastId = c.GetNextSequence(idCount);
                    });
                    break;
                case EEntityType.Transaction:
                    RedisManager.ExecAs<Transaction>((c) =>
                    {
                        lastId = c.GetNextSequence(idCount);
                    });
                    break;
            }

            for (int i = ids.Length - 1; i >= 0; i--)
            {
                ids[i] = lastId--;
            }

            return ids;
        }
    }
}
