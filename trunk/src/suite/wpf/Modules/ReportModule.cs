using DL.Framework.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class ReportModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ReportModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(ReportSummaryListView));
        }
    }
}
