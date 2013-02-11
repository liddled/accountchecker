using DL.Framework.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DL.AccountChecker.Suite.WPF
{
    public class ShellModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public ShellModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            var menuView = _container.Resolve(typeof(MenuView));
            _regionManager.RegisterViewWithRegion(Constants.MenuRegionName, () => menuView);
            _regionManager.Regions[Constants.MenuRegionName].Activate(menuView);
        }
    }
}
