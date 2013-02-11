using System.Collections.Generic;
using DL.AccountChecker.Domain;

namespace DL.AccountChecker.Framework
{
    public interface IAccountManager
    {
        void Insert(Account account);
        void Update(Account account);

        IEnumerable<Account> GetAllItems();
    }
}
