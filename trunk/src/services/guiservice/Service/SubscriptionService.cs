using System;
using System.Collections.Generic;
using System.ServiceModel;
using Common.Logging;
using DL.AccountChecker.Common;
using DL.AccountChecker.Domain;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Services.GuiService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class SubscriptionService : ISubscriptionService
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICachedTypes _cachedTypes;
        private readonly IDictionary<Topic, FilteredSubscriptionEventHandler> _subscriptionHandlers;

        private readonly object _lock = new object();

        public SubscriptionService(ICachedTypes cachedTypes)
        {
            _cachedTypes = cachedTypes;
            _subscriptionHandlers = new Dictionary<Topic, FilteredSubscriptionEventHandler>();
        }

        private static ISubscriptionCallback GetCallbackChannel()
        {
            return OperationContext.Current.GetCallbackChannel<ISubscriptionCallback>();
        }

        protected Action<Topic, bool> GetSubscribeAction(Topic topic)
        {
            switch (topic)
            {
                case Topic.Accounts:
                    return Subscribe<Account>;
                case Topic.Categories:
                    return Subscribe<Category>;
                case Topic.Transactions:
                    return Subscribe<Transaction>;
                default:
                    return (x, a) => { };
            }
        }

        public void Subscribe(Topic topic)
        {
            var action = GetSubscribeAction(topic);
            action(topic, true);
        }

        protected void Subscribe<T>(Topic topic, bool isAtomic) where T : class, IItemKey
        {
            Action<Action<CacheUpdate>> removal = _cachedTypes.GetCache<T>().RemoveEventHandler;

            var callback = GetCallbackChannel();
            var eh = new FilteredSubscriptionEventHandler(callback, removal, topic);

            lock (_lock)
            {
                if (_subscriptionHandlers.Count == 0)
                {
                    Log.InfoFormat("Client {0} subscribed to service", "[[ID]]");
                }
                _subscriptionHandlers[topic] = eh;
            }

            _cachedTypes.GetCache<T>().AddEventHandler(eh);
        }

        public void UnSubscribe(Topic topic)
        {
            lock (_lock)
            {
                FilteredSubscriptionEventHandler eh;
                if (_subscriptionHandlers.TryGetValue(topic, out eh))
                {
                    eh.RemoveHandler();
                    _subscriptionHandlers.Remove(topic);
                }
            }
        }
    }
}
