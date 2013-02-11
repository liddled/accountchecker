using System;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Common
{
    public class FilteredSubscriptionEventHandler : FilteredEventHandler
    {
        private readonly ISubscriptionCallback _callback;

        public FilteredSubscriptionEventHandler(ISubscriptionCallback callback, Action<Action<CacheUpdate>> removal, Topic topic)
            : base(removal)
        {
            _callback = callback;

            Event = m => _callback.OnMessage(topic, m);
        }
    }
}