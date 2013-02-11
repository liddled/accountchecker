using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;

namespace DL.AccountChecker.BusinessLogic
{
	public class AccountManager : IAccountManager
	{
        private readonly IAccountRepository _accountRepository;

        public AccountManager(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public void Insert(Account account)
        {
            _accountRepository.Insert(account);
        }

        public void Update(Account account)
        {
            _accountRepository.Update(account);
        }

        public IEnumerable<Account> GetAllItems()
        {
            return _accountRepository.GetAll();
        }
	}
}
