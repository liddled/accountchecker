using System.Collections.Generic;
using System.ServiceModel;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    [ServiceContract]
    [ServiceKnownType(typeof(BusDate))]
    [ServiceKnownType(typeof(Status))]
    [ServiceKnownType(typeof(Account))]
    [ServiceKnownType(typeof(Category))]
    [ServiceKnownType(typeof(Transaction))]
    public interface ISyncService
    {
        
    }
}
