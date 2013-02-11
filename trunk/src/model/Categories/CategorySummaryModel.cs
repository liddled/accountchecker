using System;
using System.Collections.Generic;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Model
{
    [Serializable]
    public class CategorySummaryModel
    {
        public Account Account { get; set; }
        public IList<Category> Categories { get; set; }
    }
}
