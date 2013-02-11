using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.AccountChecker.Model;
using DL.Framework.Common;
using DL.Framework.Services;
using DL.Framework.Web;

namespace DL.AccountChecker.Suite.Web
{
    [HandleError]
    public class TransactionsController : BaseController
    {
        private const string TRANSACTION_HISTORY_ROUTE_VALUES = "TransactionHistoryRouteValues";

		public RouteValueDictionary TransactionHistoryRouteValues
		{
			get
			{
                if (Session[TRANSACTION_HISTORY_ROUTE_VALUES] == null)
                    Session[TRANSACTION_HISTORY_ROUTE_VALUES] = new RouteValueDictionary();

                return (RouteValueDictionary)Session[TRANSACTION_HISTORY_ROUTE_VALUES];
			}
			set
			{
                Session[TRANSACTION_HISTORY_ROUTE_VALUES] = value;
			}
		}

        private RouteValueDictionary GetRedirectToHistoryRouteData(Transaction transaction)
        {
            RouteValueDictionary routes = TransactionHistoryRouteValues;
            routes["controller"] = RouteData.Values["controller"];
            routes["action"] = "History";
            routes["accountId"] = transaction.AccountId;
            //routes["transactionId"] = transaction.TransactionId;
            //routes["date"] = transaction.Date.Sort8Date;
            routes.Remove("status");
            routes.Remove("categories");

            return routes;
            //return Redirect(string.Format("{0}/?t={1}#{1}", url, t.TransactionId));
        }

        [UrlRoute(Path = "transactions")]
        public ActionResult Index(Status status)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var accountTransactionList = proxy.GetAccountTransactions(BusDate.Today, DateView.All, status);

            var model = new TransactionIndexViewModel();

            foreach (var item in accountTransactionList)
            {
                var monthlyTransactionList = item.Value.Where(TransactionFilter.WithDateBetween(item.Key.AccountId, BusDate.Today, DateView.Month, Status.Active));

                var startBalance = item.Value.Where(t => t.Date.Sort8Date < BusDate.Today.MonthSort8Date).Sum(t => t.Amount);
                var currentBalance = item.Value.Sum(t => t.Amount);

                var outcome = monthlyTransactionList.Where(t => t.Amount < 0).Sum(t => t.Amount);
                var income = monthlyTransactionList.Where(t => t.Amount > 0).Sum(t => t.Amount);

                model.Views.Add(new TransactionSummaryModel
                {
                    Account = item.Key,
                    StartBalance = startBalance,
                    CurrentBalance = currentBalance,
                    Outcome = outcome,
                    Income = income,
                    TransactionCount = monthlyTransactionList.Count()
                });
            }

            return View(model);
        }

        [UrlRoute(Path = "transactions/{accountId}")]
        [UrlRouteParameterConstraint(Name = "accountId", Regex = @"\d+")]
        public ActionResult History(long accountId, int? date, DateView view, string categories, Status status)
        {
            var thisDate = (date.HasValue) ? new BusDate(date.Value) : new BusDate(BusDate.Today.MonthSort8Date);
            var categoryNameList = categories.SplitToList(true);

            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

            var account = proxy.GetAccount(accountId);
            var categoryList = proxy.GetCategories(categoryNameList);
            var transactionList = proxy.GetTransactions(accountId, thisDate, view, status, categoryNameList);

            var outcome = transactionList.Where(t => !t.Info.Excluded && t.Amount < 0).Sum(t => t.Amount);
            var income = transactionList.Where(t => !t.Info.Excluded && t.Amount > 0).Sum(t => t.Amount);

            TransactionHistoryRouteValues = DL.Framework.Web.ViewExtensions.GetPageRouteValues(RouteData.Values, Request.QueryString);

            var model = new TransactionHistoryModel
            {
                Account = account,
                Date = BusDate.GetDate(thisDate, view),
                DateView = view,
                Categories = categoryList,
                TransactionCount = transactionList.Count,
                Outcome = outcome,
                Income = income,
                Transactions = transactionList
            };

            return View(model);
        }

        [UrlRoute(Path = "transactions/add/{accountId}"),
        UrlRouteParameterDefault(Name = "accountId", Value = "")]
        public ActionResult Add(long? accountId)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            
            ViewBag.Accounts = proxy.GetAccounts(Status.Active);

            return View();
        }

        [UrlRoute(Name = "transaction_add", Path = "transactions/add/{accountId}")]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Add(Transaction transaction)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

			if (!ModelState.IsValid)
			{
                ViewBag.Accounts = proxy.GetAccounts(Status.Active);
				return View(transaction);
			}

            proxy.InsertTransaction(transaction);

            var routes = GetRedirectToHistoryRouteData(transaction);
			return RedirectToRoute(routes);
        }

		[UrlRoute(Path = "transactions/edit/{transactionId}")]
		[UrlRouteParameterConstraint(Name = "transactionId", Regex = @"\d+")]
		public ActionResult Edit(long transactionId)
		{
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

            ViewBag.Accounts = proxy.GetAccounts(Status.Active);
            var transaction = proxy.GetTransaction(transactionId);

			if (transaction == null)
				return RedirectToAction("Index");

			return View(transaction);
		}

		[UrlRoute(Name = "transaction_edit", Path = "transactions/edit/{transactionId}")]
		[UrlRouteParameterConstraint(Name = "transactionId", Regex = @"\d+")]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Edit(Transaction transaction)
		{
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

			if (!ModelState.IsValid)
			{
                ViewBag.Accounts = proxy.GetAccounts(Status.Active);

				return View(transaction);
			}

            proxy.UpdateTransaction(transaction);

            var routes = GetRedirectToHistoryRouteData(transaction);
            return RedirectToRoute(routes);
		}

        [UrlRoute(Path = "transactions/exclude/{transactionId}/{exclude}")]
        [UrlRouteParameterConstraint(Name = "transactionId", Regex = @"\d+")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Exclude(long transactionId, bool exclude)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

            proxy.ExcludeTransaction(transactionId, exclude);
            
            return RedirectToRoute(TransactionHistoryRouteValues);
        }
	}
}
