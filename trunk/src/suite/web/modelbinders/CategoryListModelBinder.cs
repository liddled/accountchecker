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
    public class CategoryListModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IList<Category> categoryList = new List<Category>();

            var prefix = bindingContext.ModelName;

            if (bindingContext.ValueProvider.ContainsPrefix(prefix))
            {
                var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

                var categoryNameList = bindingContext.ValueProvider.GetValue(prefix).AttemptedValue;
                var categoryNameListSplit = categoryNameList.SplitToList(true);

                categoryList.AddRange(proxy.GetCategories(categoryNameListSplit));

                var newCategoryList = categoryNameListSplit.Except(categoryList.Select(c => c.CategoryName));

                foreach (var categoryName in newCategoryList)
                {
                    var category = new Category
                    {
                        CategoryName = categoryName,
                        Status = Status.Active,
                    };
                    categoryList.Add(category);
                }
            }

            return categoryList;
        }
    }
}
