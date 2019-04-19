using System;
using System.Collections.Generic;
using UnityEngine;

namespace SznFramework.UtilPackage
{
    public class TimerTools : MonoBehaviour
    {
        private const float INTERVALS = .1f;
        public class TriggerItem
        {
            private float time;
            private readonly Action callback;
            private readonly bool isBinding;
            private readonly MonoBehaviour binding;
            private bool isCompleted;
            public bool IsCompleted
            {
                get { return isCompleted; }
            }

            public TriggerItem(float InTime, Action InCallback, MonoBehaviour InBinding)
            {
                time = InTime;
                callback = InCallback;
                binding = InBinding;
                isBinding = binding != null;
                isCompleted = false;
            }

            public void Update()
            {
                if (isCompleted) return;

                time -= INTERVALS;
                if (time < float.Epsilon)
                {
                    Debug.LogError("Finished isBinding = " + isBinding + "<>" + (null == binding));
                    isCompleted = true;

                    if (null != callback)
                    {
                        if (isBinding)
                        {
                            if(null != binding) callback.Invoke();
                            else Debug.LogError("Binding GameObject is Destroyed.");
                        }
                        else callback.Invoke();
                    }
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

        private List<TriggerItem> triggerList;
        private int triggerCount;
        public void RegisterTrigger(float InTime, Action InCallback, MonoBehaviour InBinding = null)
        {
            if (null == triggerList)
            {
                Debug.LogError("New Trigger List.");
                triggerList = new List<TriggerItem>(8);
                triggerCount = 0;
            }

            TriggerItem item = new TriggerItem(InTime, InCallback, InBinding);
            if (triggerList.Count == triggerCount)
            {
                Debug.LogError("Add Trigger " + triggerCount + "<>" + triggerList.Count);
                triggerList.Add(item);
            }
            else
            {
                Debug.LogError("Insert Trigger " + triggerCount + "<>" + triggerList.Count);
                triggerList[triggerCount] = item;
            }
            ++triggerCount;
            if (!IsInvoking("TriggerInvoke")) InvokeRepeating("TriggerInvoke", 0, INTERVALS);
        }

        private void TriggerInvoke()
        {

            for (int i = 0; i < triggerCount; i++)
            {
                if (triggerList[i].IsCompleted)
                {
                    --triggerCount;
                    triggerList[i] = triggerList[triggerCount];
                    triggerList[triggerCount] = null;
                    if (i == triggerCount) break;
                }

                triggerList[i].Update();
            }

            if (triggerCount == 0)
            {
                Debug.LogError("CancelInvoke");
                CancelInvoke("TriggerInvoke");
            }
        }
    }
}

