using System.Collections.Generic;
using DL.AccountChecker.Model;

namespace DL.AccountChecker.Suite.Web
{
    public class ReportIndexViewModel
    {
        public IList<ReportSummaryModel> Views { get; set; }

        public ReportIndexViewModel()
        {
            Views = new List<ReportSummaryModel>();
        }
    }
}