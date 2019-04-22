using GoogleMobileAds.Api;
using UnityEngine;

namespace SznFramework.AdMob
{
    public static class AdMobManager
    {
        public static BannerAdController BannerAd { get; private set; }

        public static InterstitialAdController InterstitialAd { get; private set; }

        public static RewardAdController RewardAd { get; private set; }

        public static void Init(AdSize InBannerAdSize = null, AdPosition InBannerAdPosition = AdPosition.Bottom)
        {
            if (string.IsNullOrEmpty(AdMobConfig.AD_MOB_APP_ID))
            {
                Debug.LogError("The AdMob id is empty. Please set the corresponding platform ID in the 'AdMobConfig' script. ");
            }
            else
            {
                MobileAds.SetiOSAppPauseOnBackground(true);

                // Initialize the Google Mobile Ads SDK.
                MobileAds.Initialize(AdMobConfig.AD_MOB_APP_ID);

                if (!string.IsNullOrEmpty(AdMobConfig.BANNER_AD_ID))
                {
                    if(null == InBannerAdSize) InBannerAdSize = AdSize.Banner;
                    BannerAd = new BannerAdController(AdMobConfig.BANNER_AD_ID, InBannerAdSize, InBannerAdPosition);
                }

                if (!string.IsNullOrEmpty(AdMobConfig.INTERSTITIAL_AD_ID))
                {
                    InterstitialAd = new InterstitialAdController(AdMobConfig.INTERSTITIAL_AD_ID);
                }

                if (!string.IsNullOrEmpty(AdMobConfig.REWARD_AD_ID))
                {
                    RewardAd = new RewardAdController(AdMobConfig.REWARD_AD_ID);
                }
            }
        }
    }
}