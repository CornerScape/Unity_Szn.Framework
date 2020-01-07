#if UNIT_TEST
using Szn.Framework.UtilPackage;
using UnityEngine;
using UnityEngine.UI;

namespace Szn.Framework.UnitTest
{
    public class TimerTest : MonoBehaviour
    {
        private int left, right;

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width * .5f, Screen.height * .5f, Screen.width * .5f,
                Screen.height * .5f));
            GUI.skin.button.fontSize = 32;
            if (GUILayout.Button("Trigger"))
            {
                TimerTools.Instance.RegisterTrigger(6,
                    () => { Debug.LogError("Finished Trigger"); },
                    this);
            }

            if (GUILayout.Button("Float Countdown"))
            {
                TimerTools.Instance.RegisterCountdown(5.5f, InTime => { Debug.LogError("Countdown = " + InTime); },
                    null, this);
            }

            if (GUILayout.Button("Int Countdown"))
            {
                TimerTools.Instance.RegisterCountdown(10, InTime => { Debug.LogError("Countdown = " + InTime); }, null,
                    this);
            }


            if (GUILayout.Button("Text hhmmss Countdown"))
            {
                Text text = transform.Find(string.Format("Left/Left ({0})", left)).GetComponent<Text>();
                TimerTools.Instance.RegisterCountdown(100, TimerTools.TimeType.HhMmSs, text,
                    () => { Debug.LogError(text + " Finished."); }, this);
                ++left;
            }

            if (GUILayout.Button("Text mmss Countdown"))
            {
                Text text = transform.Find(string.Format("Left/Left ({0})", left)).GetComponent<Text>();
                TimerTools.Instance.RegisterCountdown(100, TimerTools.TimeType.MmSs, text,
                    () => { Debug.LogError(text + " Finished."); }, this);
                ++left;
            }

            if (GUILayout.Button("Text ss Countdown"))
            {
                Text text = transform.Find(string.Format("Left/Left ({0})", left)).GetComponent<Text>();
                TimerTools.Instance.RegisterCountdown(200, TimerTools.TimeType.Ss, text,
                    () => { Debug.LogError(text + " Finished."); }, this);
                ++left;
            }


            if (GUILayout.Button("Time hhmmss Countdown"))
            {
                Text text = transform.Find(string.Format("Right/Right ({0})", right)).GetComponent<Text>();
                TimerTools.Instance.RegisterCountdown(100, TimerTools.TimeType.HhMmSs,
                    InTimer => { text.text = InTimer; },
                    () => { Debug.LogError(text + " Finished."); }, this);
                ++right;
            }

            if (GUILayout.Button("Time mmss Countdown"))
            {
                Text text = transform.Find(string.Format("Right/Right ({0})", right)).GetComponent<Text>();
                TimerTools.Instance.RegisterCountdown(100, TimerTools.TimeType.MmSs,
                    InTimer => { text.text = InTimer; },
                    () => { Debug.LogError(text + " Finished."); }, this);
                ++right;
            }

            if (GUILayout.Button("Time ss Countdown"))
            {
                Text text = transform.Find(string.Format("Right/Right ({0})", right)).GetComponent<Text>();
                TimerTools.Instance.RegisterCountdown(200, TimerTools.TimeType.Ss,
                    InTimer => { text.text = InTimer; },
                    () => { Debug.LogError(text + " Finished."); },
                    this);
                ++right;
            }


            if (GUILayout.Button("Destroy"))
            {
                DestroyImmediate(this);
            }

            GUILayout.EndArea();
        }
    }
}
#endif