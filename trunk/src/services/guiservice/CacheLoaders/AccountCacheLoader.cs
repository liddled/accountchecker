using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Services.GuiService
{
    public class AccountCacheLoader : ICacheLoader
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly GenericCache<Account> _cache;
        private readonly IAccountRepository _repository;

        public AccountCacheLoader(ICachedTypes cachedTypes, IAccountRepository repository)
        {
            _cache = cachedTypes.GetCache<Account>();
            _repository = repository;
        }

        public void Start()
        {
            Log.Info("Loading Account cache");
            var items = _repository.GetAll();
            _cache.AddItems(items);
        }

        public void Stop()
        {

        }
    }
}
