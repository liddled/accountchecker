using Common.Logging;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.BusinessLogic
{
    public class ExcelController : IExcelController
    {
        private readonly ExcelNamespace _excelNamespace;

        public ExcelController(ExcelNamespace excelNamespace)
        {
            _excelNamespace = excelNamespace;
        }

        public ExcelNamespace GetNodes()
        {
            return _excelNamespace;
        }
    }
}
