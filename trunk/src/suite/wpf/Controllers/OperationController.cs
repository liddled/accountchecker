using System;
using System.Collections.Generic;
using System.Linq;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.AccountChecker.Model;
using DL.AccountChecker.Suite.Service;
using DL.Framework.Common;

namespace DL.AccountChecker.Suite.WPF
{
    public class OperationController
    {
        private readonly ServiceProxyController _subscriptionManager;

        public OperationController(ServiceProxyController subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
        }

        private IGuiService GuiService
        {
            get { return _subscriptionManager.GuiService; }
        }

        public Account GetAccount(long accountId)
        {
            return GuiService.GetAccount(accountId);
        }

        public IList<AccountSummaryModel> GetAccountSummaries(BusDate date, DateView dateView, Status status)
        {
            var accountSummaryList = new List<AccountSummaryModel>();

            var accountTransactionList = GetAccountTransactions(date, dateView, status);
            foreach (var item in accountTransactionList)
            {
                var outcome = item.Value.Where(t => t.Amount < 0).Sum(t => t.Amount);
                var income = item.Value.Where(t => t.Amount > 0).Sum(t => t.Amount);

                accountSummaryList.Add(new AccountSummaryModel
                {
                    Account = item.Key,
                    TransactionCount = item.Value.Count,
                    Outcome = outcome,
                    Income = income
                });
            }

            return accountSummaryList;
        }

        private IDictionary<Account, IList<Transaction>> GetAccountTransactions(BusDate date, DateView dateView, Status status)
        {
            return GuiService.GetAccountTransactions(date, dateView, status);
        }

        public void InsertAccount(Account account)
        {
            GuiService.InsertAccount(account);
        }

        public void UpdateAccount(Account account)
        {
            GuiService.UpdateAccount(account);
        }

        public IList<CategorySummaryModel> GetCategorySummaries(Status status)
        {
            var categorySummaryList = new List<CategorySummaryModel>();

            var accountCategoryList = GuiService.GetAccountCategories(status);
            foreach (var acl in accountCategoryList)
            {
                categorySummaryList.Add(new CategorySummaryModel
                {
                    Account = acl.Key,
                    Categories = acl.Value
                });
            }

            return categorySummaryList;
        }

        public IList<Category> GetCategories(IList<string> categoryNames)
        {
            return GuiService.GetCategories(categoryNames);
        }

        public IList<ReportSummaryModel> GetReportSummaries(BusDate date, DateView dateView, Status status)
        {
            var reportSummaryList = new List<ReportSummaryModel>();

            var accountTransactionList = GetAccountTransactions(date, dateView, status);
            foreach (var item in accountTransactionList)
            {
                var monthlyTransactionList = item.Value.Where(TransactionFilter.WithDateBetween(item.Key.AccountId, BusDate.Today, DateView.Month, Status.Active));

                var balance = item.Value.Where(t => t.Date.Sort8Date < BusDate.Today.MonthSort8Date).Sum(t => t.Amount);

                var dailyBalances = monthlyTransactionList.OrderBy(t => t.Date)
                    .GroupBy(t => t.Date, t => t)
                    .Select(g => new
                    {
                        Date = g.Key,
                        Balance = balance += g.Sum(t => t.Amount)
                    })
                    .ToDictionary(g => g.Date, g => g.Balance);

                decimal highestBalance = 0;
                decimal lowestBalance = 0;

                if (dailyBalances.Values.Count > 0)
                {
                    highestBalance = dailyBalances.Values.Max();
                    lowestBalance = dailyBalances.Values.Min();
                }

                decimal change = Math.Abs(highestBalance) - Math.Abs(lowestBalance);
                decimal changePercentage = (lowestBalance != 0) ? change / lowestBalance : 0;

                reportSummaryList.Add(new ReportSummaryModel
                {
                    Account = item.Key,
                    Change = change,
                    ChangePercentage = changePercentage,
                    HighestBalance = highestBalance,
                    LowestBalance = lowestBalance
                });
            }

            return reportSummaryList;
        }

        public IList<TransactionSummaryModel> GetTransactionSummaries(BusDate date, DateView dateView, Status status)
        {
            var transactionSummaryList = new List<TransactionSummaryModel>();

            var accountTransactionList = GetAccountTransactions(date, dateView, status);

            foreach (var item in accountTransactionList)
            {
                var monthlyTransactionList = item.Value.Where(TransactionFilter.WithDateBetween(item.Key.AccountId, BusDate.Today, DateView.Month, Status.Active));

                var startBalance = item.Value.Where(t => t.Date.Sort8Date < BusDate.Today.MonthSort8Date).Sum(t => t.Amount);
                var currentBalance = item.Value.Sum(t => t.Amount);

                var outcome = monthlyTransactionList.Where(t => t.Amount < 0).Sum(t => t.Amount);
                var income = monthlyTransactionList.Where(t => t.Amount > 0).Sum(t => t.Amount);

                transactionSummaryList.Add(new TransactionSummaryModel
                {
                    Account = item.Key,
                    StartBalance = startBalance,
                    CurrentBalance = currentBalance,
                    Outcome = outcome,
                    Income = income,
                    TransactionCount = monthlyTransactionList.Count()
                });
            }

            return transactionSummaryList;
        }

        public IList<Transaction> GetTransactions(long accountId, BusDate date, DateView dateView, Status status, IList<string> categoryNames)
        {
            return GuiService.GetTransactions(accountId, date, dateView, status, categoryNames);
        }

        public Transaction GetTransaction(long transactionId)
        {
            return GuiService.GetTransaction(transactionId);
        }

        public void InsertTransaction(Transaction transaction)
        {
            GuiService.InsertTransaction(transaction);
        }

        public void UpdateTransaction(Transaction transaction)
        {
            GuiService.UpdateTransaction(transaction);
        }

        public void ExcludeTransaction(long transactionId, bool exclude)
        {
            GuiService.ExcludeTransaction(transactionId, exclude);
        }

        public void Subscribe(Action<CacheUpdate> callback, Topic topic)
        {
            _subscriptionManager.Subscribe(callback, topic);
        }

        public void UnSubscribe(Action<CacheUpdate> callback, Topic topic)
        {
            _subscriptionManager.UnSubscribe(callback, topic);
        }
    }
}
