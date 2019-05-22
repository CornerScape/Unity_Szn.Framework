using System;
using UnityEngine;

namespace SznFramework.UtilPackage.Event
{
    public enum EventDelegateType : byte
    {
        None,
        One,
        Two,
        Three,
        Four
    }

    public class EventDelegate
    {
        public readonly Delegate CusDelegate;
        public readonly EventDelegateType Type;
        public readonly bool IsSingleTime;
        public EventDelegate PrevNode, NextNode;

        private EventDelegate(Delegate InCusDelegate, EventDelegateType InType, bool InIsSingleTime)
        {
            CusDelegate = InCusDelegate;
            Type = InType;
            IsSingleTime = InIsSingleTime;
            PrevNode = null;
            NextNode = null;
        }

        public static EventDelegate RegisterHead(EventDelegateType InEventDelegateType)
        {
            return new EventDelegate(null, InEventDelegateType, false);
        }

        public static EventDelegate Register(Action InAction, bool InIsSingleTime = false)
        {
            return new EventDelegate(InAction, EventDelegateType.None, InIsSingleTime);
        }

        public static EventDelegate Register<T>(Action<T> InAction, bool InIsSingleTime = false)
        {
            return new EventDelegate(InAction, EventDelegateType.One, InIsSingleTime);
        }

        public static EventDelegate Register<T, T1>(Action<T, T1> InAction, bool InIsSingleTime = false)
        {
            return new EventDelegate(InAction, EventDelegateType.Two, InIsSingleTime);
        }

        public static EventDelegate Register<T, T1, T2>(Action<T, T1, T2> InAction, bool InIsSingleTime = false)
        {
            return new EventDelegate(InAction, EventDelegateType.Three, InIsSingleTime);
        }

        public static EventDelegate Register<T, T1, T2, T3>(Action<T, T1, T2, T3> InAction,
            bool InIsSingleTime = false)
        {
            return new EventDelegate(InAction, EventDelegateType.Four, InIsSingleTime);
        }

        private EventDelegate MoveNext()
        {
            if (IsSingleTime)
            {
                if (NextNode == null) PrevNode.NextNode = null;
                else
                {
                    PrevNode.NextNode = NextNode;
                    NextNode.PrevNode = PrevNode;
                }
            }

            return NextNode;
        }

        public EventDelegate Trigger()
        {
            if (Type == EventDelegateType.None)
            {
                ((Action) CusDelegate).Invoke();
                return MoveNext();
            }

            Debug.LogError(
                string.Format("Invoke event error, parameters are not match.\nRegister event need {0} parameters.",
                    Type));

            return null;
        }

        public EventDelegate Trigger<T>(T InParam)
        {
            if (Type == EventDelegateType.One)
            {
                ((Action<T>) CusDelegate).Invoke(InParam);
                return MoveNext();
            }

            Debug.LogError(
                string.Format("Invoke event error, parameters are not match.\nRegister event need {0} parameters.",
                    Type));

            return null;
        }

        public EventDelegate Trigger<T, T1>(T InParam, T1 InParam1)
        {
            if (Type == EventDelegateType.Two)
            {
                ((Action<T, T1>) CusDelegate).Invoke(InParam, InParam1);
                return MoveNext();
            }

            Debug.LogError(
                string.Format("Invoke event error, parameters are not match.\nRegister event need {0} parameters.",
                    Type));

            return null;
        }

        public EventDelegate Trigger<T, T1, T2>(T InParam, T1 InParam1, T2 InParam2)
        {
            if (Type == EventDelegateType.Three)
            {
                ((Action<T, T1, T2>) CusDelegate).Invoke(InParam, InParam1, InParam2);
                return MoveNext();
            }

            Debug.LogError(
                string.Format("Invoke event error, parameters are not match.\nRegister event need {0} parameters.",
                    Type));

            return null;
        }

        public EventDelegate Trigger<T, T1, T2, T3>(T InParam, T1 InParam1, T2 InParam2, T3 InParam3)
        {
            if (Type == EventDelegateType.Four)
            {
                ((Action<T, T1, T2, T3>) CusDelegate).Invoke(InParam, InParam1, InParam2, InParam3);
                return MoveNext();
            }

            Debug.LogError(
                string.Format("Invoke event error, parameters are not match.\nRegister event need {0} parameters.",
                    Type));

            return null;
        }

        public bool Equals(Delegate InDelegate, EventDelegateType InEventDelegateType)
        {
            return Type == InEventDelegateType && CusDelegate == InDelegate;
        }
    }
}