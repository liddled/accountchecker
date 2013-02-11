using System.Collections.Generic;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Framework
{
    public interface ICategoryManager
    {
        void Insert(Category category);
        void Insert(IList<Category> categories);
        void Update(Category category);

        IEnumerable<Category> GetAllItems();
    }
}
