using System;
using System.Collections.Generic;
using Common.Logging;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.GuiService
{
    internal class Loader
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var cachedTypes = ObjectFactoryManager.GetObject<ICachedTypes>("cachedTypes");
            var cacheLoaders = ObjectFactoryManager.GetObject<IList<ICacheLoader>>("cacheLoaders");
            var accountManagerFactory = ObjectFactoryManager.GetObject<IAccountManagerFactory>("accountManagerFactory");
            var categoryManagerFactory = ObjectFactoryManager.GetObject<ICategoryManagerFactory>("categoryManagerFactory");
            var transactionManagerFactory = ObjectFactoryManager.GetObject<ITransactionManagerFactory>("transactionManagerFactory");
            var cacheSync = ObjectFactoryManager.GetObject<ICacheSync>("cacheSync");

            var service = new Service(cachedTypes, cacheLoaders, accountManagerFactory, categoryManagerFactory, transactionManagerFactory, cacheSync);
            StandardService.RunMain(args, service);
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.ExceptionObject);
        }
    }
}
