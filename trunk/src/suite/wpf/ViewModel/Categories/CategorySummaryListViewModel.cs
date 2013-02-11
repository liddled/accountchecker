using System.Collections.ObjectModel;
using DL.AccountChecker.Model;
using DL.Framework.Common;
using DL.Framework.WPF;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class CategorySummaryListViewModel : NavigationViewModel<ViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        public ObservableCollection<CategorySummaryModel> CategorySummaries { get; set; }

        public CategorySummaryListViewModel()
        {
            CategorySummaries = new AsyncObservableCollection<CategorySummaryModel>();
        }

        public CategorySummaryListViewModel(IRegionManager regionManager, OperationController operationController)
            : this()
        {
            _operationController = operationController;
            _regionManager = regionManager;
        }

        public override void Load()
        {
            CategorySummaries.Clear();

            var categorySummaryList = _operationController.GetCategorySummaries(Status.Active);
            CategorySummaries.AddRange(categorySummaryList);
        }
    }
}
