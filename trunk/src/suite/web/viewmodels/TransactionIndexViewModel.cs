using System.Collections.Generic;
using DL.AccountChecker.Model;

namespace DL.AccountChecker.Suite.Web
{
    public class TransactionIndexViewModel
    {
        public IList<TransactionSummaryModel> Views { get; set; }

        public TransactionIndexViewModel()
        {
            Views = new List<TransactionSummaryModel>();
        }
    }
}
