using System.ComponentModel;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.WorkflowService
{
    [RunInstaller(true)]
    public class Installer : StandardServiceInstaller
    {
        public Installer()
            : base("AccountCheckerWorkflowService")
        {
        }
    }
}
