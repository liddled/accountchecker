using System;
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
    public class TransactionHistoryViewModelKey : ViewModelKey
    {
        public long AccountId { get; set; }
        public BusDate Date { get; set; }
        public DateView DateView { get; set; }
        public string Categories { get; set; }
        public Status Status { get; set; }
    }

    public class TransactionHistoryViewModel : NavigationViewModel<TransactionHistoryViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        private bool _isSubscribed;
        private const DateView DefaultSortedByDateViewSelectedItem = DateView.Month;
        private const Status DefaultSortedByStatusSelectedItem = Status.Active;

        private ICommand _previousHistoryCommand;
        private ICommand _nextHistoryCommand;
        private ICommand _dateViewDayHistoryCommand;
        private ICommand _dateViewMonthHistoryCommand;
        private ICommand _dateViewYearHistoryCommand;
        private ICommand _viewReportsCommand;
        private ICommand _categoryHistoryCommand;
        private ICommand _editTransactionCommand;
        private ICommand _excludIncludeTransactionCommand;

        private TransactionHistoryModel _transactionHistory;

        public TransactionHistoryViewModel()
        {
        }

        public TransactionHistoryViewModel(IRegionManager regionManager, OperationController operationController)
            : this()
        {
            _regionManager = regionManager;
            _operationController = operationController;
        }

        public TransactionHistoryModel TransactionHistory
        {
            get
            {
                return _transactionHistory;
            }
            set
            {
                if (value != _transactionHistory)
                {
                    _transactionHistory = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DateView SortByDateViewSelectedItem
        {
            get { return (ViewModelKey == null) ? DefaultSortedByDateViewSelectedItem : ViewModelKey.DateView; }
            set
            {
                if (ViewModelKey.DateView == value)
                    return;

                var query = new UriQuery
                {
                    { "accountId", _transactionHistory.Account.ToString() },
                    { "date", ViewModelKey.Date.Sort8Date.ToString() },
                    { "dateView", value.ToString() },
                    { "categories", _transactionHistory.Categories.Select(c => c.CategoryName).ConvertListToString(Constants.Space) },
                    { "status", ViewModelKey.Status.ToString() },
                };

                var uri = new Uri(TransactionHistoryView.ViewName + query.ToString(), UriKind.Relative);
                _regionManager.RequestNavigate(Constants.MainRegionName, uri);
            }
        }

        public Status SortByStatusSelectedItem
        {
            get { return (ViewModelKey == null) ? DefaultSortedByStatusSelectedItem : ViewModelKey.Status; }
            set
            {
                if (ViewModelKey.Status == value)
                    return;

                var query = new UriQuery
                {
                    { "accountId", _transactionHistory.Account.ToString() },
                    { "date", ViewModelKey.Date.Sort8Date.ToString() },
                    { "dateView", _transactionHistory.DateView.ToString() },
                    { "categories", _transactionHistory.Categories.Select(c => c.CategoryName).ConvertListToString(Constants.Space) },
                    { "status", value.ToString() },
                };

                var uri = new Uri(TransactionHistoryView.ViewName + query.ToString(), UriKind.Relative);
                _regionManager.RequestNavigate(Constants.MainRegionName, uri);
            }
        }

        protected override TransactionHistoryViewModelKey GetViewModelKey(UriQuery query)
        {
            long accountId;
            BusDate date = null;
            DateView dateView;
            string categories = null;
            Status status;

            var pAccountId = query["accountId"];
            if (String.IsNullOrEmpty(pAccountId) || !long.TryParse(pAccountId, out accountId))
                throw new ApplicationException("No AccountId specified");

            var pDate = query["date"];
            if (String.IsNullOrEmpty(pDate) || !BusDate.TryParseExact(pDate, out date))
                date = BusDate.Today;

            var pDateView = query["dateView"];
            if (String.IsNullOrEmpty(pDateView) || !Enum.TryParse<DateView>(pDateView, out dateView))
                dateView = DefaultSortedByDateViewSelectedItem;

            var pCategories = query["categories"];
            if (!String.IsNullOrEmpty(pCategories))
                categories = pCategories;

            var pStatus = query["status"];
            if (String.IsNullOrEmpty(pStatus) || !Enum.TryParse<Status>(pStatus, out status))
                status = DefaultSortedByStatusSelectedItem;

            return new TransactionHistoryViewModelKey
            {
                AccountId = accountId,
                Date = date,
                DateView = dateView,
                Categories = categories,
                Status = status
            };
        }

        public override void Load()
        {
            var categoryNames = ViewModelKey.Categories.SplitToList(true);
            var account = _operationController.GetAccount(ViewModelKey.AccountId);
            var categoryList = _operationController.GetCategories(categoryNames);
            var transactionList = _operationController.GetTransactions(ViewModelKey.AccountId, ViewModelKey.Date, ViewModelKey.DateView, ViewModelKey.Status, categoryNames);

            var outcome = transactionList.Where(t => !t.Info.Excluded && t.Amount < 0).Sum(t => t.Amount);
            var income = transactionList.Where(t => !t.Info.Excluded && t.Amount > 0).Sum(t => t.Amount);

            TransactionHistory = new TransactionHistoryModel
            {
                Account = account,
                Date = BusDate.GetDate(ViewModelKey.Date, ViewModelKey.DateView),
                DateView = ViewModelKey.DateView,
                Categories = categoryList,
                TransactionCount = transactionList.Count,
                Outcome = outcome,
                Income = income,
                Transactions = transactionList
            };
        }

        protected override void Subscribe()
        {
            if (_isSubscribed) return;
            _operationController.Subscribe(OnMessage, Topic.Transactions);
            _isSubscribed = true;
        }

        protected override void UnSubscribe()
        {
            if (!_isSubscribed) return;
            _operationController.UnSubscribe(OnMessage, Topic.Transactions);
            _isSubscribed = false;
        }

        private void OnMessage(CacheUpdate cu)
        {
            try
            {
                foreach (var item in cu.UpdatedItems)
                {
                    var transaction = item.UpdatedItem as Transaction;

                    if (transaction == null)
                        continue;

                    switch (cu.Reason)
                    {
                        case EventAction.Delete:
                            _transactionHistory.Transactions.Remove(transaction);
                            break;
                        case EventAction.Update:
                            _transactionHistory.Transactions.Remove(transaction);
                            _transactionHistory.Transactions.Add(transaction);
                            break;
                        case EventAction.Add:
                        default:
                            _transactionHistory.Transactions.Add(transaction);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                var a = "";
            }
        }

        public ICommand PreviousHistoryCommand
        {
            get { return _previousHistoryCommand ?? (_previousHistoryCommand = new DelegateCommand(PreviousHistoryExecute)); }
        }

        public ICommand NextHistoryCommand
        {
            get { return _nextHistoryCommand ?? (_nextHistoryCommand = new DelegateCommand(NextHistoryExecute)); }
        }

        public ICommand DateViewDayHistoryCommand
        {
            get { return _dateViewDayHistoryCommand ?? (_dateViewDayHistoryCommand = new DelegateCommand<DateTime?>((d) => DateViewHistoryExecute(d.Value, DateView.Day))); }
        }

        public ICommand DateViewMonthHistoryCommand
        {
            get { return _dateViewMonthHistoryCommand ?? (_dateViewMonthHistoryCommand = new DelegateCommand<DateTime?>((d) => DateViewHistoryExecute(d.Value, DateView.Month))); }
        }

        public ICommand DateViewYearHistoryCommand
        {
            get { return _dateViewYearHistoryCommand ?? (_dateViewYearHistoryCommand = new DelegateCommand<DateTime?>((d) => DateViewHistoryExecute(d.Value, DateView.Year))); }
        }

        public ICommand ViewReportsCommand
        {
            get { return _viewReportsCommand ?? (_viewReportsCommand = new DelegateCommand(ViewReportsExecute)); }
        }

        public ICommand CategoryHistoryCommand
        {
            get { return _categoryHistoryCommand ?? (_categoryHistoryCommand = new DelegateCommand<string>(CategoryHistoryExecute)); }
        }

        public ICommand EditTransactionCommand
        {
            get { return _editTransactionCommand ?? (_editTransactionCommand = new DelegateCommand<long?>(EditTransactionExecute)); }
        }

        public ICommand ExcludeIncludeTransactionCommand
        {
            get { return _excludIncludeTransactionCommand ?? (_excludIncludeTransactionCommand = new DelegateCommand<long?>(ExcludeIncludeTransactionExecute)); }
        }        

        public void PreviousHistoryExecute()
        {
            var uriQuery = new UriQuery
            {
                { "accountId", _transactionHistory.Account.ToString() },
                { "categories", _transactionHistory.Categories.Select(c => c.CategoryName).ConvertListToString(Constants.Space) },
                { "date", BusDate.GetPreviousDate(_transactionHistory.Date, _transactionHistory.DateView).Sort8Date.ToString() },
                { "dateView", _transactionHistory.DateView.ToString() },
            };
            var uri = new Uri(TransactionHistoryView.ViewName + uriQuery, UriKind.Relative);

            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }

        public void NextHistoryExecute()
        {
            var uriQuery = new UriQuery
            {
                { "accountId", _transactionHistory.Account.ToString() },
                { "categories", _transactionHistory.Categories.Select(c => c.CategoryName).ConvertListToString(Constants.Space) },
                { "date", BusDate.GetNextDate(_transactionHistory.Date, _transactionHistory.DateView).Sort8Date.ToString() },
                { "dateView", _transactionHistory.DateView.ToString() },
            };
            var uri = new Uri(TransactionHistoryView.ViewName + uriQuery, UriKind.Relative);

            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }

        public void DateViewHistoryExecute(DateTime date, DateView dateView)
        {
            var busDate = new BusDate(date);

            var uriQuery = new UriQuery
            {
                { "accountId", _transactionHistory.Account.ToString() },
                { "categories", _transactionHistory.Categories.Select(c => c.CategoryName).ConvertListToString(Constants.Space) },
                { "date", busDate.Sort8Date.ToString() },
                { "dateView", dateView.ToString() },
            };
            var uri = new Uri(TransactionHistoryView.ViewName + uriQuery, UriKind.Relative);

            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }

        public void ViewReportsExecute()
        {
            var fromDate = BusDate.GetDate(_transactionHistory.Date, _transactionHistory.DateView);
            var endDate = BusDate.GetNextDate(_transactionHistory.Date, _transactionHistory.DateView);

            var uriQuery = new UriQuery
            {
                { "fromDate", fromDate.Sort8Date.ToString() },
                { "endDate", endDate.Sort8Date.ToString() },
            };
            var uri = new Uri(ReportSummaryListView.ViewName + uriQuery, UriKind.Relative);

            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }

        public void CategoryHistoryExecute(string categoryName)
        {
            var categoryList = _transactionHistory.Categories.Select(c => c.CategoryName).ToList();
            if (categoryList.Contains(categoryName)) categoryList.Remove(categoryName);
            else categoryList.Add(categoryName);

            var uriQuery = new UriQuery
            {
                { "accountId", _transactionHistory.Account.ToString() },
                { "categories", categoryList.ConvertListToString(Constants.Space) },
                { "date", _transactionHistory.Date.Sort8Date.ToString() },
                { "dateView", _transactionHistory.DateView.ToString() },
            };
            var uri = new Uri(TransactionHistoryView.ViewName + uriQuery, UriKind.Relative);

            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }

        public void EditTransactionExecute(long? transactionId)
        {
            var query = new UriQuery();
            query.Add("transactionId", transactionId.ToString());

            var uri = new Uri(TransactionEditView.ViewName + query.ToString(), UriKind.Relative);
            _regionManager.RequestNavigate(Constants.MainRegionName, uri);
        }

        public void ExcludeIncludeTransactionExecute(long? transactionId)
        {
            if (!transactionId.HasValue)
                return;

            var existingTransaction = _transactionHistory.Transactions.SingleOrDefault(t => t.TransactionId == transactionId.Value);
            if (existingTransaction == null)
                return;

            var excluded = !existingTransaction.Info.Excluded;
            existingTransaction.Info.Excluded = excluded;

            _operationController.ExcludeTransaction(transactionId.Value, excluded);

            if (excluded)
            {
                
            }
            else
            {
            }
        }
    }
}
