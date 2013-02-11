using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.BusinessLogic
{
	public class CategoryManagerFactory : ICategoryManagerFactory
	{
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICachedTypes _cachedTypes;
        private readonly GenericCache<Category> _categoryCache;

        private readonly ICategoryManager _categoryManager;
        private readonly IIdAllocationManager _idAllocationManager;

        public CategoryManagerFactory(ICachedTypes cachedTypes, ICategoryManager categoryManager, IIdAllocationManager idAllocationManager)
        {
            _cachedTypes = cachedTypes;
            _categoryCache = _cachedTypes.GetCache<Category>();

            _categoryManager = categoryManager;
            _idAllocationManager = idAllocationManager;
        }

        public void LoadCache()
        {
            var items = _categoryManager.GetAllItems();
            _categoryCache.AddItems(items);
        }

        public void Insert(Category category)
        {
            category.CategoryId = _idAllocationManager.GetNextId((long)EEntityType.Category);
            _categoryManager.Insert(category);
            _categoryCache.AddItem(category);
        }

        public void Update(Category category)
        {
            _categoryManager.Update(category);
            _categoryCache.UpdateItem(category);
        }

        public Category GetCategory(long categoryId)
        {
            return _categoryCache.GetItem(categoryId);
        }

        public IList<Category> GetCategories(Status status)
        {
            return _categoryCache.GetFilteredList(c => (status == Status.All || c.Status == status));
        }

        public IList<Category> GetCategories(IList<long> categoryIdList, Status status)
        {
            return _categoryCache.GetFilteredList(c => (status == Status.All || c.Status == status) 
                                                       && categoryIdList.Contains(c.CategoryId));
        }

        public IList<Category> GetCategories(IList<string> categoryNameList, Status status)
        {
            var filteredList = _categoryCache.GetFilteredList(c => (status == Status.All || c.Status == status));
            return (from category in filteredList
                    join c in categoryNameList on category.CategoryName equals c
                    select category).ToList();
        }

	    public IList<Category> SearchCategories(string searchText, Status status)
	    {
            return _categoryCache.GetFilteredList(c => (status == Status.All || c.Status == status)
                                                       && (String.IsNullOrEmpty(searchText) || c.CategoryName.StartsWith(searchText))).ToList();
	    }
    }
}
