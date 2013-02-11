using System.ServiceModel;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    [ServiceContract]
    [ServiceKnownType(typeof(IExcelNode))]
    [ServiceKnownType(typeof(ExcelNamespace))]
    [ServiceKnownType(typeof(ExcelNode))]
    public interface IExcelService
    {
        [OperationContract]
        ExcelNamespace GetNodes();
    }
}
