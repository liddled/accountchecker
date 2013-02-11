using System;
using System.Collections.Generic;
using Common.Logging;
using DL.AccountChecker.Domain;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.WorkflowService
{
    public class Loader
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var workflowList = ObjectFactoryManager.GetObject<IList<IWorkflow>>("workflowList");

            var workflowService = new WorkflowService(workflowList);
            StandardService.RunMain(args, workflowService);
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.ExceptionObject);
        }
    }
}
