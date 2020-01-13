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

            if (GUILayout.Button("open A"))
            {
                UIManager.Instance.OpenUI(UIKey.A);
            }

            if (GUILayout.Button("close A"))
            {
                UIManager.Instance.CloseUI(UIKey.A);
            }

            if (GUILayout.Button("open B"))
            {
                UIManager.Instance.OpenUI(UIKey.B);
            }

            if (GUILayout.Button("close B"))
            {
                UIManager.Instance.CloseUI(UIKey.B);
            }

            if (GUILayout.Button("open C"))
            {
                UIManager.Instance.OpenUI(UIKey.C);
            }

            if (GUILayout.Button("close C"))
            {
                UIManager.Instance.CloseUI(UIKey.C);
            }

            if (GUILayout.Button("open D"))
            {
                UIManager.Instance.OpenUI(UIKey.D);
            }

            if (GUILayout.Button("close D"))
            {
                UIManager.Instance.CloseUI(UIKey.D);
            }

            if (GUILayout.Button("open E"))
            {
                UIManager.Instance.OpenUI(UIKey.E);
            }

            if (GUILayout.Button("close E"))
            {
                UIManager.Instance.CloseUI(UIKey.E);
            }
        };
    }
}