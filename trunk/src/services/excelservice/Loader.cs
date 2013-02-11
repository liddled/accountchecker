using System;
using Common.Logging;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.ExcelService
{
    public class Loader
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var excelController = ObjectFactoryManager.GetObject<IExcelController>(Settings.ExcelController);

            //var service = new Service(excelController);
            //StandardService.RunMain(args, service);
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject);
        }
    }
}
