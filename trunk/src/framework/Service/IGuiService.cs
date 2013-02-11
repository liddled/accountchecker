using System.Collections.Generic;
using System.ServiceModel;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    [ServiceContract]
    [ServiceKnownType(typeof(BusDate))]
    [ServiceKnownType(typeof(Status))]
    [ServiceKnownType(typeof(Account))]
    [ServiceKnownType(typeof(Category))]
    [ServiceKnownType(typeof(Transaction))]
    public interface IGuiService
    {
        [OperationContract]
        void InsertAccount(Account account);

        [OperationContract]
        void UpdateAccount(Account account);

        [OperationContract]
        IList<Account> GetAccounts(Status status);

        [OperationContract]
        Account GetAccount(long accountId);

        [OperationContract(Name="GetAccountById")]
        Account GetAccount(string id);

        [OperationContract]
        decimal GetBalance(long accountId, BusDate endDate, Status status);

        [OperationContract]
        void UpdateCategory(Category category);

        [OperationContract]
        Category GetCategory(long categoryId);

        [OperationContract]
        IList<Category> GetCategories(Status status);

        [OperationContract(Name = "GetCategoriesByIdStatus")]
        IList<Category> GetCategories(long accountId, Status status);

        [OperationContract(Name = "GetCategoriesByNamesStatus")]
        IList<Category> GetCategories(IList<string> categoryNames);

        [OperationContract(Name = "GetCategoriesBySearchText")]
        IList<Category> GetCategories(string searchText);

        [OperationContract]
        IDictionary<Account, IList<Category>> GetAccountCategories(Status status);

        [OperationContract]
        void InsertTransaction(Transaction transaction);

        [OperationContract]
        void UpdateTransaction(Transaction transaction);

        [OperationContract]
        void ExcludeTransaction(long transactionId, bool exclude);

        [OperationContract]
        Transaction GetTransaction(long transactionId);

        [OperationContract(Name = "GetTransactionByUniqueId")]
        Transaction GetTransaction(string uniqueId);

        [OperationContract]
        IList<Transaction> GetTransactions(long accountId, BusDate startDate, BusDate endDate, Status status);

        [OperationContract(Name = "GetTransactionsByCategories")]
        IList<Transaction> GetTransactions(long accountId, BusDate initialDate, DateView dateView, Status status, IList<string> categoryNames);

        [OperationContract]
        IDictionary<Account, IList<Transaction>> GetAccountTransactions(BusDate initialDate, DateView dateView, Status status);
    }
}
