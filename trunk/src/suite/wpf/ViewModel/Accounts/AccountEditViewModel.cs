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
    public class AccountEditViewModelKey : ViewModelKey
    {
        public long AccountId { get; set; }
    }

    public class AccountEditViewModel : NavigationViewModel<AccountEditViewModelKey>
    {
        private readonly IRegionManager _regionManager;
        private readonly OperationController _operationController;

        private ICommand _saveCommand;

        private Account _account;
        public Account Account
        {
            get
            {
                return _account;
            }
            set
            {
                if (value != _account)
                {
                    _account = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public AccountEditViewModel()
        {
        }

        public AccountEditViewModel(IRegionManager regionManager, OperationController operationController)
            : this()
        {
            _regionManager = regionManager;
            _operationController = operationController;
        }

        protected override AccountEditViewModelKey GetViewModelKey(UriQuery query)
        {
            if (String.IsNullOrEmpty(query["accountId"]))
                return null;

            long accountId;
            if (!long.TryParse(query["accountId"], out accountId))
                return null;

            return new AccountEditViewModelKey
            {
                AccountId = accountId
            };
        }

        public override void Load()
        {
            Account = _operationController.GetAccount(ViewModelKey.AccountId);
        }

        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand<Account>(SaveExecute)); }
        }

        public void SaveExecute(Account account)
        {
            //require validation logic

            try
            {
                _operationController.UpdateAccount(account);
            }
            catch (Exception ex)
            {
                //keep on same page and report error
            }

            var query = new UriQuery();
            _regionManager.RequestNavigate(Constants.MainRegionName, AccountSummaryListView.ViewName);
        }
    }
}
