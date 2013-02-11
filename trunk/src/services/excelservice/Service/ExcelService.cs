using System.ServiceModel;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Services.ExcelService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ExcelService : IExcelService
    {
        private readonly IExcelController _excelController;

        public ExcelService(IExcelController excelController)
        {
            _excelController = excelController;
        }

        public ExcelNamespace GetNodes()
        {
            return _excelController.GetNodes();
        }
    }
}
