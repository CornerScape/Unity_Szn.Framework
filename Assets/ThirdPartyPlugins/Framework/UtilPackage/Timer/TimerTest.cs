using System.Collections;
using System.Collections.Generic;
using SznFramework.UtilPackage;
using UnityEngine;
using UnityEngine.UI;

public class TimerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private int left, right;
	// Update is called once per frame
	void OnGUI ()
    {
        GUILayout.BeginArea(new Rect(Screen.width * .5f, Screen.height * .5f, Screen.width * .5f, Screen.height * .5f));
        GUI.skin.button.fontSize = 32;
        if (GUILayout.Button("Trigger"))
        {
            TimerTools.Instance.RegisterTrigger(6,
                () =>
                {
                    Debug.LogError("Finished Trigger");
                },
                this);
        }

        if (GUILayout.Button("Float Countdown"))
        {
            TimerTools.Instance.RegisterCountdown(5.5f, InTime =>
            {
                Debug.LogError("Countdown = " + InTime);
            }, this);
        }

        if (GUILayout.Button("Int Countdown"))
        {
            TimerTools.Instance.RegisterCountdown(10, InTime =>
            {
                Debug.LogError("Countdown = " + InTime);
            }, this);
        }


        if (GUILayout.Button("Text Int Countdown"))
        {
            Text text = transform.Find(string.Format("Left/Left ({0})", left)).GetComponent<Text>();
            TimerTools.Instance.RegisterCountdown(10, text, this);
            ++left;
        }

        if (GUILayout.Button("Text Float Countdown"))
        {
            Text text = transform.Find(string.Format("Right/Right ({0})", right) ).GetComponent<Text>();
            TimerTools.Instance.RegisterCountdown(5.5f, text, this);
            ++right;
        }

        if (GUILayout.Button("Destroy"))
        {
            DestroyImmediate(this);
        }
        GUILayout.EndArea();
    }
}
