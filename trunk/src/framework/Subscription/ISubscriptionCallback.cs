using System.ServiceModel;
using DL.AccountChecker.Domain;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    [ServiceKnownType(typeof(Account))]
    [ServiceKnownType(typeof(Category))]
    [ServiceKnownType(typeof(Transaction))]
    public interface ISubscriptionCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnMessage(Topic topic, CacheUpdate msg);
    }
}
