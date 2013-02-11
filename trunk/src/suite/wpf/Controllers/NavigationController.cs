/*using System;
using System.Linq;
using System.Windows;
using DL.Framework.WPF;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DL.AccountChecker.Suite.WPF
{
    public class NavigationController : INavigationController
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public NavigationController(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        private IRegion GetRegion(string regionName)
        {
            if (!_regionManager.Regions.ContainsRegionWithName(regionName))
                throw new ApplicationException("No region found with name " + regionName);

            return _regionManager.Regions[regionName];
        }

        public void RegisterViewWithRegion(string viewName, string regionName, Type viewType)
        {
            var region = GetRegion(regionName);
            var view = _container.Resolve(viewType);
            region.Add(view, viewName);

            //CommandViewDefinition definition = new CommandViewDefinition()
            //{
            //    CommandName = commandName,
            //    Command = new DelegateCommand<object>(o =>
            //    {
            //        object view = container.Resolve(viewType);
            //        IRegion region = regionManager.Regions[regionName];
            //        region.Add(view, viewName);
            //        region.Activate(view);
            //    })
            //};
            //commands.Add(definition);
            //FirePropertyChanged("Commands");
        }

        public void RemoveViewFromRegion(string viewName, string regionName)
        {
            var region = GetRegion(regionName);
            var view = region.GetView(viewName);

            region.Remove(view);
        }

        public void Transition(string viewName, string regionName)
        {
            var region = GetRegion(regionName);

            var currentView = region.ActiveViews.FirstOrDefault() as FrameworkElement;
            var newView = region.GetView(viewName) as FrameworkElement;

            var container = currentView.Parent as FrameworkElement;

            Transitions.FadeTransition(currentView, newView);
            //Transitions.SinglePaneRightToLeftTransition(container, currentView, newView);

            if (newView == null)
                return;

            region.Activate(newView);

            var loadableContext = newView.DataContext as ILoadable;
            if (loadableContext != null)
                loadableContext.Load(null);
        }

        public void LoadModal(string viewName, object param)
        {
            //we want to fade out and lock the shell and display the modal box

        }
    }
}
*/