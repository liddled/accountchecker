using System.Windows.Input;
using DL.Framework.Common;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class MenuViewModel
    {
        private readonly IRegionManager _regionManager;

        private ICommand _accountsCommand;
        private ICommand _categoriesCommand;
        private ICommand _reportsCommand;
        private ICommand _transactionsCommand;

        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public ICommand AccountsCommand
        {
            get { return _accountsCommand ?? (_accountsCommand = new DelegateCommand(AccountsExecute)); }
        }

        public ICommand CategoriesCommand
        {
            get { return _categoriesCommand ?? (_categoriesCommand = new DelegateCommand(CategoriesExecute)); }
        }

        public ICommand ReportsCommand
        {
            get { return _reportsCommand ?? (_reportsCommand = new DelegateCommand(ReportsExecute)); }
        }

        public ICommand TransactionsCommand
        {
            get { return _transactionsCommand ?? (_transactionsCommand = new DelegateCommand(TransactionsExecute)); }
        }

        public void AccountsExecute()
        {
            _regionManager.RequestNavigate(Constants.MainRegionName, AccountSummaryListView.ViewName);
        }

        public void CategoriesExecute()
        {
            _regionManager.RequestNavigate(Constants.MainRegionName, CategorySummaryListView.ViewName);
        }

        public void ReportsExecute()
        {
            _regionManager.RequestNavigate(Constants.MainRegionName, ReportSummaryListView.ViewName);
        }

        public void TransactionsExecute()
        {
            _regionManager.RequestNavigate(Constants.MainRegionName, TransactionSummaryListView.ViewName);
        }
    }
}