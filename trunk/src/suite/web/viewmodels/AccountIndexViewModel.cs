using System.Collections.Generic;
using DL.AccountChecker.Model;

namespace DL.AccountChecker.Suite.Web
{
    public class AccountIndexViewModel
    {
        public IList<AccountSummaryModel> Views { get; set; }

        public AccountIndexViewModel()
        {
            Views = new List<AccountSummaryModel>();
        }
    }
}
