using System;
using System.Collections.Generic;
using UnityEngine;

namespace SznFramework.UtilPackage.Event
{
    public class EventDispatcher : Singleton<EventDispatcher>
    {
        #region Ctor

        private readonly Dictionary<EventKey, EventDelegate> router;

        public EventDispatcher()
        {
            router = new Dictionary<EventKey, EventDelegate>((int) EventKey.Max, new EventKeyComparer());
        }

        #endregion

        #region Event

        private Action<EventKey> onEventRegister, onEventUnregister;

        /// <summary>
        /// Triggered when the event is register. 
        /// </summary>
        public void RegisterOnEventRegisterEvent(Action<EventKey> InAction)
        {
            onEventRegister += InAction;
        }

        public void UnregisterOnEventRegisterEvent(Action<EventKey> InAction)
        {
            // ReSharper disable once DelegateSubtraction
            onEventRegister -= InAction;
        }

        private void InvokeEventRegisterEvent(EventKey InEventKey)
        {
            if (null != onEventRegister) onEventRegister.Invoke(InEventKey);
        }

        /// <summary>
        /// Triggered only when the event is actively unregister. 
        /// </summary>
        public void RegisterOnEventUnregisterEvent(Action<EventKey> InAction)
        {
            onEventUnregister += InAction;
        }

        public void UnregisterOnEventUnregisterEvent(Action<EventKey> InAction)
        {
            // ReSharper disable once DelegateSubtraction
            onEventUnregister -= InAction;
        }

        private void InvokeEventUnregisterEvent(EventKey InEventKey)
        {
            if (null != onEventUnregister) onEventUnregister.Invoke(InEventKey);
        }

        #endregion

        #region Register

        public void Register(EventKey InEventKey, EventDelegate InEventDelegate)
        {
            EventDelegate cusDelegate;
            if (router.TryGetValue(InEventKey, out cusDelegate))
            {
                if (null == cusDelegate) router[InEventKey] = InEventDelegate;
                else
                {
                    if (cusDelegate.Type == InEventDelegate.Type)
                    {
                        while (cusDelegate.NextNode != null)
                        {
                            cusDelegate = cusDelegate.NextNode;
                        }

                        cusDelegate.NextNode = InEventDelegate;
                        InEventDelegate.PrevNode = cusDelegate;
                    }
                    else
                        Debug.LogError(string.Format(
                            "Register event error, the current event requires {0} parameter, but the new event requires {1} parameters.",
                            cusDelegate.Type, InEventDelegate.Type));
                }
            }
            else
            {
                //Add default chain header, easy to handle when unregistering events
                EventDelegate head = EventDelegate.RegisterHead(InEventDelegate.Type);
                head.NextNode = InEventDelegate;
                InEventDelegate.PrevNode = head;
                router.Add(InEventKey, head);
            }

            InvokeEventRegisterEvent(InEventKey);
        }

        public void Register(EventKey InEventKey, Action InAction, bool InIsSingleTime = false)
        {
            Register(InEventKey, EventDelegate.Register(InAction, InIsSingleTime));
        }

        public void Register<T>(EventKey InEventKey, Action<T> InAction, bool InIsSingleTime = false)
        {
            Register(InEventKey, EventDelegate.Register(InAction, InIsSingleTime));
        }

        public void Register<T, T1>(EventKey InEventKey, Action<T, T1> InAction, bool InIsSingleTime = false)
        {
            Register(InEventKey, EventDelegate.Register(InAction, InIsSingleTime));
        }

        public void Register<T, T1, T2>(EventKey InEventKey, Action<T, T1, T2> InAction, bool InIsSingleTime = false)
        {
            Register(InEventKey, EventDelegate.Register(InAction, InIsSingleTime));
        }

        public void Register<T, T1, T2, T3>(EventKey InEventKey, Action<T, T1, T2, T3> InAction,
            bool InIsSingleTime = false)
        {
            Register(InEventKey, EventDelegate.Register(InAction, InIsSingleTime));
        }

        #endregion

        #region Unregister

        private void Unregister(EventKey InEventKey, Delegate InDelegate, EventDelegateType InEventDelegateType)
        {
            EventDelegate eventDelegate;
            if (router.TryGetValue(InEventKey, out eventDelegate))
            {
                eventDelegate = eventDelegate.NextNode;
                if (null == eventDelegate)
                {
                    Debug.LogError(string.Format("Unregister event named {0}.", InEventKey));
                }
                else
                {
                    do
                    {
                        if (eventDelegate.Equals(InDelegate, InEventDelegateType))
                        {
                            if (eventDelegate.NextNode == null)
                            {
                                eventDelegate.PrevNode.NextNode = null;
                            }
                            else
                            {
                                eventDelegate.PrevNode.NextNode = eventDelegate.NextNode;
                                eventDelegate.NextNode.PrevNode = eventDelegate.PrevNode;
                            }
                        }

                        eventDelegate = eventDelegate.NextNode;
                    } while (null != eventDelegate);
                }
            }

            InvokeEventUnregisterEvent(InEventKey);
        }

        public void Unregister(EventKey InEventKey, Action InAction)
        {
            Unregister(InEventKey, InAction, EventDelegateType.None);
        }

        public void Unregister<T>(EventKey InEventKey, Action<T> InAction)
        {
            Unregister(InEventKey, InAction, EventDelegateType.One);
        }

        public void Unregister<T, T1>(EventKey InEventKey, Action<T, T1> InAction)
        {
            Unregister(InEventKey, InAction, EventDelegateType.Two);
        }

        public void Unregister<T, T1, T2>(EventKey InEventKey, Action<T, T1, T2> InAction)
        {
            Unregister(InEventKey, InAction, EventDelegateType.Three);
        }

        public void Unregister<T, T1, T2, T3>(EventKey InEventKey, Action<T, T1, T2, T3> InAction)
        {
            Unregister(InEventKey, InAction, EventDelegateType.Four);
        }

        #endregion

        #region Trigger

        public void Trigger(EventKey InEventKey)
        {
            EventDelegate eventDelegate;
            if (router.TryGetValue(InEventKey, out eventDelegate))
            {
                //he first node is the default empty node.
                eventDelegate = eventDelegate.NextNode;
                if (null == eventDelegate)
                {
                    Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
                }
                else
                {
                    do
                    {
                        eventDelegate = eventDelegate.Trigger();
                    } while (eventDelegate != null);
                }
            }
            else Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
        }

        public void Trigger<T>(EventKey InEventKey, T InParam)
        {
            EventDelegate eventDelegate;
            if (router.TryGetValue(InEventKey, out eventDelegate))
            {
                //he first node is the default empty node.
                eventDelegate = eventDelegate.NextNode;
                if (null == eventDelegate)
                {
                    Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
                }
                else
                {
                    do
                    {
                        eventDelegate = eventDelegate.Trigger(InParam);
                    } while (eventDelegate != null);
                }
            }
            else Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
        }

        public void Trigger<T, T1>(EventKey InEventKey, T InParam, T1 InParam1)
        {
            EventDelegate eventDelegate;
            if (router.TryGetValue(InEventKey, out eventDelegate))
            {
                //he first node is the default empty node.
                eventDelegate = eventDelegate.NextNode;
                if (null == eventDelegate)
                {
                    Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
                }
                else
                {
                    do
                    {
                        eventDelegate = eventDelegate.Trigger(InParam, InParam1);
                    } while (eventDelegate != null);
                }
            }
            else Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
        }

        public void Trigger<T, T1, T2>(EventKey InEventKey, T InParam, T1 InParam1, T2 InParam2)
        {
            EventDelegate eventDelegate;
            if (router.TryGetValue(InEventKey, out eventDelegate))
            {
                //he first node is the default empty node.
                eventDelegate = eventDelegate.NextNode;
                if (null == eventDelegate)
                {
                    Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
                }
                else
                {
                    do
                    {
                        eventDelegate = eventDelegate.Trigger(InParam, InParam1, InParam2);
                    } while (eventDelegate != null);
                }
            }
            else Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
        }

        public void Trigger<T, T1, T2, T3>(EventKey InEventKey, T InParam, T1 InParam1, T2 InParam2, T3 InParam3)
        {
            EventDelegate eventDelegate;
            if (router.TryGetValue(InEventKey, out eventDelegate))
            {
                //he first node is the default empty node.
                eventDelegate = eventDelegate.NextNode;
                if (null == eventDelegate)
                {
                    Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
                }
                else
                {
                    do
                    {
                        eventDelegate = eventDelegate.Trigger(InParam, InParam1, InParam2, InParam3);
                    } while (eventDelegate != null);
                }
            }
            else Debug.LogWarning(string.Format("No event named {0} is registered", InEventKey));
        }

        #endregion

        #region Handle

        public void ClearAll()
        {
            router.Clear();
        }

        #endregion
    }
}