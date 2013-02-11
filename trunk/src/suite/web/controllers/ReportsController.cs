using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.AccountChecker.Model;
using DL.Framework.Common;
using DL.Framework.Services;
using DL.Framework.Web;

namespace DL.AccountChecker.Suite.Web
{
    [HandleError]
    public class ReportsController : BaseController
    {
        [UrlRoute(Path = "reports")]
        public ActionResult Index(Status status)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var accountTransactionList = proxy.GetAccountTransactions(BusDate.Today, DateView.All, status);

            var model = new ReportIndexViewModel();

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

                model.Views.Add(new ReportSummaryModel
                {
                    Account = item.Key,
                    Change = change,
                    ChangePercentage = changePercentage,
                    HighestBalance = highestBalance,
                    LowestBalance = lowestBalance
                });
            }

            return View(model);
        }

        [UrlRoute(Path = "reports/{accountId}")]
        [UrlRouteParameterConstraint(Name = "accountId", Regex = @"\d+")]
        public ActionResult Details(long accountId, int? from, int? to)
        {
            var fromDate = (from.HasValue) ? new BusDate(from.Value) : BusDate.GetDate(BusDate.Today, DateView.Month);
            var toDate = (to.HasValue) ? new BusDate(to.Value) : BusDate.GetNextDate(fromDate, DateView.Month);

            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

            var account = proxy.GetAccount(accountId);
            var transactionList = proxy.GetTransactions(accountId, fromDate, toDate, Status.Active)
                                    .OrderBy(t => t.Date)
                                    .ThenBy(t => t.TransactionId)
                                    .ToList();
            var startBalance = proxy.GetBalance(accountId, fromDate, Status.Active);

            decimal endBalance = 0;
            var highestBalanceDate = BusDate.Null;
            var lowestBalanceDate = BusDate.Null;
            decimal highestBalance = 0;
            decimal lowestBalance = 0;
            decimal change = 0;
            decimal changePercentage = 0;

            IDictionary<BusDate, Candlestick> dailyBalances = new SortedDictionary<BusDate, Candlestick>();

            if (transactionList.Count > 0)
            {
                endBalance = startBalance;
                highestBalanceDate = fromDate;
                lowestBalanceDate = fromDate;
                highestBalance = startBalance;
                lowestBalance = startBalance;

                foreach (var transaction in transactionList)
                {
                    if (!dailyBalances.ContainsKey(transaction.Date))
                        dailyBalances.Add(transaction.Date, new Candlestick { Open = endBalance, Close = endBalance, Low = endBalance, High = endBalance });

                    var candlestick = dailyBalances[transaction.Date];
                    var value = candlestick.Close + transaction.Amount;

                    if (value > candlestick.High)
                        candlestick.High = value;
                    if (value < candlestick.Low)
                        candlestick.Low = value;

                    candlestick.Close = value;

                    if (candlestick.Low < lowestBalance)
                    {
                        lowestBalanceDate = transaction.Date;
                        lowestBalance = candlestick.Low;
                    }

                    if (candlestick.High > highestBalance)
                    {
                        highestBalanceDate = transaction.Date;
                        highestBalance = candlestick.High;
                    }

                    endBalance += transaction.Amount;
                }

                change = Math.Abs(endBalance) - Math.Abs(startBalance);
                changePercentage = (startBalance != 0) ? change / startBalance : 0;
            }

            var model = new ReportDetailViewModel
            {
                Account = account,
                Balances = dailyBalances,
                FromDate = fromDate,
                ToDate = toDate,
                StartBalance = startBalance,
                EndBalance = endBalance,
                Change = change,
                ChangePercentage = changePercentage,
                HighestBalanceDate = highestBalanceDate,
                HighestBalance = highestBalance,
                LowestBalanceDate = lowestBalanceDate,
                LowestBalance = lowestBalance
            };

			return View(model);
        }
    }
}