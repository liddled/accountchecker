using System;
using System.ServiceModel;
using DL.Framework.Common;

namespace DL.AccountChecker.Framework
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISubscriptionCallback))]
    public interface ISubscriptionService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe(Topic topic);

        [OperationContract(IsOneWay = true)]
        void UnSubscribe(Topic topic);
    }
}
