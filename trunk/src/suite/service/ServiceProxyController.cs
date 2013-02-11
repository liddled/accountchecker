using System;
using Common.Logging;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Suite.Service
{
    public class ServiceProxyController : IDisposable
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private ServiceProxy<IGuiService> _guiServiceProxy;
        private SubscriptionController _subscriptionController;

        private readonly object _lock = new object();

        public ServiceProxyController()
        {
        }

        public IGuiService GuiService
        {
            get
            {
                if (_guiServiceProxy == null)
                {
                    lock (_lock)
                    {
                        if (_guiServiceProxy == null)
                            _guiServiceProxy = CreateServiceProxy<IGuiService>(EndPoints.GuiService);
                    }
                }
                return _guiServiceProxy.Instance;
            }
        }

        private SubscriptionController SubscriptionController
        {
            get
            {
                if (_subscriptionController == null)
                {
                    lock (_lock)
                    {
                        if (_subscriptionController == null)
                            _subscriptionController = new SubscriptionController();
                    }
                }
                return _subscriptionController;
            }
        }

        private ServiceProxy<T> CreateServiceProxy<T>(string endPoint) where T : class
        {
            //var serviceAddress = ServiceProxy.GetEndPointFromDiscoveryServices<T>(_probeEndpointAddress, _tradeSource.ToString());
            return new ServiceProxy<T>(endPoint);
        }

        public void Subscribe(Action<CacheUpdate> callback, Topic topic)
        {
            SubscriptionController.Subscribe(callback, topic);
        }

        public void UnSubscribe(Action<CacheUpdate> callback, Topic topic)
        {
            SubscriptionController.UnSubscribe(callback, topic);
        }

        public void Dispose()
        {
            Close();
        }

        private void Close()
        {
            var guiServiceProxyCopy = _guiServiceProxy;
            _guiServiceProxy = null;

            if (guiServiceProxyCopy != null)
            {
                try
                {
                    guiServiceProxyCopy.Dispose();
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to close AccountCheckerGuiService", ex);
                }
            }

            var subscriptionControllerCopy = _subscriptionController;
            _subscriptionController = null;

            if (subscriptionControllerCopy != null)
            {
                subscriptionControllerCopy.Dispose();
            }
        }
    }
}
