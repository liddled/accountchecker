using DL.Framework.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class AccountModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public AccountModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(AccountSummaryListView));
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(AccountAddView));
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(AccountEditView));
        }
    }
}
