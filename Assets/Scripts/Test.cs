using Szn.Framework.Audio;
using Szn.Framework.UtilPackage;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Awake()
    {
        UnityGuiTest.UnityGUIAction += () =>
        {
            if (GUILayout.Button("Piano"))
            {
                AudioManager.Instance.PlayEffect(AudioKey.Piano);
            }

            if (GUILayout.Button("Piano2_6ra"))
            {
                AudioManager.Instance.PlayEffect(AudioKey.Piano2_6ra);
            }

            if (GUILayout.Button("Piano2_6si"))
            {
                AudioManager.Instance.PlayEffect(AudioKey.Piano2_7si);
            }
        };
    }
}

