using System;
using System.Windows.Input;
using DL.AccountChecker.Domain;
using DL.Framework.Common;
using DL.Framework.WPF;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class TransactionAddViewModel : NavigationViewModel<ViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        private ICommand _saveCommand;

        public TransactionAddViewModel()
        {
        }

        public TransactionAddViewModel(IRegionManager regionManager, OperationController operationController)
            : this()
        {
            _regionManager = regionManager;
            _operationController = operationController;
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand<Transaction>(SaveExecute)); }
        }

        public void SaveExecute(Transaction transaction)
        {
            //require validation logic

            try
            {
                _operationController.InsertTransaction(transaction);
            }
            catch (Exception ex)
            {
                //keep on same page and report error
            }

            _regionManager.RequestNavigate(Constants.MainRegionName, AccountSummaryListView.ViewName);
        }
    }
}
