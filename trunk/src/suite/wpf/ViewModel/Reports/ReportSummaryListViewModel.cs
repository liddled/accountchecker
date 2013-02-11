using System.Collections.ObjectModel;
using DL.AccountChecker.Model;
using DL.Framework.Common;
using DL.Framework.WPF;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class ReportSummaryListViewModel : NavigationViewModel<ViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        public ObservableCollection<ReportSummaryModel> ReportSummaries { get; set; }

        public ReportSummaryListViewModel()
        {
            ReportSummaries = new AsyncObservableCollection<ReportSummaryModel>();
        }

        public ReportSummaryListViewModel(IRegionManager regionManager, OperationController operationController)
            : this()
        {
            _operationController = operationController;
            _regionManager = regionManager;
        }

        public override void Load()
        {
            ReportSummaries.Clear();

            var reportSummaryList = _operationController.GetReportSummaries(BusDate.Today, DateView.All, Status.Active);
            ReportSummaries.AddRange(reportSummaryList);
        }
    }
}
