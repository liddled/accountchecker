using Common.Logging;
using DL.AccountChecker.Framework;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.ExcelService
{
    /*public class Service : StandardService
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IExcelService _excelService;

        private DependencyInjectionServiceHost _excelServiceHost;

        public Service(IExcelController excelController)
        {
            _excelService = new ExcelService(excelController);
        }

        protected override void OnStart(string[] args)
        {
            Log.Info("ExcelService starting...");
            _excelServiceHost = new DependencyInjectionServiceHost(typeof(ExcelService), () => _excelService);
            _excelServiceHost.Open();
        }

        protected override void OnStop()
        {
            Log.Info("ExcelService stopping...");
            if (_excelServiceHost != null)
                _excelServiceHost.Close();
        }
    }*/
}
