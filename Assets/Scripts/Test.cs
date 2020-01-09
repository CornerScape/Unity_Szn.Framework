using Szn.Framework.Audio;
using Szn.Framework.UI;
using Szn.Framework.UtilPackage;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Awake()
    {
//        AudioConfig.UpdateProgressAction += (InF, InS) => Debug.Log(InF + "<>" + InS);
//        AudioConfig.UpdateCompletedCallbackAction += Debug.Log;

        UIBase uiBase = FindObjectOfType<UITest>();
        UnityGuiTest.UnityGUIAction += () =>
        {
//            if (GUILayout.Button("Piano"))
//            {
//                AudioManager.Instance.PlayEffect(AudioKey.Piano);
//            }
//
//            if (GUILayout.Button("Piano2_6ra"))
//            {
//                AudioManager.Instance.PlayEffect(AudioKey.Piano26);
//            }
//
//            if (GUILayout.Button("Piano2_6si"))
//            {
//                AudioManager.Instance.PlayEffect(AudioKey.Piano27);
//            }
//            if (GUILayout.Button("bird02"))
//            {
//                AudioManager.Instance.PlayEffect(AudioKey.Bird02);
//            }
//            
//            if (GUILayout.Button("bird03"))
//            {
//                AudioManager.Instance.PlayEffect(AudioKey.Bird03);
//            }
//            
//            if (GUILayout.Button("bird05"))
//            {
//                AudioManager.Instance.PlayEffect(AudioKey.Bird05);
//            }

            if (GUILayout.Button("SelfStart"))
            {
                uiBase.Handle.SelfLoaded();
            }

            if (GUILayout.Button("SelfOpen"))
            {
                uiBase.Handle.SelfOpen();
            }

            if (GUILayout.Button("SelfEnable"))
            {
                uiBase.Handle.SelfEnable();
            }

            if (GUILayout.Button("SelfDisable"))
            {
                uiBase.Handle.SelfDisable();
            }

            if (GUILayout.Button("SelfDestroy"))
            {
                uiBase.Handle.SelfDestroy();
            }
        };
    }
}