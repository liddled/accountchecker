using System;
using System.Web.Mvc;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;
using DL.Framework.Web;

namespace DL.AccountChecker.Suite.Web
{
    public class AccountModelBinder : DomainObjectModelBinder<Account>
    {
        private const string Prefix = "accountId";

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            Account account = null;

            if (bindingContext.ValueProvider.ContainsPrefix(Prefix))
            {
                var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

                var accountId = Convert.ToInt64(bindingContext.ValueProvider.GetValue(Prefix).AttemptedValue);
                account = proxy.GetAccount(accountId);
            }
            else
            {
                account = new Account
                {
                    Status = Status.Active
                };
            }

            return account;
        }
    }
}
