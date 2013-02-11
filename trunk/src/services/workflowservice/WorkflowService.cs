using System.Collections.Generic;
using Common.Logging;
using DL.AccountChecker.Framework;
using DL.AccountChecker.Servers;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.WorkflowService
{
    public class WorkflowService : StandardService
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IWorkflowServer _workflowServer;

        private DependencyInjectionServiceHost _workflowServerHost;

        public WorkflowService(IList<IWorkflow> workflowList)
        {
            _workflowServer = new WorkflowServer(workflowList);
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("Starting...");
            _workflowServerHost = new DependencyInjectionServiceHost(typeof(WorkflowServer), () => _workflowServer);
            _workflowServerHost.Open();
        }

        protected override void OnStop()
        {
            Log.Info("Stopping...");
            if (_workflowServerHost != null)
                _workflowServerHost.Close();
        }
    }
}
