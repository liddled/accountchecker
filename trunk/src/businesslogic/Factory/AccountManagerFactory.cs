using System;
using System.Collections.Generic;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.BusinessLogic
{
	public class AccountManagerFactory : IAccountManagerFactory
	{
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICachedTypes _cachedTypes;
        private readonly GenericCache<Account> _accountCache;

        private readonly IAccountManager _accountManager;
        private readonly IIdAllocationManager _idAllocationManager;

        public AccountManagerFactory(ICachedTypes cachedTypes, IAccountManager accountManager, IIdAllocationManager idAllocationManager)
        {
            _cachedTypes = cachedTypes;
            _accountCache = _cachedTypes.GetCache<Account>();

            _accountManager = accountManager;
            _idAllocationManager = idAllocationManager;
        }

        public void Insert(Account account)
        {
            account.AccountId = _idAllocationManager.GetNextId((long)EEntityType.Account);
            _accountManager.Insert(account);
            _accountCache.AddItem(account);
        }

        public void Update(Account account)
        {
            _accountManager.Update(account);
            _accountCache.UpdateItem(account);
        }

        public IList<Account> GetAccounts(Status status)
        {
            return _accountCache.GetFilteredList(a => a.Status == status);
        }

        public Account GetAccount(long accountId)
        {
            return _accountCache.GetItem(accountId);
        }

        public Account GetAccount(string id)
        {
            return _accountCache.GetItem(a => (a.BankCode + a.CardNumber).Equals(id, StringComparison.OrdinalIgnoreCase));
        }
    }
}
