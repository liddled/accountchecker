using System;
using System.Windows.Input;
using DL.AccountChecker.Domain;
using DL.Framework.Common;
using DL.Framework.WPF;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class TransactionEditViewModelKey : ViewModelKey
    {
        public long TransactionId { get; set; }
    }

    public class TransactionEditViewModel : NavigationViewModel<TransactionEditViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        private ICommand _saveCommand;

        public TransactionEditViewModel()
        {
        }

        public TransactionEditViewModel(IRegionManager regionManager, OperationController operationController)
            : this()
        {
            _regionManager = regionManager;
            _operationController = operationController;
        }

        private Transaction _transaction;
        public Transaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                if (value != _transaction)
                {
                    _transaction = value;
                    NotifyPropertyChanged();
                }
            }
        }

        protected override TransactionEditViewModelKey GetViewModelKey(UriQuery query)
        {
            if (String.IsNullOrEmpty(query["transactionId"]))
                return null;

            long transactionId;
            if (!long.TryParse(query["transactionId"], out transactionId))
                return null;

            return new TransactionEditViewModelKey
            {
                TransactionId = transactionId
            };
        }

        public override void Load()
        {
            Transaction = _operationController.GetTransaction(ViewModelKey.TransactionId);
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
                _operationController.UpdateTransaction(transaction);
            }
            catch (Exception ex)
            {
                //keep on same page and report error
            }

            _regionManager.RequestNavigate(Constants.MainRegionName, AccountSummaryListView.ViewName);
        }
    }
}
