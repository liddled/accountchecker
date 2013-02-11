using System;
using System.Collections.Generic;
using Common.Logging;
using DL.AccountChecker.Common;
using DL.AccountChecker.Framework;
using DL.Framework.Common;

namespace DL.AccountChecker.Suite.Service
{
    public class SubscriptionController : ISubscriptionController, IDisposable
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDictionary<string, DuplexServiceProxy> _serviceProxyByEndPoint;
        private Dictionary<Topic, Action<CacheUpdate>> _eventsByType;

        private readonly object _lockProxy = new object();
        private readonly object _lockEvent = new object();

        public SubscriptionController()
        {
            _serviceProxyByEndPoint = new Dictionary<string, DuplexServiceProxy>();
            _eventsByType = new Dictionary<Topic, Action<CacheUpdate>>();
        }

        private DuplexServiceProxy GetServiceProxy(string endPoint)
        {
            DuplexServiceProxy serviceProxy;
            bool newProxy = false;
            lock (_lockProxy)
            {
                if (!_serviceProxyByEndPoint.TryGetValue(endPoint, out serviceProxy))
                {
                    //var endpointAddress = ServiceProxy.GetEndPointFromDiscoveryServices<ISubscriptionService>(_probeEndpointAddress, suffix);
                    serviceProxy = new DuplexServiceProxy(endPoint);
                    _serviceProxyByEndPoint.Add(endPoint, serviceProxy);
                    newProxy = true;
                }
            }
            if (newProxy)
            {
                //serviceProxy.ServiceReOpened += ReOpened;
                //serviceProxy.ServiceFaulted += Faulted;
                serviceProxy.MessageReceived += OnMessage;
            }
            return serviceProxy;
        }

        public void Subscribe(Action<CacheUpdate> callback, Topic topic)
        {
            Action<CacheUpdate> cb;
            lock (_lockEvent)
            {
                if (_eventsByType.TryGetValue(topic, out cb))
                {
                    cb += callback;
                }
                else
                {
                    cb = callback;
                    _eventsByType.Add(topic, cb);
                }
            }

            var subscriptionService = GetServiceProxy(EndPoints.SubscriptionService).Instance;
            subscriptionService.Subscribe(topic);
        }

        public void UnSubscribe(Action<CacheUpdate> callback, Topic topic)
        {
            Action<CacheUpdate> cb;
            lock (_lockEvent)
            {
                _eventsByType.TryGetValue(topic, out cb);
            }
            if (cb == null) return;
            if (cb != null)
            {
                cb -= callback;
            }
            if (cb != null) return;
            try
            {
                var subscriptionService = GetServiceProxy(EndPoints.SubscriptionService).Instance;
                subscriptionService.UnSubscribe(topic);
            }
            catch (Exception ex)
            {
                Log.DebugFormat("Unsubscribe failed.", ex);
            }
        }

        public void OnMessage(Topic topic, CacheUpdate cu)
        {
            try
            {
                Action<CacheUpdate> callback;
                lock (_lockEvent)
                {
                    _eventsByType.TryGetValue(topic, out callback);
                }
                if (callback != null)
                {
                    callback(cu);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("SubscriptionController failed to process message", ex);
            }
        }

        public void Dispose()
        {
            Close();
        }

        private void Close()
        {
            throw new NotImplementedException("unsubscribe from everything here");
        }
    }
}
