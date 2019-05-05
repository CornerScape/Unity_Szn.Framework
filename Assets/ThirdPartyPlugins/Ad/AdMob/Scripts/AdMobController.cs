#if ADS_ADMOB
using GoogleMobileAds.Api;
using UnityEngine;

namespace SznFramework.AdMob
{
    public static class AdMobController
    {
        public static BannerAdController BannerAd { get; private set; }

        public static InterstitialAdController InterstitialAd { get; private set; }

        public static RewardAdController RewardAd { get; private set; }

        /// <summary>
        /// 初始化广告
        /// </summary>
        /// <param name="InAppId">AdMob App Id，不能为空</param>
        /// <param name="InBannerId">AdMob Banner Id，如果不需要banner ad传null，默认Banner size 为 AdSize.Banner, Banner Position 为 AdPosition.Bottom.</param>
        /// <param name="InInterstitialId">AdMob Banner Id，如果不需要banner ad传null</param>
        /// <param name="InRewardId">AdMob Banner Id，如果不需要banner ad传null</param>
        public static void Init(string InAppId, string InBannerId, string InInterstitialId, string InRewardId)
        {
            if (string.IsNullOrEmpty(InAppId))
            {
                Debug.LogError(
                    "The AdMobTest id is empty. Please set the corresponding platform ID in the 'AdMobConfig' script. ");
            }
            else
            {
                MobileAds.SetiOSAppPauseOnBackground(true);

                // Initialize the Google Mobile Ads SDK.
                MobileAds.Initialize(InAppId);

                if (!string.IsNullOrEmpty(InBannerId))
                {
                    BannerAd = new BannerAdController(InBannerId, AdSize.Banner, AdPosition.Bottom);
                }

                if (!string.IsNullOrEmpty(InInterstitialId))
                {
                    InterstitialAd = new InterstitialAdController(InInterstitialId);
                }

                if (!string.IsNullOrEmpty(InRewardId))
                {
                    RewardAd = new RewardAdController(InRewardId);
                }
            }
        }

        /// <summary>
        /// 初始化广告
        /// </summary>
        /// <param name="InAppId">AdMob App Id，不能为空</param>
        /// <param name="InBannerId">AdMob Banner Id，如果不需要banner ad传null</param>
        /// <param name="InInterstitialId">AdMob Banner Id，如果不需要banner ad传null</param>
        /// <param name="InRewardId">AdMob Banner Id，如果不需要banner ad传null</param>
        /// <param name="InAdSize">Banner Ad Size.</param>
        /// <param name="InAdPosition">Banner Ad Position.</param>
        public static void Init(string InAppId, string InBannerId, string InInterstitialId, string InRewardId,
            AdSize InAdSize, AdPosition InAdPosition)
        {
            if (string.IsNullOrEmpty(InAppId))
            {
                Debug.LogError(
                    "The AdMobTest id is empty. Please set the corresponding platform ID in the 'AdMobConfig' script. ");
            }
            else
            {
                MobileAds.SetiOSAppPauseOnBackground(true);

                // Initialize the Google Mobile Ads SDK.
                MobileAds.Initialize(InAppId);

                if (!string.IsNullOrEmpty(InBannerId))
                {
                    BannerAd = new BannerAdController(InBannerId, InAdSize, InAdPosition);
                }

                if (!string.IsNullOrEmpty(InInterstitialId))
                {
                    InterstitialAd = new InterstitialAdController(InInterstitialId);
                }

                if (!string.IsNullOrEmpty(InRewardId))
                {
                    RewardAd = new RewardAdController(InRewardId);
                }
            }
        }
    }
}
#endif