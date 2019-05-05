using System;
using SznFramework.Ads;
using SznFramework.UtilPackage;
using Together;
using UnityEngine;

namespace SznFramework.YoMob
{
    public class YoMobController : Singleton<YoMobController>
    {
        private Action<bool> rewardCallback;
        public void Init(bool InDebugMode, Action<bool> InLoadBannerCallback = null,
            Action<bool> InLoadInterstitialCallback = null,
            Action<bool> InLoadRewardCallback = null)
        {
            TGSDK.SetDebugModel(InDebugMode);
            TGSDK.SDKInitFinishedCallback = InMsg => { Debug.LogError("YoMob Init Result = " + InMsg); };
            TGSDK.Initialize(AdManager.AD_APP_ID);

            TGSDK.AdClickCallback += (InS, InS1) =>
            {
                Debug.LogError("AdClickCallback = " + InS + "<>" + InS1);
            };

            TGSDK.AdCloseCallback += (InS, InS1, InArg3) =>
            {
                Debug.LogError("AdCloseCallback = " + InS + "<>" + InS1 + "<>" + InArg3);
                if (null != rewardCallback) rewardCallback.Invoke(InArg3);
            };

            TGSDK.AdShowFailedCallback += (InS, InS1, InArg3) =>
            {
                Debug.LogError("AdShowFailedCallback = " + InS + "<>" + InS1 + "<>" + InArg3);
            };

            TGSDK.AdShowSuccessCallback = (InS, InS1) =>
            {
                Debug.LogError("AdShowSuccessCallback = " + InS + "<>" + InS1);
            };

            TGSDK.AwardVideoLoadedCallback = InS =>
            {
                Debug.LogError("AwardVideoLoadedCallback = " + InS + "<>");
            };

            TGSDK.InterstitialLoadedCallback = InS =>
            {
                Debug.LogError("InterstitialLoadedCallback = " + InS + "<>");
            };

            TGSDK.InterstitialVideoLoadedCallback = InS =>
            {
                Debug.LogError("InterstitialVideoLoadedCallback = " + InS + "<>");
            };
        }

        public bool IsInterstitialAdReady
        {
            get { return TGSDK.CouldShowAd(AdManager.AD_INTERSTITIAL_ID); }
        }

        public void ShowInterstitialAd()
        {
            TGSDK.ShowAd(AdManager.AD_INTERSTITIAL_ID);
        }

        public bool IsRewardAdReady
        {
            get { return TGSDK.CouldShowAd(AdManager.AD_REWARD_ID); }
        }

        public void ShowRewardAd(Action<bool> InRewardCallback)
        {
            rewardCallback = null;
            rewardCallback = InRewardCallback;
            TGSDK.ShowAd(AdManager.AD_REWARD_ID);
        }
    }

}

