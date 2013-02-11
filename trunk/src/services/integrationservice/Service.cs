using System.Collections.Generic;
using Common.Logging;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.IntegrationService
{
    public class Service : StandardService
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IList<IWorkflow> _workflowList;

        public Service(IList<IWorkflow> workflowList)
        {
            _workflowList = workflowList;
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Starting workflow(s)...");
            foreach (var workflow in _workflowList)
                workflow.Start();
        }

        protected override void OnStop()
        {
            Log.Info("Stopping workflow(s)...");
            foreach (var workflow in _workflowList)
                workflow.Stop();
        }
    }
}
