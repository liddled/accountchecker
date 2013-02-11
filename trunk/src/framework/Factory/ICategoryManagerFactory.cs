using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    public interface ICategoryManagerFactory
    {
        void Insert(Category category);
        void Update(Category category);

        Category GetCategory(long categoryId);
        IList<Category> GetCategories(Status status);
        IList<Category> GetCategories(IList<long> categoryIdList, Status status);
        IList<Category> GetCategories(IList<string> categoryNameList, Status status);
        IList<Category> SearchCategories(string search, Status status);
    }
}
