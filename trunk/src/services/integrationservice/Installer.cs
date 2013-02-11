using System.ComponentModel;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.IntegrationService
{
    [RunInstaller(true)]
    public class Installer : StandardServiceInstaller
    {
        public Installer()
            : base("AccountCheckerIntegrationService")
        {
        }
    }
}
