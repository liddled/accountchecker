using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Common
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class DuplexServiceProxy : ServiceProxy<ISubscriptionService>, ISubscriptionCallback
    {
        public event Action<Topic, CacheUpdate> MessageReceived;

        public DuplexServiceProxy(string endPoint)
            : base(endPoint)
        {
        }

        protected override ChannelFactory<ISubscriptionService> CreateChannelFactory()
        {
            return new DuplexChannelFactory<ISubscriptionService>(this, EndPoint);
        }

        public void OnMessage(Topic topic, CacheUpdate msg)
        {
            Action<Topic, CacheUpdate> handler = MessageReceived;
            if (handler != null) handler(topic, msg);
        }
    }
}
