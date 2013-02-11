using System;
using System.Collections.Generic;
using System.Threading;
using Common.Logging;
using DL.AccountChecker.Framework;
using DL.Framework.Common;
using DL.Framework.Services;

namespace DL.AccountChecker.Suite.Web
{
    public class CallbackEvent
    {
        public void ProcessMessage(CacheUpdate cu)
        {
            /*Stopwatch stopWatch = null;
            if (log.IsDebugEnabled)
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
                log.DebugFormat("ProcessMessage Called {0} {1} {2}", cu.Type, cu.Reason.ToString(), Environment.StackTrace);
            }
            else
            {
                log.InfoFormat("ProcessMessage Called {0} {1}", cu.Type, cu.Reason.ToString());
            }

            List<CacheUpdateItem> itemsToProcess = cu.UpdatedItems;
            try
            {
                lock (lockObject)
                {
                    if (!string.IsNullOrEmpty(cu.DeleteWhereClause))
                    {
                        LambdaExpression exp = DynamicExpression.ParseLambda(Type.GetType(cu.Type), typeof(bool), cu.DeleteWhereClause);
                        Delegate del = exp.Compile();
                        items.RemoveWhere(i => i.IsRequired(del));
                    }
                    if (cu.Reason == NotificationReason.NotifyUnsentComplete)
                    {
                        // Reconnection in process, remove any items cached not received from server
                        if (oldItems == null)
                            return;
                        cu = new CacheUpdate(cu.Type) { Reason = NotificationReason.Deleted, UpdatedTime = cu.UpdatedTime };
                        foreach (var item in oldItems)
                        {
                            if (!items.Contains(item.Key))
                            {
                                cu.UpdatedItems.Add(item.Key);
                            }
                        }
                        oldItems = null;
                        itemsToProcess = cu.UpdatedItems;
                    }
                    else
                    {
                        foreach (CacheUpdateItem item in itemsToProcess)
                        {
                            items.Remove(item);
                            if (cu.Reason != NotificationReason.Deleted)
                                items.Add(item);
                            CacheUpdateItem old;
                            if (oldItems != null && oldItems.TryGetValue(item, out old))
                            {
                                oldItems.Remove(item);
                                item.OldItem = old.UpdatedItem;
                            }
                        }
                    }
                    if (callbackEvent == null)
                        return;

                    foreach (Action<CacheUpdate> callback in callbackEvent.GetInvocationList())
                    {
                        IEventFilter filter;
                        if (filtersByCallback.TryGetValue(callback, out filter))
                        {
                            List<CacheUpdateItem> filterdList = itemsToProcess.Where(item => filter != null && (filter.IsRequired(item.UpdatedItem) || (item.OldItem != null && filter.IsRequired(item.OldItem)))).ToList();
                            try
                            {
                                if (filterdList.Count > 0 || cu.LastInSequence)
                                    callback(new CacheUpdate(filterdList, cu.Reason, cu.Type)
                                    {
                                        DeleteWhereClause = filterdList.Count > 0 ? cu.DeleteWhereClause : null,
                                        LastInSequence = cu.LastInSequence,
                                        UpdatedTime = cu.UpdatedTime
                                    });
                            }
                            catch
                            {
                                filtersByCallback.Remove(callback);

                                callbackEvent -= callback;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (stopWatch != null)
                {
                    stopWatch.Stop();
                    log.DebugFormat("ProcessMessage Type : {0}, Msg Count : {1}, Milliseconds : {2}", cu.Type, itemsToProcess.Count, stopWatch.ElapsedMilliseconds);
                }
                log.InfoFormat("ProcessMessage Ended {0}", cu.Type);
            }*/
        }
    }

    /*[HubName("subscriptionController")]
    public class SubscriptionController : Hub, ISubscriptionCallback
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private bool _isSubscribed;

        private IDictionary<Topic, CallbackEvent> _eventsByType = new Dictionary<Topic, CallbackEvent>();
        private readonly object _lockEventObject = new object();

        public SubscriptionController()
        {
        }

        public void Subscribe(Topic topic)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    var subscriptionId = Guid.NewGuid();
                    //proxy.Subscribe(subscriptionId, new string[] { "ClientDealEvent", "PaymentOutEvent" });
                    _isSubscribed = true;
                }
                catch
                {
                }
            });
        }

        public void UnSubscribe(Topic topic)
        {
            try
            {
                if (_isSubscribed)
                {
                    ThreadPool.QueueUserWorkItem(x =>
                    {
                        try
                        {
                            //EventBrokerProxy proxy = new EventBrokerProxy(instanceContext);
                            //proxy.EndSubscription(subscriptionId);
                        }
                        catch
                        {
                        }
                    });
                    _isSubscribed = false;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Unsubscription failed", ex);
            }
        }

        public void OnMessage(Topic topic, CacheUpdate cu)
        {
            //Hub.GetClients<EventTickerHub>().addMessage(streamingResult);

            try
            {
                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("ProcessMessage Called {0} {1}", cu.Reason.ToString(), Environment.StackTrace);
                }
                else
                {
                    Log.InfoFormat("ProcessMessage Called {0}", cu.Reason.ToString());
                }

                Clients.cacheUpdate(cu);

                CallbackEvent cbe;
                lock (_lockEventObject)
                {
                    _eventsByType.TryGetValue(topic, out cbe);
                }
                if (cbe != null)
                {
                    cbe.ProcessMessage(cu);
                }
                Log.InfoFormat("ProcessMessage Ended");
            }
            catch (Exception e)
            {
                Log.Error("SubscriptionController failed to process message", e);
            }
        }
    }*/
}
