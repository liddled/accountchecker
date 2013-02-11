using System;
using System.Windows;
using Common.Logging;

namespace DL.AccountChecker.Suite.WPF
{
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject);
        }
    }
}
