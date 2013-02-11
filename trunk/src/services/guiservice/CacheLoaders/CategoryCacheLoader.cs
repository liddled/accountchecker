using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Services.GuiService
{
    public class CategoryCacheLoader : ICacheLoader
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly GenericCache<Category> _cache;
        private readonly ICategoryRepository _repository;

        public CategoryCacheLoader(ICachedTypes cachedTypes, ICategoryRepository repository)
        {
            _cache = cachedTypes.GetCache<Category>();
            _repository = repository;
        }

        public void Start()
        {
            Log.Info("Loading Category cache");
            var items = _repository.GetAll();
            _cache.AddItems(items);
        }

        public void Stop()
        {

        }
    }
}
