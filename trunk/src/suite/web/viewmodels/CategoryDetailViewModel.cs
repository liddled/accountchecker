using System.Collections.Generic;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Suite.Web
{
    public class CategoryDetailViewModel
    {
        public Account Account { get; set; }
        public int TotalCategories { get; set; }
        public IDictionary<Category, int> GroupedCategories { get; set; }
    }
}