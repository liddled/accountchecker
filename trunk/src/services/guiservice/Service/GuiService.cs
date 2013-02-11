using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Services.GuiService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class GuiService : IGuiService
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private IAccountManagerFactory _accountManagerFactory;
        private ICategoryManagerFactory _categoryManagerFactory;
        private ITransactionManagerFactory _transactionManagerFactory;

        public GuiService(IAccountManagerFactory accountManagerFactory, ICategoryManagerFactory categoryManagerFactory, ITransactionManagerFactory transactionManagerFactory)
        {
            _accountManagerFactory = accountManagerFactory;
            _categoryManagerFactory = categoryManagerFactory;
            _transactionManagerFactory = transactionManagerFactory;
        }

        #region Accounts

        public void InsertAccount(Account account)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                _accountManagerFactory.Insert(account);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("InsertAccount failed");
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("InsertAccount time - {0}", timer.Elapsed);
            }
        }

        public void UpdateAccount(Account account)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                _accountManagerFactory.Update(account);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("UpdateAccount failed.");
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("UpdateAccount time - {0}", timer.Elapsed);
            }
        }

        public IList<Account> GetAccounts(Status status)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _accountManagerFactory.GetAccounts(status);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetAccounts failed - Status {0}", status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetAccounts time - Status {0} - {1}", status, timer.Elapsed);
            }
        }

        public Account GetAccount(long accountId)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _accountManagerFactory.GetAccount(accountId);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetAccount failed - AccountId {0}", accountId);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("{2} - GetAccount time - AccountId {0} - {1}", accountId, timer.Elapsed, OperationContext.Current.SessionId);
            }
        }

        public Account GetAccount(string id)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _accountManagerFactory.GetAccount(id);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetAccount failed - id {0}", id);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetAccount time - id {0} - {1}", id, timer.Elapsed);
            }
        }

        #endregion

        #region Budgets

        public decimal GetBalance(long accountId, BusDate endDate, Status status)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                var transactions = _transactionManagerFactory.GetTransactions(accountId, BusDate.Null, endDate, status);
                return transactions.Sum(t => t.Amount);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetBalance failed - Account {0}, EndDate {1}, Status {2}", accountId, endDate, status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetBalance time - Account {0}, EndDate {1}, Status {2} - {3}", accountId, endDate, status, timer.Elapsed);
            }
        }

        #endregion
        
        #region Categories

        public void UpdateCategory(Category category)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                _categoryManagerFactory.Update(category);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("UpdateCategory failed.");
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("UpdateCategory time - {0}", timer.Elapsed);
            }
        }

        public Category GetCategory(long categoryId)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _categoryManagerFactory.GetCategory(categoryId);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetCategory failed - CategoryId {0}", categoryId);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetCategory time - CategoryId {0} - {1}", categoryId, timer.Elapsed);
            }
        }

        public IList<Category> GetCategories(Status status)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _categoryManagerFactory.GetCategories(status).OrderBy(c => c.CategoryName).ToList();
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetCategories failed");
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetCategories time - {0}", timer.Elapsed);
            }
        }

        public IList<Category> GetCategories(long accountId, Status status)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                var transactionList = _transactionManagerFactory.GetTransactions(accountId);
                return transactionList.SelectMany(t => t.Categories).ToList();
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetCategories failed - AccountId {0}, Status {1}", accountId, status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetCategories time - AccountId {0}, Status {1} - {2}", accountId, status, timer.Elapsed);
            }
        }

        public IList<Category> GetCategories(IList<string> categoryNames)
        {
            var timer = Stopwatch.StartNew();
            var status = Status.Active;

            try
            {
                return _categoryManagerFactory.GetCategories(categoryNames, status);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetCategories failed");
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetCategories time - {0}", timer.Elapsed);
            }
        }

        public IList<Category> GetCategories(string searchText)
        {
            var timer = Stopwatch.StartNew();
            var status = Status.Active;

            try
            {
                return _categoryManagerFactory.SearchCategories(searchText, status);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetCategories failed - SearchText {0}, Status {1}", searchText, status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetCategories time - SearchText {0}, Status {1} - {2}", searchText, status, timer.Elapsed);
            }
        }

        public IDictionary<Account, IList<Category>> GetAccountCategories(Status status)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                var accountCategoryList = new SortedDictionary<Account, IList<Category>>();

                var accountList = _accountManagerFactory.GetAccounts(status);

                foreach (var account in accountList)
                {
                    var transactionList = _transactionManagerFactory.GetTransactions(account.AccountId);

                    var categoryIdList = transactionList.SelectMany(t => t.Categories).Select(c => c.CategoryId).Distinct().ToList();
                    var categoryList = _categoryManagerFactory.GetCategories(categoryIdList, Status.Active).OrderBy(c => c.CategoryName).ToList();

                    accountCategoryList.Add(account, categoryList);
                }

                return accountCategoryList;
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetAccountCategories failed - Status {0}", status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetAccountCategories time - Status {0} - {1}", status, timer.Elapsed);
            }
        }

        #endregion

        #region Transactions

        public void InsertTransaction(Transaction transaction)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                _transactionManagerFactory.Insert(transaction);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("InsertTransaction failed");
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("InsertTransaction time - {0}", timer.Elapsed);
            }
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                _transactionManagerFactory.Update(transaction);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("UpdateTransaction failed");
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("UpdateTransaction time - {0}", timer.Elapsed);
            }
        }

        public void ExcludeTransaction(long transactionId, bool exclude)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                _transactionManagerFactory.Exclude(transactionId, exclude);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("ExcludeTransaction failed - TransactionId {0}, Exclude {1}", transactionId, exclude);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("ExcludeTransaction time - TransactionId {0}, Exclude {1} - {2}", transactionId, exclude, timer.Elapsed);
            }
        }

        public Transaction GetTransaction(long transactionId)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _transactionManagerFactory.GetTransaction(transactionId);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetTransaction failed - TransactionId {0}", transactionId);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetTransaction time - TransactionId {0} - {1}", transactionId, timer.Elapsed);
            }
        }

        public Transaction GetTransaction(string uniqueId)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _transactionManagerFactory.GetTransaction(uniqueId);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetTransaction failed - UniqueId {0}", uniqueId);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetTransaction time - UniqueId {0} - {1}", uniqueId, timer.Elapsed);
            }
        }

        public IList<Transaction> GetTransactions(long accountId, BusDate startDate, BusDate endDate, Status status)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                return _transactionManagerFactory.GetTransactions(accountId, startDate, endDate, status);
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetTransactions failed - AccountId {0}, StartDate {1}, EndDate {2}, Status {3}", accountId, startDate, endDate, status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetTransactions time - AccountId {0}, StartDate {1}, EndDate {2}, Status {3} - {4}", accountId, startDate, endDate, status, timer.Elapsed);
            }
        }

        public IList<Transaction> GetTransactions(long accountId, BusDate initialDate, DateView dateView, Status status, IList<string> categoryNames)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                var categoryList = _categoryManagerFactory.GetCategories(categoryNames, status);
                var transactionList = _transactionManagerFactory.GetTransactions(accountId, initialDate, dateView, status, categoryList);

                return transactionList;
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetTransactions failed - AccountId {0}, Date {1}, DateView {2}, Status {3}", accountId, initialDate, dateView, status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("GetTransactions time - AccountId {0}, Date {1}, DateView {2}, Status {3} - {4}", accountId, initialDate, dateView, status, timer.Elapsed);
            }
        }

        public IDictionary<Account, IList<Transaction>> GetAccountTransactions(BusDate initialDate, DateView dateView, Status status)
        {
            var timer = Stopwatch.StartNew();

            try
            {
                var accountTransactionList = new SortedDictionary<Account, IList<Transaction>>();

                var accountList = _accountManagerFactory.GetAccounts(status);

                foreach (var account in accountList)
                {
                    var transactionList = _transactionManagerFactory.GetTransactions(account.AccountId, initialDate, dateView, Status.Active);
                    accountTransactionList.Add(account, transactionList);
                }

                return accountTransactionList;
            }
            catch (Exception ex)
            {
                var exceptionString = String.Format("GetAccountTransactions failed - Date {0}, DateView {1}, Status {2}", initialDate, dateView, status);
                Log.Error(exceptionString, ex);
                throw new ApplicationException(exceptionString, ex);
            }
            finally
            {
                timer.Stop();
                Log.DebugFormat("{4} - GetAccountTransactions time - Date {0}, DateView {1}, Status {2} - {3}", initialDate, dateView, status, timer.Elapsed, OperationContext.Current.SessionId);
            }
        }

        #endregion
    }
}
