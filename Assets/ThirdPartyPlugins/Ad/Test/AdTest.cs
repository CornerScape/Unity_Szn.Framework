using SznFramework.Ads;
using UnityEngine;

namespace SznFramework.AdMob.Test
{
    public class AdTest : MonoBehaviour
    {
        private bool bannerEnable;
        private bool interstitialEnable;
        private bool rewardEnable;

        void Start()
        {
            AdManager.Instance.InitAd(InBannerLoadResult => { bannerEnable = InBannerLoadResult; },
                InInterstitialResult => { interstitialEnable = InInterstitialResult; },
                InRewardResult => { rewardEnable = InRewardResult; });
        }

        // Use this for initialization
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width * .5f, Screen.height * .5f, Screen.width * .5f,
                Screen.height * .5f));
            GUI.skin.button.fontSize = 32;
            if (GUILayout.Button("Refresh Banner Ad"))
            {
                bannerEnable = false;
                AdManager.Instance.RefreshBannerAd();
            }

            GUI.enabled = bannerEnable;
            if (GUILayout.Button("Show Banner Ad"))
            {
                AdManager.Instance.ShowBannerAd();
            }

            if (GUILayout.Button("Hide Banner Ad"))
            {
                AdManager.Instance.HideBannerAd();
            }

            GUI.enabled = true;

            GUI.enabled = interstitialEnable;
            if (GUILayout.Button("Show Interstitial Ad"))
            {
                interstitialEnable = false;
                AdManager.Instance.ShowInterstitialAd();
            }

            GUI.enabled = true;


            GUI.enabled = rewardEnable;
            if (GUILayout.Button("Show Reward Ad"))
            {
                rewardEnable = false;
                AdManager.Instance.ShowRewardAd(InCanReward =>
                {
                    Debug.LogError("Get reward result = " + InCanReward);
                });
            }

            GUI.enabled = true;
            GUILayout.EndArea();
        }
    }
}