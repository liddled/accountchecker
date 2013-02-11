using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;
using DL.Framework.Web;

namespace DL.AccountChecker.Suite.Web
{
    [HandleError]
    public class CategoriesController : BaseController
    {
        [UrlRoute(Path = "categories")]
		public ActionResult Index(Status status)
		{
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var accountCategoryList = proxy.GetAccountCategories(status);

            return View(accountCategoryList);
		}

        [UrlRoute(Path = "categories/all")]
        public ActionResult All(Status status)
        {
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var categoryList = proxy.GetCategories(status);

            var groupedCategoryList = categoryList.GroupBy(c => c.CategoryName[0].ToString().ToUpper(), c => c)
                .ToDictionary(item => item.Key, item => (IList<Category>)item.ToList());

            return View(groupedCategoryList);
        }

		[UrlRoute(Path = "categories/{accountId}")]
		[UrlRouteParameterConstraint(Name = "accountId", Regex = @"\d+")]
		public ActionResult Details(long accountId, Status status)
		{
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var account = proxy.GetAccount(accountId);
            var categoryList = proxy.GetCategories(accountId, status);

            var model = new CategoryDetailViewModel
            {
                Account = account,
                GroupedCategories = categoryList.GroupBy(c => c)
                    .OrderBy(g => g.Key.CategoryName)
                    .ToDictionary(item => item.Key, item => item.Count()),
                TotalCategories = categoryList.Count
            };

			return View(model);
		}

        [UrlRoute(Name = "categories_lookup", Path = "categories/lookup")]
		public ActionResult Lookup(string term)
		{
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var categoriesFiltered = proxy.GetCategories(term);

            var categories = from c in categoriesFiltered
							 select new { id = c.CategoryId, label = c.CategoryName, value = c.CategoryName };

			return Json(categories, JsonRequestBehavior.AllowGet);
		}

		[UrlRoute(Path = "categories/edit/{categoryId}")]
		[UrlRouteParameterConstraint(Name = "categoryId", Regex = @"\d+")]
		public ActionResult Edit(long categoryId)
		{
            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            var category = proxy.GetCategory(categoryId);

			if (category == null)
				return RedirectToAction("Index");

            return View(category);
		}

		[UrlRoute(Name = "category_edit", Path = "categories/edit/{categoryId}")]
		[UrlRouteParameterConstraint(Name = "categoryId", Regex = @"\d+")]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
		public ActionResult Edit(Category category)
		{
			if (!ModelState.IsValid)
				return View(category);

            var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);
            proxy.UpdateCategory(category);

			return RedirectToAction("Index");
		}
    }
}
