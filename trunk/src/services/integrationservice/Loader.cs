using System;
using System.Collections.Generic;
using Common.Logging;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.IntegrationService
{
    internal class Loader
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var workflowList = ObjectFactoryManager.GetObject<IList<IWorkflow>>("workflowList");

            var service = new Service(workflowList);
            StandardService.RunMain(args, service);
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject);
        }
    }
}
