using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DL.AccountChecker.Model;
using DL.Framework.Common;
using DL.Framework.WPF;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class TransactionSummaryListViewModelKey : ViewModelKey
    {
        public BusDate Date { get; set; }
        public DateView DateView { get; set; }
        public Status Status { get; set; }
    }

    public class TransactionSummaryListViewModel : NavigationViewModel<TransactionSummaryListViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        private ICommand _addCommand;
        private ICommand _viewHistoryCommand;

        public ObservableCollection<TransactionSummaryModel> TransactionSummaries { get; private set; }

        public TransactionSummaryListViewModel(IRegionManager regionManager, OperationController operationController)
        {
            _regionManager = regionManager;
            _operationController = operationController;
            TransactionSummaries = new ObservableCollection<TransactionSummaryModel>();
        }

        protected override TransactionSummaryListViewModelKey GetViewModelKey(UriQuery query)
        {
            return new TransactionSummaryListViewModelKey
            {
                Date = BusDate.Today,
                DateView = DateView.All,
                Status = Status.Active
            };
        }

        public override void Load()
        {
            TransactionSummaries.Clear();

            var transactionSummaryList = _operationController.GetTransactionSummaries(ViewModelKey.Date, ViewModelKey.DateView, ViewModelKey.Status);
            TransactionSummaries.AddRange(transactionSummaryList);
        }

        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new DelegateCommand(AddExecute)); }
        }

        public ICommand ViewHistoryCommand
        {
            get { return _viewHistoryCommand ?? (_viewHistoryCommand = new DelegateCommand<long?>(ViewHistoryExecute)); }
        }

        public void AddExecute()
        {
            _regionManager.RequestNavigate(Constants.MainRegionName, TransactionAddView.ViewName);
        }

        public void ViewHistoryExecute(long? accountId)
        {
            var uriQuery = new UriQuery
            {
                { "accountId", accountId.ToString() }
            };
            var uri = new Uri(TransactionHistoryView.ViewName + uriQuery, UriKind.Relative);

            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }
    }
}
