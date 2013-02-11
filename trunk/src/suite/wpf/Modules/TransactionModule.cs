﻿using DL.Framework.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class TransactionModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public TransactionModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(TransactionSummaryListView));
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(TransactionHistoryView));
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(TransactionAddView));
            _regionManager.RegisterViewWithRegion(Constants.MainRegionName, typeof(TransactionEditView));
        }
    }
}
