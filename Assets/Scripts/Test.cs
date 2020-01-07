using Szn.Framework.Audio;
using UnityEngine;

public class Test : MonoBehaviour
{
    void OnGUI()
    {
        if (GUILayout.Button("A"))
        {
            AudioManager.Instance.PlayEffect(AudioKey.A);
        }
        
        if (GUILayout.Button("B"))
        {
            AudioManager.Instance.PlayEffect(AudioKey.B);
        }
        
        if (GUILayout.Button("C"))
        {
            AudioManager.Instance.PlayEffect(AudioKey.C);
        }
        
        if (GUILayout.Button("D"))
        {
            AudioManager.Instance.PlayEffect(AudioKey.D);
        }
    }
}


#define UNIT_TEST
#if UNITY_EDITOR

using ADA_Manager;
using SimpleJSON;
using UnityEngine;

namespace Ada.ThirdParty.Plugins
{
    public class ThirdPartyH5Simulate : MonoBehaviour
    {
#if CHINA_VERSION && UNIT_TEST
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            GameObject go = new GameObject("H5 Simulate");
            DontDestroyOnLoad(go);
            go.AddComponent<ThirdPartyH5Simulate>();
        }
#endif
        private float width;
        private float height;
        private Rect logWinRect;
        private Rect screenRect;

        private void Awake()
        {
            width = Screen.width;
            height = Screen.height;
            logWinRect = new Rect(width * .375f, height * .4f, width * .25f, height * .2f);
            screenRect = new Rect(0, 0, width, height);
        }

        private void OnGUI()
        {
            logWinRect = GUI.Window(0, logWinRect, LogWindow, "H5 Simulate");
        }

        private void LogWindow(int InWindowId)
        {
            if (GUILayout.Button("Ranking Back"))
            {
                TestGotoRankingInfo();
            }

            if (GUILayout.Button("Quit h5"))
            {
                JSONNode jsonNode = new JSONObject()
                {
                    ["url"] = "home"
                };
                ThirdPartyH5Dispatch.Handle("gotoUnityPage", jsonNode, null, string.Empty);
            }

            if (GUILayout.Button("Native Quit"))
            {
                JSONNode jsonNode = new JSONObject()
                {
                    ["type"] = "close"
                };
                ThirdPartyDispatch.Handle(CommandType.NativeWebviewHandle, jsonNode, null);
            }

            if (GUILayout.Button("Hide"))
            {
                PageManager.Instance.PageUIHide();
            }
            
            if (GUILayout.Button("Show"))
            {
                PageManager.Instance.PageUIShow();
            }

            if (GUILayout.Button("Stylebook"))
            {
                TestGotoStylebook();
            }
            GUI.DragWindow(screenRect);
        }


        private static void TestGotoRankingInfo()
        {
            JSONObject param = new JSONObject()
            {
                ["url"] = "RankingInfo",
                ["type"] = 1,
                ["account_id"] = "100005115"
            };
            ThirdPartyH5Dispatch.Handle("gotoUnityPage",param,null,string.Empty);
        }

        private static void TestGotoStylebook()
        {
            JSONArray account_id_list = new JSONArray();
            account_id_list.Add(100005014);
            account_id_list.Add(100005326);
            JSONObject param = new JSONObject
            {
                ["url"] = "Stylebook",
                ["account_id_list"] = account_id_list,
                ["stylebook_id"] = 4
            };
            ThirdPartyH5Dispatch.Handle("gotoUnityPage",param,null,string.Empty);
        }

    }
}
#endif