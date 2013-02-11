using System;
using System.Web.Mvc;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;
using DL.Framework.Web;

namespace DL.AccountChecker.Suite.Web
{
    public class CategoryModelBinder : DomainObjectModelBinder<Category>
    {
        private const string Prefix = "categoryId";

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            Category category = null;

            if (bindingContext.ValueProvider.ContainsPrefix(Prefix))
            {
                var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

                var categoryId = Convert.ToInt64(bindingContext.ValueProvider.GetValue(Prefix).AttemptedValue);
                category = proxy.GetCategory(categoryId);
            }
            else
            {
                category = new Category
                {
                    Status = Status.Active
                };
            }

            return category;
        }
    }
}
