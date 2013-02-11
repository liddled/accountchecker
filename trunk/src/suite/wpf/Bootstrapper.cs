using System.Windows;
using DL.AccountChecker.Suite.Service;
using DL.Framework.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace DL.AccountChecker.Suite.WPF
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (ShellView)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var catalog = new ModuleCatalog();
            catalog.AddModule(typeof(ShellModule));
            catalog.AddModule(typeof(AccountModule));
            catalog.AddModule(typeof(CategoryModule));
            catalog.AddModule(typeof(ReportModule));
            catalog.AddModule(typeof(TransactionModule));
            return catalog;
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);

            var serviceProxyManager = new ServiceProxyController();
            var operationController = new OperationController(serviceProxyManager);

            Container.RegisterInstance<OperationController>(operationController);

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(Constants.MainRegionName, AccountSummaryListView.ViewName);
        }
    }
}
