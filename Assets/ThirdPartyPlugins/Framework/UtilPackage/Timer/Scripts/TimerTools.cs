using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Szn.Framework.UtilPackage
{
    public class TimerTools : MonoBehaviour
    {
        private const float INTERVALS_F = .1f;
        private const int INTERVALS_I = 1;

        public enum TimeType
        {
            HhMmSs,
            MmSs,
            Ss
        }

        private enum TimerState
        {
            Running,
            Completed,
            Canceled
        }

        private abstract class Timer
        {
            protected readonly bool IsBinding;
            protected readonly MonoBehaviour Binding;
            public bool IsCompleted { get; protected set; }

            protected Timer(MonoBehaviour InBinding)
            {
                Binding = InBinding;
                IsBinding = InBinding != null;
            }

            public abstract bool CheckTimer();

            protected abstract TimerState CheckTimerState();

            public abstract void Update();
        }

        private abstract class TimerFloat : Timer
        {
            protected float Time;

            protected TimerFloat(float InTime, MonoBehaviour InBinding) : base(InBinding)
            {
                Time = InTime;
            }

            protected override TimerState CheckTimerState()
            {
                if (IsBinding && null == Binding) return TimerState.Canceled;

                Time -= INTERVALS_F;
                if (Time < INTERVALS_F)
                {
                    IsCompleted = true;
                    Time = 0;
                    return TimerState.Completed;
                }

                return TimerState.Running;
            }
        }

        private abstract class TimerInt : Timer
        {
            protected int Time;

            protected TimerInt(int InTime, MonoBehaviour InBinding) : base(InBinding)
            {
                Time = InTime;
            }

            protected override TimerState CheckTimerState()
            {
                if (IsBinding && null == Binding) return TimerState.Canceled;

                Time -= INTERVALS_I;
                if (Time == 0)
                {
                    IsCompleted = true;
                    Time = 0;
                    return TimerState.Completed;
                }

                return TimerState.Running;
            }
        }

        private sealed class TimerTriggerInt : TimerInt
        {
            private readonly Action callback;

            public TimerTriggerInt(int InTime, Action InCallback, MonoBehaviour InBinding) : base(InTime, InBinding)
            {
                callback = InCallback;
            }

            public override bool CheckTimer()
            {
                return Time != 0 && callback != null;
            }

            public override void Update()
            {
                if (CheckTimerState() == TimerState.Completed) callback.Invoke();
            }
        }

        private sealed class TimerTriggerFloat : TimerFloat
        {
            private readonly Action callback;
            public TimerTriggerFloat(float InTime, Action InCallback, MonoBehaviour InBinding) : base(InTime, InBinding)
            {
                callback = InCallback;
            }

            public override bool CheckTimer()
            {
                return Time >= INTERVALS_F && callback != null;
            }

            public override void Update()
            {
                if (CheckTimerState() == TimerState.Completed) callback.Invoke();
            }
        }

        private sealed class TimerCountdownNumInt : TimerInt
        {
            private readonly Action<int> runningCallback;
            private readonly Action finishedCallback;

            public TimerCountdownNumInt(int InTime, Action<int> InRunningCallback,
                Action InFinishedCallback,
                MonoBehaviour InBinding)
                : base(InTime, InBinding)
            {
                runningCallback = InRunningCallback;
                finishedCallback = InFinishedCallback;
                if (CheckTimer()) runningCallback.Invoke(InTime);
            }

            public override bool CheckTimer()
            {
                return Time != 0 && runningCallback != null;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        runningCallback.Invoke(Time);
                        break;

                    case TimerState.Completed:
                        if (null != finishedCallback) finishedCallback.Invoke();
                        break;

                    case TimerState.Canceled:
                        break;
                }
            }
        }

        private sealed class TimerCountdownNumFloat : TimerFloat
        {
            private readonly Action<float> runningCallback;
            private readonly Action finishedCallback;

            public TimerCountdownNumFloat(float InTime, Action<float> InRunningCallback,
                Action InFinishedCallback,
                MonoBehaviour InBinding)
                : base(InTime, InBinding)
            {
                runningCallback = InRunningCallback;
                finishedCallback = InFinishedCallback;
                if (CheckTimer()) runningCallback.Invoke(InTime);
            }

            public override bool CheckTimer()
            {
                return Time >= INTERVALS_F && runningCallback != null;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        runningCallback.Invoke(Time);
                        break;

                    case TimerState.Completed:
                        if (null != finishedCallback) finishedCallback.Invoke();
                        break;

                    case TimerState.Canceled:
                        break;
                }
            }
        }

        private sealed class CountDownHhMmSsTimeItem : TimerInt
        {
            private readonly Action<string> runningCallback;
            private readonly Action finishedCallback;

            public CountDownHhMmSsTimeItem(int InIntTime, Action<string> InRunningCallback,
                Action InFinishedCallback,
                MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                runningCallback = InRunningCallback;
                finishedCallback = InFinishedCallback;
                if (CheckTimer()) runningCallback.Invoke(string.Format("{0:D2}:{1:D2}:{2:D2}", Time / 3600, Time % 3600 / 60, Time % 60));
            }

            public override bool CheckTimer()
            {
                return Time != 0 && runningCallback != null;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        runningCallback.Invoke(string.Format("{0:D2}:{1:D2}:{2:D2}", Time / 3600, Time % 3600 / 60, Time % 60));
                        break;

                    case TimerState.Completed:
                        runningCallback.Invoke("00:00:00");
                        if (null != finishedCallback) finishedCallback.Invoke();
                        break;
                }
            }
        }

        private sealed class CountDownMmSsTimeItem : TimerInt
        {
            private readonly Action<string> runningCallback;
            private readonly Action finishedCallback;

            public CountDownMmSsTimeItem(int InIntTime, Action<string> InRunningCallback,
                Action InFinishedCallback,
                MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                runningCallback = InRunningCallback;
                finishedCallback = InFinishedCallback;
                if (CheckTimer()) runningCallback.Invoke(string.Format("{0:D2}:{1:D2}", Time % 3600 / 60, Time % 60));
            }

            public override bool CheckTimer()
            {
                return Time != 0 && null != runningCallback;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        runningCallback.Invoke(string.Format("{0:D2}:{1:D2}", Time % 3600 / 60, Time % 60));
                        break;

                    case TimerState.Completed:
                        runningCallback.Invoke("00:00");
                        if (null != finishedCallback) finishedCallback.Invoke();
                        break;
                }
            }
        }

        private sealed class CountDownSsTimeItem : TimerInt
        {
            private readonly Action<string> runningCallback;
            private readonly Action finishedCallback;

            public CountDownSsTimeItem(int InIntTime, Action<string> InRunningCallback,
                Action InFinishedCallback,
                MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                runningCallback = InRunningCallback;
                finishedCallback = InFinishedCallback;
                if (CheckTimer()) runningCallback.Invoke(string.Format("{0:D2}", Time));
            }

            public override bool CheckTimer()
            {
                return Time != 0 && null != runningCallback;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        runningCallback.Invoke(string.Format("{0:D2}", Time));
                        break;

                    case TimerState.Completed:
                        runningCallback.Invoke("00");
                        if (null != finishedCallback) finishedCallback.Invoke();
                        break;
                }
            }
        }

        private sealed class CountDownHhMmSsTextItem : TimerInt
        {
            private readonly Text uiText;
            private readonly Action callback;

            public CountDownHhMmSsTextItem(int InIntTime, Text InText,
                Action InFinishedCallback,
                MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                uiText = InText;
                callback = InFinishedCallback;
                if (CheckTimer()) uiText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", Time / 3600, Time % 3600 / 60, Time % 60);
            }

            public override bool CheckTimer()
            {
                return Time != 0 && null != uiText;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        uiText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", Time / 3600, Time % 3600 / 60, Time % 60);
                        break;

                    case TimerState.Completed:
                        uiText.text = "00:00:00";
                        if (null != callback) callback.Invoke();
                        break;
                }
            }
        }

        private sealed class CountDownMmSsTextItem : TimerInt
        {
            private readonly Text uiText;
            private readonly Action finishedCallback;

            public CountDownMmSsTextItem(int InIntTime, Text InText, Action InFinishedCallback,
                MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                uiText = InText;
                finishedCallback = InFinishedCallback;
                if (CheckTimer()) uiText.text = string.Format("{0:D2}:{1:D2}", Time % 3600 / 60, Time % 60);
            }

            public override bool CheckTimer()
            {
                return Time != 0 && null != uiText;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        uiText.text = string.Format("{0:D2}:{1:D2}", Time % 3600 / 60, Time % 60);
                        break;

                    case TimerState.Completed:
                        uiText.text = "00:00";
                        if (null != finishedCallback) finishedCallback.Invoke();
                        break;
                }
            }
        }

        private sealed class CountDownSsTextItem : TimerInt
        {
            private readonly Text uiText;
            private readonly Action finishedCallback;

            public CountDownSsTextItem(int InIntTime, Text InText, Action InFinishedCallback,
                MonoBehaviour InBinding = null)
                : base(InIntTime, InBinding)
            {
                uiText = InText;
                finishedCallback = InFinishedCallback;
                if (CheckTimer()) uiText.text = string.Format("{0:D2}", Time);
            }

            public override bool CheckTimer()
            {
                return Time != 0 && null != uiText;
            }

            public override void Update()
            {
                switch (CheckTimerState())
                {
                    case TimerState.Running:
                        uiText.text = string.Format("{0:D2}", Time);
                        break;

                    case TimerState.Completed:
                        uiText.text = "00";
                        if (null != finishedCallback) finishedCallback.Invoke();
                        break;

                    case TimerState.Canceled:
                        break;
                }
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
        private readonly List<TimerInt> timerListInt;
        private readonly List<TimerFloat> timerListFloat;
        private int floatTimerCount, intTimerCount;
        private const string INVOKE_FLOAT_FUNCTION_NAME_S = "FloatInvoke";
        private const string INVOKE_INT_FUNCTION_NAME_S = "IntInvoke";

        private TimerTools()
        {
            timerListInt = new List<TimerInt>(INITIAL_SIZE_I);
            intTimerCount = 0;

            timerListFloat = new List<TimerFloat>(INITIAL_SIZE_I);
            floatTimerCount = 0;
        }

        public void RegisterTrigger(float InTime, Action InCallback, MonoBehaviour InBinding = null)
        {
            RegisterTimerFloat(new TimerTriggerFloat(InTime, InCallback, InBinding));
        }

        public void RegisterTrigger(int InTime, Action InCallback, MonoBehaviour InBinding = null)
        {
            RegisterTimerInt(new TimerTriggerInt(InTime, InCallback, InBinding));
        }

        public void RegisterCountdown(float InTime, Action<float> InRunningCallback,
            Action InFinishedCallback = null, MonoBehaviour InBinding = null)
        {
            RegisterTimerFloat(new TimerCountdownNumFloat(InTime, InRunningCallback, InFinishedCallback, InBinding));
        }

        public void RegisterCountdown(int InTime, Action<int> InRunningCallback,
            Action InFinishedCallback = null,
            MonoBehaviour InBinding = null)
        {
            RegisterTimerInt(new TimerCountdownNumInt(InTime, InRunningCallback, InFinishedCallback, InBinding));
        }

        public void RegisterCountdown(int InTime, TimeType InTimeType, Action<string> InRunningCallback,
            Action InFinishedCallback = null, MonoBehaviour InBinding = null)
        {
            switch (InTimeType)
            {
                case TimeType.HhMmSs:
                    RegisterTimerInt(new CountDownHhMmSsTimeItem(InTime, InRunningCallback, InFinishedCallback, InBinding));
                    break;

                case TimeType.MmSs:
                    RegisterTimerInt(new CountDownMmSsTimeItem(InTime, InRunningCallback, InFinishedCallback, InBinding));
                    break;

                case TimeType.Ss:
                    RegisterTimerInt(new CountDownSsTimeItem(InTime, InRunningCallback, InFinishedCallback, InBinding));
                    break;
            }
        }

        public void RegisterCountdown(int InTime, TimeType InTimeType, Text InText,
            Action InFinishedCallback = null, MonoBehaviour InBinding = null)
        {
            switch (InTimeType)
            {
                case TimeType.HhMmSs:
                    RegisterTimerInt(new CountDownHhMmSsTextItem(InTime, InText, InFinishedCallback, InBinding));
                    break;

                case TimeType.MmSs:
                    RegisterTimerInt(new CountDownMmSsTextItem(InTime, InText, InFinishedCallback, InBinding));
                    break;

                case TimeType.Ss:
                    RegisterTimerInt(new CountDownSsTextItem(InTime, InText, InFinishedCallback, InBinding));
                    break;
            }
        }

        private void RegisterTimerInt(TimerInt InTimerInt)
        {
            lock (timerListInt)
            {
                if (timerListInt.Count == intTimerCount)
                {
                    //Debug.LogError("Add TimerTriggerInt " + intTimerCount + "<>" + timerListInt.Count);
                    timerListInt.Add(InTimerInt);
                }
                else
                {
                    //Debug.LogError("Insert TimerTriggerInt " + intTimerCount + "<>" + timerListInt.Count);
                    timerListInt[intTimerCount] = InTimerInt;
                }

                ++intTimerCount;
                if (!IsInvoking(INVOKE_INT_FUNCTION_NAME_S))
                    InvokeRepeating(INVOKE_INT_FUNCTION_NAME_S, 0, INTERVALS_I);
            }
        }

        private void RegisterTimerFloat(TimerFloat InTimerFloat)
        {
            lock (timerListFloat)
            {
                if (timerListFloat.Count == floatTimerCount)
                {
                    //Debug.LogError("Add TimerTriggerInt " + floatTimerCount + "<>" + timerListFloat.Count);
                    timerListFloat.Add(InTimerFloat);
                }
                else
                {
                    //Debug.LogError("Insert TimerTriggerInt " + floatTimerCount + "<>" + timerListFloat.Count);
                    timerListFloat[floatTimerCount] = InTimerFloat;
                }

                ++floatTimerCount;
                if (!IsInvoking(INVOKE_FLOAT_FUNCTION_NAME_S))
                    InvokeRepeating(INVOKE_FLOAT_FUNCTION_NAME_S, 0, INTERVALS_F);
            }
        }

        private void FloatInvoke()
        {
            lock (timerListFloat)
            {
                for (int i = 0; i < floatTimerCount; i++)
                {
                    if (timerListFloat[i].IsCompleted)
                    {
                        --floatTimerCount;
                        if (i == floatTimerCount)
                        {
                            timerListFloat[floatTimerCount] = null;
                            break;
                        }

                        timerListFloat[i] = timerListFloat[floatTimerCount];
                        timerListFloat[floatTimerCount] = null;
                    }

                    timerListFloat[i].Update();
                }

                if (floatTimerCount == 0)
                {
                    //Debug.LogError("Cancel Float Invoke");
                    CancelInvoke(INVOKE_FLOAT_FUNCTION_NAME_S);
                }
            }
        }

        private void IntInvoke()
        {
            lock (timerListInt)
            {
                for (int i = 0; i < intTimerCount; i++)
                {
                    if (timerListInt[i].IsCompleted)
                    {
                        --intTimerCount;
                        if (i == intTimerCount)
                        {
                            timerListInt[intTimerCount] = null;
                            break;
                        }

                        timerListInt[i] = timerListInt[intTimerCount];
                        timerListInt[intTimerCount] = null;
                    }

                    timerListInt[i].Update();
                }

                if (intTimerCount == 0)
                {
                    Debug.LogError("Cancel Int Invoke");
                    CancelInvoke(INVOKE_INT_FUNCTION_NAME_S);
                }
            }
        }
    }
}