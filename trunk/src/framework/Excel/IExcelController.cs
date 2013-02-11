using System.ServiceModel;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    [ServiceContract]
    public interface IExcelController
    {
        ExcelNamespace GetNodes();
    }
}
