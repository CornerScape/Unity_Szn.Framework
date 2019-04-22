using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace SznFramework.UtilPackage
{
    public class TimerTools : MonoBehaviour
    {
        private const float INTERVALS_F = .1f;
        private const int INTERVALS_I = 1;

        private enum TimerType
        {
            Int,
            Float,
            Max
        }

        private abstract class TimerItem
        {
            protected int IntTime;
            protected float FloatTime;
            public TimerType Type { get; private set; }
            protected readonly bool IsBinding;
            protected readonly MonoBehaviour Binding;
            protected bool Completed;

            public bool IsCompleted
            {
                get { return Completed; }
            }

            protected TimerItem(int InIntTime, MonoBehaviour InBinding = null)
            {
                Type = TimerType.Int;
                IntTime = InIntTime;
                Binding = InBinding;
                IsBinding = Binding != null;
                Completed = false;
            }

            protected TimerItem(float InFloatTime, MonoBehaviour InBinding = null)
            {
                Type = TimerType.Float;
                FloatTime = InFloatTime;
                Binding = InBinding;
                IsBinding = Binding != null;
                Completed = false;
            }

            public abstract void UpdateInt();

            public abstract void UpdateFloat();
        }

        private sealed class TriggerItem : TimerItem
        {
            private readonly Action callback;

            public TriggerItem(float InFloatTime, Action InCallback, MonoBehaviour InBinding = null)
                : base(InFloatTime, InBinding)
            {
                callback = InCallback;
            }

            public TriggerItem(int InIntTime, Action InCallback, MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                callback = InCallback;
            }

            public override void UpdateInt()
            {
                if (Completed) return;

                IntTime -= INTERVALS_I;
                if (IntTime == 0)
                {
                    Debug.LogError("Finished isBinding = " + IsBinding + "<>" + (null == Binding));
                    Completed = true;

                    if (null != callback)
                    {
                        if (IsBinding)
                        {
                            if (null != Binding) callback.Invoke();
                            else Debug.LogError("Binding GameObject is Destroyed.");
                        }
                        else callback.Invoke();
                    }
                }
            }

            public override void UpdateFloat()
            {
                if (Completed) return;

                FloatTime -= INTERVALS_F;
                if (FloatTime < INTERVALS_F)
                {
                    Debug.LogError("Finished isBinding = " + IsBinding + "<>" + (null == Binding));
                    Completed = true;

                    if (null != callback)
                    {
                        if (IsBinding)
                        {
                            if (null != Binding) callback.Invoke();
                            else Debug.LogError("Binding GameObject is Destroyed.");
                        }
                        else callback.Invoke();
                    }
                }
            }
        }

        private sealed class CountDownNumItem : TimerItem
        {
            private readonly Action<float> floatCallback;
            private readonly Action<int> intCallback;

            public CountDownNumItem(float InFloatTime, Action<float> InFloatCallback, MonoBehaviour InBinding = null)
                : base(InFloatTime, InBinding)
            {
                floatCallback = InFloatCallback;
            }

            public CountDownNumItem(int InIntTime, Action<int> InIntCallback, MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                intCallback = InIntCallback;
            }

            public override void UpdateInt()
            {
                if (Completed) return;
                if (IsBinding && null == Binding) return;

                IntTime -= INTERVALS_I;
                if (IntTime == 0)
                {
                    Completed = true;
                }
                intCallback.Invoke(IntTime);
            }

            public override void UpdateFloat()
            {
                if (Completed) return;
                if (IsBinding && null == Binding) return;

                FloatTime -= INTERVALS_F;
                if (FloatTime < INTERVALS_F)
                {
                    Completed = true;
                    FloatTime = 0;
                }
                floatCallback.Invoke(FloatTime);
            }
        }

        private sealed class CountDownTextItem : TimerItem
        {
            private readonly Text showText;
            public CountDownTextItem(float InFloatTime, Text InShowText, MonoBehaviour InBinding = null)
                : base(InFloatTime, InBinding)
            {
                showText = InShowText;
            }

            public CountDownTextItem(int InIntTime, Text InShowText, MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                showText = InShowText;
            }

            public override void UpdateInt()
            {
                if (Completed) return;
                if (IsBinding && null == Binding) return;

                IntTime -= INTERVALS_I;
                if (IntTime == 0)
                {
                    Completed = true;
                }

                showText.text = IntTime.ToString(CultureInfo.InvariantCulture);
            }

            public override void UpdateFloat()
            {
                if (Completed) return;
                if (IsBinding && null == Binding) return;

                FloatTime -= INTERVALS_F;
                if (FloatTime < INTERVALS_F)
                {
                    Completed = true;
                    FloatTime = 0;
                }

                showText.text = FloatTime.ToString(CultureInfo.InvariantCulture);
            }
        }

        private static TimerTools instance;

        public static TimerTools Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = FindObjectOfType<TimerTools>();
                    if (null == instance)
                    {
                        GameObject go = new GameObject("Timer");
                        instance = go.AddComponent<TimerTools>();
                        DontDestroyOnLoad(go);
                    }
                }

                return instance;
            }
        }

        private const int INITIAL_SIZE_I = 8;
        private List<TimerItem> intTimerList, floatTimerList;
        private int floatTimerCount, intTimerCount;
        private readonly object lockObj = new object();
        private const string INVOKE_FLOAT_FUNCTION_NAME_S = "FloatInvoke";
        private const string INVOKE_INT_FUNCTION_NAME_S = "IntInvoke";
        
        public void RegisterTrigger(float InTime, Action InCallback, MonoBehaviour InBinding = null)
        {
            RegisterTimer(new TriggerItem(InTime, InCallback, InBinding));
        }

        public void RegisterTrigger(int InTime, Action InCallback, MonoBehaviour InBinding = null)
        {
            RegisterTimer(new TriggerItem(InTime, InCallback, InBinding));
        }

        public void RegisterCountdown(float InTime, Action<float> InCallback, MonoBehaviour InBinding = null)
        {
            RegisterTimer(new CountDownNumItem(InTime, InCallback, InBinding));
        }

        public void RegisterCountdown(int InTime, Action<int> InCallback, MonoBehaviour InBinding = null)
        {
            RegisterTimer(new CountDownNumItem(InTime, InCallback, InBinding));
        }

        public void RegisterCountdown(float InTime, Text InText, MonoBehaviour InBinding = null)
        {
            RegisterTimer(new CountDownTextItem(InTime, InText, InBinding));
        }

        public void RegisterCountdown(int InTime, Text InText, MonoBehaviour InBinding = null)
        {
            RegisterTimer(new CountDownTextItem(InTime, InText, InBinding));
        }

        private void RegisterTimer(TimerItem InItem)
        {
            lock (lockObj)
            {
                if (InItem.Type == TimerType.Float)
                {
                    if (null == floatTimerList)
                    {
                        floatTimerList = new List<TimerItem>(INITIAL_SIZE_I);
                        floatTimerCount = 0;
                    }

                    if (floatTimerList.Count == floatTimerCount)
                    {
                        Debug.LogError("Add Trigger " + floatTimerCount + "<>" + floatTimerList.Count);
                        floatTimerList.Add(InItem);
                    }
                    else
                    {
                        Debug.LogError("Insert Trigger " + floatTimerCount + "<>" + floatTimerList.Count);
                        floatTimerList[floatTimerCount] = InItem;
                    }

                    ++floatTimerCount;
                    if (!IsInvoking(INVOKE_FLOAT_FUNCTION_NAME_S)) InvokeRepeating(INVOKE_FLOAT_FUNCTION_NAME_S, 0, INTERVALS_F);
                }
                else
                {
                    if (null == intTimerList)
                    {
                        intTimerList = new List<TimerItem>(INITIAL_SIZE_I);
                        intTimerCount = 0;
                    }

                    if (intTimerList.Count == intTimerCount)
                    {
                        Debug.LogError("Add Trigger " + intTimerCount + "<>" + intTimerList.Count);
                        intTimerList.Add(InItem);
                    }
                    else
                    {
                        Debug.LogError("Insert Trigger " + intTimerCount + "<>" + intTimerList.Count);
                        intTimerList[intTimerCount] = InItem;
                    }

                    ++intTimerCount;
                    if (!IsInvoking(INVOKE_INT_FUNCTION_NAME_S)) InvokeRepeating(INVOKE_INT_FUNCTION_NAME_S, 0, INTERVALS_I);
                }
            }
        }

        private void FloatInvoke()
        {
            lock (lockObj)
            {
                int i = 0;
                try
                {
                    for (; i < floatTimerCount; i++)
                    {
                        if (floatTimerList[i].IsCompleted)
                        {
                            --floatTimerCount;
                            if (i == floatTimerCount)
                            {
                                floatTimerList[floatTimerCount] = null;
                                break;
                            }

                            floatTimerList[i] = floatTimerList[floatTimerCount];
                            floatTimerList[floatTimerCount] = null;
                        }

                        floatTimerList[i].UpdateFloat();
                    }

                    if (floatTimerCount == 0)
                    {
                        Debug.LogError("Cancel Float Invoke");
                        CancelInvoke(INVOKE_FLOAT_FUNCTION_NAME_S);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(i + "\n" + e.Message + "\n" + e.StackTrace);
                    throw;
                }
            }
        }

        private void IntInvoke()
        {
            lock (lockObj)
            {
                int i = 0;
                try
                {
                    for (; i < intTimerCount; i++)
                    {
                        if (intTimerList[i].IsCompleted)
                        {
                            --intTimerCount;
                            if (i == intTimerCount)
                            {
                                intTimerList[intTimerCount] = null;
                                break;
                            }

                            intTimerList[i] = intTimerList[intTimerCount];
                            intTimerList[intTimerCount] = null;
                        }

                        intTimerList[i].UpdateInt();
                    }

                    if (intTimerCount == 0)
                    {
                        Debug.LogError("Cancel Int Invoke");
                        CancelInvoke(INVOKE_INT_FUNCTION_NAME_S);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(i + "\n" + e.Message + "\n" + e.StackTrace);
                    throw;
                }
            }
        }
    }
}

