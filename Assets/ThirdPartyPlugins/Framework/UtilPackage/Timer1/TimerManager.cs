using UnityEngine;

namespace Framework.Timer
{
    public sealed class TimerManager : MonoBehaviour
    {
        private TimerBase[] timerArray;
        private int timerArrayLen = 15;

        private int timerArrayUsefullyLen; 
        
        private static TimerManager instance;

        public static TimerManager GetSingleton()
        {
            if (null == instance)
            {
                instance = FindObjectOfType<TimerManager>();
                if (null == instance)
                {
                    GameObject timerGameObj = new GameObject("TimerManager");
                    instance = timerGameObj.AddComponent<TimerManager>();
                    DontDestroyOnLoad(timerGameObj);
                }
            }

            return instance;
        }

        public static void DestroySingleton()
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        private void Awake()
        {
            timerArray = new TimerBase[timerArrayLen];
            //timerDict = new Dictionary<int, TimerBase>(10);
            //timerCount = 0;
            //completedIndexList = new List<int>(10);
            //completedIndexCount = 0;
        }

        public void AddTimer(TimerBase InTimerBase)
        {
            for (int i = 0; i < timerArrayLen; i++)
            {
                if (timerArray[i] == null)
                {
                    timerArray[i] = InTimerBase;
                    break;
                }
            }

            ++timerArrayUsefullyLen;
        }

        private float time;

        private void Update()
        {
            if (timerArrayUsefullyLen == 0) return;

            time += Time.deltaTime;

            if (time < TimerConfig.DELTA_TIME) return;
            time -= TimerConfig.DELTA_TIME;


            for (int i = 0; i < timerArrayLen; i++)
            {
                if (timerArray[i] != null)
                {
                    TimerBase curTimer = timerArray[i];
                    curTimer.Update();
                    if (curTimer.IsDone)
                    {
                        timerArray[i] = null;
                        --timerArrayUsefullyLen;
                    }
                }
            }
        }
    }
}