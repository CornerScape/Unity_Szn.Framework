//#if UNITY_EDITOR
//#define ADS_EDITOR
//#endif

using System;
using UnityEngine;

#if ADS_ADMOB
using SznFramework.AdMob;
#endif

namespace SznFramework.Ads
{
    public class AdManager : MonoBehaviour
    {
#if ADS
#if AD_DEBUG
#if UNITY_ANDROID
        public const string AD_APP_ID = "ca-app-pub-3940256099942544~3347511713";
        public const string AD_BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
        public const string AD_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
        public const string AD_REWARD_ID = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
        public const string AD_APP_ID = "ca-app-pub-3940256099942544~1458002511";
        public const string AD_BANNER_ID = "ca-app-pub-3940256099942544/2934735716";
        public const string AD_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/4411468910";
        public const string AD_REWARD_ID = "ca-app-pub-3940256099942544/1712485313";
#endif

#elif AD_RELEASE
#if UNITY_ANDROID
        public const string AD_APP_ID = "";
        public const string AD_BANNER_ID = "";
        public const string AD_INTERSTITIAL_ID = "";
        public const string AD_REWARD_ID = "";
#elif UNITY_IOS
        public const string AD_APP_ID = "";
        public const string AD_BANNER_ID = "";
        public const string AD_INTERSTITIAL_ID = "";
        public const string AD_REWARD_ID = "";
#endif
#endif
#endif

        private static AdManager instance;

        public static AdManager Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = FindObjectOfType<AdManager>();
                    if (null == instance)
                    {
                        GameObject adGameObj = new GameObject("AdManager");
                        instance = adGameObj.AddComponent<AdManager>();
                    }
                }

                return instance;
            }
        }

        private Action<bool> bannerLoadCallback, interstitialLoadCallback, rewardLoadCallback;

        public void RegisterBannerLoadCallback(Action<bool> InLoadCallback)
        {
            if (null != InLoadCallback)
                bannerLoadCallback += InLoadCallback;
        }

        public void UnRegisterBannerLoadCallback(Action<bool> InLoadCallback)
        {
            if (null != bannerLoadCallback && null != InLoadCallback)
                // ReSharper disable once DelegateSubtraction
                bannerLoadCallback -= InLoadCallback;
        }

        private void InvokeBannerLoadCallback(bool InLoadResult)
        {
            if (null != bannerLoadCallback) bannerLoadCallback.Invoke(InLoadResult);
        }

        public void RegisterInterstitialLoadCallback(Action<bool> InLoadCallback)
        {
            if (null != interstitialLoadCallback)
                interstitialLoadCallback += InLoadCallback;
        }

        public void UnRegisterInterstitialLoadCallback(Action<bool> InLoadCallback)
        {
            if (null != interstitialLoadCallback && null != InLoadCallback)
                // ReSharper disable once DelegateSubtraction
                interstitialLoadCallback -= InLoadCallback;
        }

        private void InvokeInterstitialLoadCallback(bool InLoadResult)
        {
            if (null != interstitialLoadCallback) interstitialLoadCallback.Invoke(InLoadResult);
        }

        public void RegisterRewardLoadCallback(Action<bool> InLoadCallback)
        {
            if (null != InLoadCallback)
                rewardLoadCallback += InLoadCallback;
        }

        public void UnRegisterRewardLoadCallback(Action<bool> InLoadCallback)
        {
            if (null != rewardLoadCallback && null != InLoadCallback)
                // ReSharper disable once DelegateSubtraction
                rewardLoadCallback -= InLoadCallback;
        }

        private void InvokeRewardLoadCallback(bool InLoadResult)
        {
            if (null != rewardLoadCallback) rewardLoadCallback.Invoke(InLoadResult);
        }

        private void Awake()
        {
            if (null != instance && instance != this)
            {
                Debug.LogError("The class already has an instance. ");
                DestroyImmediate(this);
            }
        }

        public void InitAd(Action<bool> InLoadBannerCallback = null,
            Action<bool> InLoadInterstitialCallback = null,
            Action<bool> InLoadRewardCallback = null)
        {
            if (null != InLoadBannerCallback) RegisterBannerLoadCallback(InLoadBannerCallback);
            if (null != InLoadInterstitialCallback) RegisterBannerLoadCallback(InLoadInterstitialCallback);
            if (null != InLoadRewardCallback) RegisterBannerLoadCallback(InLoadRewardCallback);

#if ADS_EDITOR
        Debug.LogError("Can not init ads on editor.");
#else
#if ADS_ADMOB
            AdMobController.Init(AD_APP_ID, AD_BANNER_ID, AD_INTERSTITIAL_ID, AD_REWARD_ID);
            AdMobController.BannerAd.RegisterAdLoadCallback(InvokeBannerLoadCallback);
            AdMobController.InterstitialAd.RegisterAdLoadCallback(InvokeInterstitialLoadCallback);
            AdMobController.RewardAd.RegisterAdLoadCallback(InvokeRewardLoadCallback);
#endif
#endif
        }

        public bool IsBannerAdReady()
        {
#if ADS_EDITOR
        Debug.LogWarning("Always true in the editor.");
        return true;
#else
#if ADS_ADMOB
            return AdMobController.BannerAd != null && AdMobController.BannerAd.IsLoaded &&
                   !AdMobController.BannerAd.IsPlaying;
#else
            return false;
#endif
#endif
        }

        public bool IsBannerAdPlaying()
        {
#if ADS_EDITOR
        Debug.LogWarning("Always true in the editor.");
        return true;
#else
#if ADS_ADMOB
            return AdMobController.BannerAd != null && AdMobController.BannerAd.IsPlaying;
#else
            return false;
#endif
#endif
        }

        public void ShowBannerAd()
        {
            if (IsBannerAdReady())
            {
#if ADS_ADMOB
                AdMobController.BannerAd.Show();
#endif
            }
        }

        public void HideBannerAd()
        {
            if (IsBannerAdPlaying())
            {
#if ADS_ADMOB
                AdMobController.BannerAd.Hide();
#endif
            }
        }

        public void RefreshBannerAd()
        {
#if ADS_ADMOB
            if (null != AdMobController.BannerAd) AdMobController.BannerAd.Request();
#endif
        }

        public bool IsInterstitialAdReady()
        {
#if ADS_EDITOR
        Debug.LogWarning("Always true in the editor.");
        return true;
#else
#if ADS_ADMOB
            return AdMobController.InterstitialAd != null && AdMobController.InterstitialAd.IsReady;
#else
            return false;
#endif
#endif
        }

        public void ShowInterstitialAd()
        {
            if (IsInterstitialAdReady())
            {
#if ADS_ADMOB
                AdMobController.InterstitialAd.Show();
#endif
            }
        }

        public bool IsRewardAdReady()
        {
#if ADS_EDITOR
        Debug.LogWarning("Always true in the editor.");
        return true;
#else
#if ADS_ADMOB
            return AdMobController.RewardAd != null && AdMobController.RewardAd.IsReady;
#else
            return false;
#endif
#endif
        }

        public void ShowRewardAd(Action<bool> InCallback)
        {
            if (IsBannerAdReady())
            {
#if ADS_ADMOB
                AdMobController.RewardAd.Show(InCallback);
#endif
            }
        }

        private void OnDestroy()
        {
#if ADS_EDITOR
#else
#if ADS_ADMOB
            if (null != AdMobController.BannerAd) AdMobController.BannerAd.Destroy();
            if (null != AdMobController.InterstitialAd) AdMobController.InterstitialAd.Destroy();
#endif
#endif
        }
    }
}