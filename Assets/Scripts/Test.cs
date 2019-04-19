using SznFramework.AdMob;
using SznFramework.UtilPackage;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        AdMobManager.Init();
    }

    // Use this for initialization
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width * .5f, Screen.height * .5f, Screen.width * .5f, Screen.height * .5f));
        GUI.skin.button.fontSize = 32;
        if (GUILayout.Button("Load Banner Ad"))
        {
            AdMobManager.BannerAd.Init();
        }

        if (GUILayout.Button("Refresh Banner Ad"))
        {
            AdMobManager.BannerAd.Refresh();
        }

        if (GUILayout.Button("Show Banner Ad"))
        {
            AdMobManager.BannerAd.Show();
        }

        if (GUILayout.Button("Hide Banner Ad"))
        {
            AdMobManager.BannerAd.Hide();
        }

        if (GUILayout.Button("Close Banner Ad"))
        {
            AdMobManager.BannerAd.Close();
        }

        if (GUILayout.Button("Trigger"))
        {
            TimerTools.Instance.RegisterTrigger(6,
                () =>
                {
                    Debug.LogError("Finished Trigger");
                },
                this);
        }

        if (GUILayout.Button("Destroy"))
        {
            DestroyImmediate(this);
        }
        GUILayout.EndArea();
    }
}
