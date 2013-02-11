using System.Collections.Generic;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    public interface IAccountManagerFactory
    {
        void Insert(Account account);
        void Update(Account account);

        IList<Account> GetAccounts(Status status);
        Account GetAccount(long accountId);
        Account GetAccount(string id);
    }
}
