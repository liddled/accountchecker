using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.AccountChecker.Model;
using DL.Framework.Common;
using DL.Framework.WPF;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;

namespace DL.AccountChecker.Suite.WPF
{
    public class AccountSummaryListViewModelKey : ViewModelKey
    {
        public BusDate Date { get; set; }
        public DateView DateView { get; set; }
        public Status Status { get; set; }
        public string SortBy { get; set; }
    }

    public class AccountSummaryListViewModel : NavigationViewModel<AccountSummaryListViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        private bool _isSubscribed;
        private const string DefaultSortedBySelectedItem = "Name";

        private ICommand _addCommand;
        private ICommand _editCommand;

        public ObservableCollection<AccountSummaryModel> AccountSummaries { get; set; }

        public AccountSummaryListViewModel()
        {
            AccountSummaries = new AsyncObservableCollection<AccountSummaryModel>();
        }

        public AccountSummaryListViewModel(IRegionManager regionManager, OperationController operationController)
            : this()
        {
            _operationController = operationController;
            _regionManager = regionManager;
        }

        public string SortBySelectedItem
        {
            get { return (ViewModelKey == null) ? DefaultSortedBySelectedItem : ViewModelKey.SortBy; }
            set
            {
                if (ViewModelKey.SortBy.Equals(value, StringComparison.OrdinalIgnoreCase))
                    return;

                var query = new UriQuery
                {
                    { "date", ViewModelKey.Date.Sort8Date.ToString() },
                    { "dateView", ViewModelKey.DateView.ToString() },
                    { "status", ViewModelKey.Status.ToString() },
                    { "sortBy", value }
                };

                var uri = new Uri(AccountSummaryListView.ViewName + query.ToString(), UriKind.Relative);
                _regionManager.RequestNavigate(Constants.MainRegionName, uri);
            }
        }

        protected override AccountSummaryListViewModelKey GetViewModelKey(UriQuery query)
        {
            BusDate date = null;
            DateView dateView;
            Status status;
            string sortBy = null;

            var pDate = query["date"];
            if (String.IsNullOrEmpty(pDate) || !BusDate.TryParseExact(pDate, out date))
                date = BusDate.Today;

            var pDateView = query["dateView"];
            if (String.IsNullOrEmpty(pDateView) || !Enum.TryParse<DateView>(pDateView, out dateView))
                dateView = DateView.Month;

            var pStatus = query["status"];
            if (String.IsNullOrEmpty(pStatus) || !Enum.TryParse<Status>(pStatus, out status))
                status = Status.Active;

            sortBy = !String.IsNullOrEmpty(query["sortBy"]) ? query["sortBy"] : DefaultSortedBySelectedItem;

            return new AccountSummaryListViewModelKey
            {
                Date = date,
                DateView = dateView,
                Status = status,
                SortBy = sortBy
            };
        }

        public override void Load()
        {
            var accountSummaryList = _operationController.GetAccountSummaries(ViewModelKey.Date, ViewModelKey.DateView, ViewModelKey.Status);
            
            IList<AccountSummaryModel> orderedAccountSummaryList;
            switch (ViewModelKey.SortBy ?? String.Empty)
            {
                case "BankCode":
                    orderedAccountSummaryList = accountSummaryList.OrderBy(x => x.Account.BankCode).ToList();
                    break;
                case "CardNumber":
                    orderedAccountSummaryList = accountSummaryList.OrderBy(x => x.Account.CardNumber).ToList();
                    break;
                case "Name":
                default:
                    orderedAccountSummaryList = accountSummaryList.OrderBy(x => x.Account.BankName).ToList();
                    break;
            }

            AccountSummaries.Clear();
            AccountSummaries.AddRange(orderedAccountSummaryList);
        }

        protected override void Subscribe()
        {
            if (_isSubscribed) return;
            _operationController.Subscribe(OnMessage, Topic.Accounts);
            _isSubscribed = true;
        }

        protected override void UnSubscribe()
        {
            if (!_isSubscribed) return;
            _operationController.UnSubscribe(OnMessage, Topic.Accounts);
            _isSubscribed = false;
        }

        private void OnMessage(CacheUpdate cu)
        {
            Load();
        }

        public ICommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new DelegateCommand(AddExecute)); }
        }

        public ICommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand<Account>(EditExecute)); }
        }

        public void AddExecute()
        {
            _regionManager.RequestNavigate(Constants.MainRegionName, AccountAddView.ViewName);
        }

        public void EditExecute(Account account)
        {
            var query = new UriQuery
            {
                { "accountId", account.AccountId.ToString() },
            };

            var uri = new Uri(AccountEditView.ViewName + query.ToString(), UriKind.Relative);
            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }
    }
}
