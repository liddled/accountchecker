using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.Framework.Common;
using DL.Framework.Web;

namespace DL.AccountChecker.Suite.Web
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            Log.InfoFormat("Starting web application");
            RegisterRoutes(RouteTable.Routes);
            RegisterModelBinders(ModelBinders.Binders);
            RegisterBundles(BundleTable.Bundles);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteHelper.RegisterUrlRoutesFromAttributes(routes);
        }

        public static void RegisterModelBinders(ModelBinderDictionary binders )
        {
            Log.InfoFormat("Registering BusDate ModelBinder");
            binders.Add(typeof(BusDate), new BusDateModelBinder());

            Log.InfoFormat("Registering DateView ModelBinder");
            binders.Add(typeof(DateView), new DateViewModelBinder());

            Log.InfoFormat("Registering Status ModelBinder");
            binders.Add(typeof(Status), new StatusModelBinder());

            Log.InfoFormat("Registering Account ModelBinder");
            binders.Add(typeof(Account), new AccountModelBinder());

            Log.InfoFormat("Registering Category ModelBinder");
            binders.Add(typeof(Category), new CategoryModelBinder());

            Log.InfoFormat("Registering Category List ModelBinder");
            binders.Add(typeof(IList<Category>), new CategoryListModelBinder());

            Log.InfoFormat("Registering Transaction ModelBinder");
            binders.Add(typeof(Transaction), new TransactionModelBinder());
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css").Include(
                 "~/content/site.css",
                 "~/content/ui.js"
            ));
            bundles.Add(new ScriptBundle("~/scripts").Include(
                "~/content/scripts/jquery-1.6.4.js",
                "~/content/scripts/jquery-ui-1.8.11.js",
                //"~/content/scripts/jquery.signalR.js",
                "~/content/scripts/functions.js"
            ));
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Response.Filter.Dispose();
            var ex = Server.GetLastError().GetBaseException();
            Log.Error(ex);
        }
    }
}