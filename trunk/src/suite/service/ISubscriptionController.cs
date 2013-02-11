using System;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Suite.Service
{
    public interface ISubscriptionController
    {
        void Subscribe(Action<CacheUpdate> subscriptionCallback, Topic topic);
        void UnSubscribe(Action<CacheUpdate> subscriptionCallback, Topic topic);

        void OnMessage(Topic topic, CacheUpdate cu);
    }
}
