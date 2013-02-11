using System;
using System.Linq;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.BusinessLogic
{
    public class CacheSync : ICacheSync
    {
        private readonly IMessageSubscriber _messageSubscriber;

        public CacheSync(IMessageSubscriber messageSubscriber)
        {
            _messageSubscriber = messageSubscriber;
        }

        public void Start()
        {
            _messageSubscriber.Subscribe();
        }

        public void Stop()
        {
            _messageSubscriber.Unsubscribe();
        }
    }
}
