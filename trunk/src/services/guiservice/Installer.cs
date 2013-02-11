using System.ComponentModel;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.GUIService
{
    [RunInstaller(true)]
    public class Installer : StandardServiceInstaller
    {
        public Installer()
            : base("AccountCheckerGUIService")
        {
        }
    }
}
