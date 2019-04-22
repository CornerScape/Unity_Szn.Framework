using SznFramework.AdMob;
using UnityEngine;

namespace SznFramework.AdMob.Test
{
    public class AdMobTest : MonoBehaviour
    {
        private bool bannerEnable;
        private bool interstitialEnable;
        private bool rewardEnable;

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
                bannerEnable = false;
                AdMobManager.BannerAd.Init(InResult => { bannerEnable = InResult; });
            }

            if (GUILayout.Button("Refresh Banner Ad"))
            {
                bannerEnable = false;
                AdMobManager.BannerAd.Refresh(InResult => { bannerEnable = InResult; });
            }

            GUI.enabled = bannerEnable;
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
                bannerEnable = false;
                AdMobManager.BannerAd.Close();
            }
            GUI.enabled = true;


            if (GUILayout.Button("Init Interstitial Ad"))
            {
                interstitialEnable = false;
                AdMobManager.InterstitialAd.Init(InResult =>
                {
                    interstitialEnable = InResult;
                });
            }

            GUI.enabled = interstitialEnable;
            if (GUILayout.Button("Show Interstitial Ad"))
            {
                interstitialEnable = false;
                if (AdMobManager.InterstitialAd.IsReady)
                {
                    AdMobManager.InterstitialAd.Show(() =>
                        AdMobManager.InterstitialAd.Init(InResult =>
                        {
                            interstitialEnable = InResult;
                        }));
                }
            }
            GUI.enabled = true;

            if (GUILayout.Button("Init Reward Ad"))
            {
                AdMobManager.RewardAd.Init(InResult =>
                {
                    rewardEnable = InResult;
                });
            }

            GUI.enabled = rewardEnable;
            if (GUILayout.Button("Show Reward Ad"))
            {
                rewardEnable = false;
                if (AdMobManager.RewardAd.IsReady)
                {
                    AdMobManager.RewardAd.Show(InShowResult =>
                    {
                        Debug.LogError("Show Reward Result = " + InShowResult);
                        AdMobManager.RewardAd.Init(InInitResult => { rewardEnable = InInitResult; });
                    });
                }
            }
            GUI.enabled = true;
            GUILayout.EndArea();
        }
    }
}