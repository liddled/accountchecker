using System;
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
    public class TransactionModelBinder : DomainObjectModelBinder<Transaction>
    {
        private const string Prefix = "transactionId";
        private const string UniqueIdPrefix = "uniqueId";

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            Transaction transaction = null;

            if (bindingContext.ValueProvider.ContainsPrefix(Prefix))
            {
                var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

                var transactionId = Convert.ToInt64(bindingContext.ValueProvider.GetValue(Prefix).AttemptedValue);
                transaction = proxy.GetTransaction(transactionId);
            }
            else if (bindingContext.ValueProvider.ContainsPrefix(UniqueIdPrefix))
            {
                var value = bindingContext.ValueProvider.GetValue(Prefix);
                if (value != null && !String.IsNullOrEmpty(value.AttemptedValue))
                {
                    var proxy = ServiceProxyManager.GetProxy<IGuiService>(EndPoints.GuiService);

                    var uniqueId = bindingContext.ValueProvider.GetValue(Prefix).AttemptedValue;
                    transaction = proxy.GetTransaction(uniqueId);
                }
            }

            return transaction ?? (transaction = new Transaction
            {
                Status = Status.Active
            });
        }
    }
}
