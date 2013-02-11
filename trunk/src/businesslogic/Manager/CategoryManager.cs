using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;

namespace DL.AccountChecker.BusinessLogic
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Insert(Category category)
        {
            _categoryRepository.Insert(category);
        }

        public void Insert(IList<Category> categories)
        {
            _categoryRepository.Insert(categories);
        }

        public void Update(Category category)
        {
            _categoryRepository.Update(category);
        }

        public IEnumerable<Category> GetAllItems()
        {
            return _categoryRepository.GetAll();
        }
    }
}
