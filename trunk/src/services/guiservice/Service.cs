using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Services.GuiService
{
    public class Service : StandardService
    {
        private readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private ICachedTypes _cachedTypes;

        private IList<ICacheLoader> _cacheLoaders;
        private IAccountManagerFactory _accountManagerFactory;
        private ICategoryManagerFactory _categoryManagerFactory;
        private ITransactionManagerFactory _transactionManagerFactory;
        private ICacheSync _cacheSync;

        private FaultHandledServiceHost _subscriptionServiceHost;

        private FaultHandledServiceHost _guiServiceHost;
        private CancellationTokenSource _guiTokenSource;
        private Task _guiTaskWorker;

        private CancellationTokenSource _syncTokenSource;
        private Task _syncTaskWorker;

        public Service(ICachedTypes cachedTypes, IList<ICacheLoader> cacheLoaders,
            IAccountManagerFactory accountManagerFactory, ICategoryManagerFactory categoryManagerFactory, ITransactionManagerFactory transactionManagerFactory,
            ICacheSync cacheSync)
        {
            _cachedTypes = cachedTypes;
            _cacheLoaders = cacheLoaders;
            _accountManagerFactory = accountManagerFactory;
            _categoryManagerFactory = categoryManagerFactory;
            _transactionManagerFactory = transactionManagerFactory;
            _cacheSync = cacheSync;
        }

        protected override void OnStart(string[] args)
        {
            InitializeGuiTaskWorker();
            InitializeSyncTaskWorker();
        }

        protected override void OnStop()
        {
            Log.InfoFormat("GuiServiceWorker stopping...");
            if (!_guiTokenSource.IsCancellationRequested)
                _guiTokenSource.Cancel();
            _guiServiceHost.Close();
            Log.InfoFormat("GuiServiceWorker stopped");

            Log.InfoFormat("SyncServiceWorker stopping...");
            if (!_syncTokenSource.IsCancellationRequested)
                _syncTokenSource.Cancel();
            Log.InfoFormat("SyncServiceWorker stopped");
        }

        private void InitializeGuiTaskWorker()
        {
            Log.InfoFormat("GuiServiceWorker starting...");
            _guiTokenSource = new CancellationTokenSource();
            _guiTaskWorker = Task.Factory.StartNew(() =>
            {
                try
                {
                    Log.DebugFormat("Forcing garbage collection before gui service starts");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (_guiTokenSource.IsCancellationRequested)
                        return;

                    Log.InfoFormat("Loading cache...");
                    foreach (var loader in _cacheLoaders)
                    {
                        loader.Start();
                    }
                    Log.InfoFormat("Succesfully loaded cache");

                    var announcementEndpointAddress = ConfigurationManager.AppSettings["announcementEndpointAddress"];

                    _guiServiceHost = new FaultHandledServiceHost(typeof(GuiService), CreateGuiService);
                    _guiServiceHost.Open(announcementEndpointAddress);

                    _subscriptionServiceHost = new FaultHandledServiceHost(typeof(SubscriptionService), CreateSubscriptionService);
                    _subscriptionServiceHost.Open(announcementEndpointAddress);

                    Log.InfoFormat("GuiService ready...");
                    while (!_guiTokenSource.IsCancellationRequested)
                        Thread.Sleep(5000);
                }
                catch (FaultException ex)
                {
                    Log.ErrorFormat("A fault exception occurred.", ex);
                }
                catch (EndpointNotFoundException)
                {
                    Log.InfoFormat("Subscription service is currently down, gui service will continue once server has been restarted.");
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("An unexpected error occurred.", ex);
                }
            }, _guiTokenSource.Token)
            .ContinueWith((t) =>
            {
                Log.ErrorFormat("GuiServiceWorker completed...");
                if (!_guiTokenSource.IsCancellationRequested)
                {
                    Log.ErrorFormat("GuiServiceWorker died prematurely, restarting...");
                    Thread.Sleep(5000);
                    InitializeGuiTaskWorker();
                }
            });
        }

        private void InitializeSyncTaskWorker()
        {
            Log.InfoFormat("SyncServiceWorker starting...");
            _syncTokenSource = new CancellationTokenSource();
            _syncTaskWorker = Task.Factory.StartNew(() =>
            {
                try
                {
                    Log.DebugFormat("Forcing garbage collection before sync service starts");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (_syncTokenSource.IsCancellationRequested)
                        return;

                    Log.InfoFormat("CacheSync starting...");
                    _cacheSync.Start();

                    Log.InfoFormat("SyncService ready...");
                    while (!_syncTokenSource.IsCancellationRequested)
                        Thread.Sleep(5000);
                }
                catch (FaultException ex)
                {
                    Log.ErrorFormat("A fault exception occurred.", ex);
                }
                catch (EndpointNotFoundException)
                {
                    Log.InfoFormat("Subscription service is currently down, sync service will continue once server has been restarted.");
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("An unexpected error occurred.", ex);
                }
            }, _syncTokenSource.Token)
            .ContinueWith((t) =>
            {
                Log.InfoFormat("CacheSync stopping...");
                _cacheSync.Stop();

                Log.ErrorFormat("SyncServiceWorker completed...");
                if (!_syncTokenSource.IsCancellationRequested)
                {
                    Log.ErrorFormat("SyncServiceWorker died prematurely, restarting...");
                    Thread.Sleep(5000);
                    InitializeSyncTaskWorker();
                }
            });
        }

        private IGuiService CreateGuiService()
        {
            return new GuiService(_accountManagerFactory, _categoryManagerFactory, _transactionManagerFactory);
        }

        private ISubscriptionService CreateSubscriptionService()
        {
            return new SubscriptionService(_cachedTypes);
        }
    }
}
