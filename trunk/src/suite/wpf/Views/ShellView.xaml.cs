using System;
using System.Windows;
using Common.Logging;
using DL.Framework.Common;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public partial class ShellView
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        public ShellView(IRegionManager regionManager)
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                regionManager.Regions[Constants.MainRegionName].NavigationService.Navigating += NavigationNavigating;
                regionManager.Regions[Constants.MainRegionName].NavigationService.Navigated += NavigationNavigated;
                regionManager.Regions[Constants.MainRegionName].NavigationService.NavigationFailed += NavigationFailed;
            };
        }

        private static void NavigationNavigating(object sender, RegionNavigationEventArgs e)
        {
            Log.DebugFormat("Navigating to {0}", e.Uri);
        }

        private static void NavigationNavigated(object sender, RegionNavigationEventArgs e)
        {
            Log.DebugFormat("Navigated to {0}", e.Uri);
        }

        private static void NavigationFailed(object sender, RegionNavigationFailedEventArgs e)
        {
            const string navigationFailedMessage = "Navigation Failed";
            const string navigationFailedMessageFormatString = "Error navigating to {0}";

            Log.ErrorFormat(navigationFailedMessageFormatString, e.Error, e.Uri);

            MessageBox.Show(String.Format(navigationFailedMessageFormatString, e.Uri),
                navigationFailedMessage, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
