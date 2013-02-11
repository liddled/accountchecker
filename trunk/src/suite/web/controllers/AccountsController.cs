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
    public class AccountsController : BaseController
    {
        [UrlRoute(Path = "accounts", Order = 1), UrlRoute(Name = "accounts_index", Path = "", Order = 2)]
        public ActionResult Index(Status status)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var accountTransactionList = proxy.GetAccountTransactions(BusDate.Today, DateView.All, status);

            var model = new AccountIndexViewModel();

            foreach (var item in accountTransactionList)
            {
                var outcome = item.Value.Where(t => t.Amount < 0).Sum(t => t.Amount);
                var income = item.Value.Where(t => t.Amount > 0).Sum(t => t.Amount);

                model.Views.Add(new AccountSummaryModel
                {
                    Account = item.Key,
                    TransactionCount = item.Value.Count,
                    Outcome = outcome,
                    Income = income
                });
            }

            return View(model);
        }

        [UrlRoute(Path = "accounts/add")]
        public ActionResult Add()
        {
            return View();
        }

        [UrlRoute(Name = "account_add", Path = "accounts/add")]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Add(Account account)
        {
			if (!ModelState.IsValid)
				return View();

            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            proxy.InsertAccount(account);

            return RedirectToAction("Index");
        }

        [UrlRoute(Path = "accounts/edit/{accountId}"),
		UrlRouteParameterConstraint(Name = "accountId", Regex = @"\d+")]
        public ActionResult Edit(long accountId)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var account = proxy.GetAccount(accountId);

			if (account == null)
				return RedirectToAction("index", "accounts");

			return View(account);
        }

        [UrlRoute(Name = "account_edit", Path = "accounts/edit/{accountId}"),
		UrlRouteParameterConstraint(Name = "accountId", Regex = @"\d+")]
        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Edit(Account account)
        {
			if (!ModelState.IsValid)
				return View(account);

            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            proxy.UpdateAccount(account);

            return RedirectToAction("Index");
        }
    }
}
