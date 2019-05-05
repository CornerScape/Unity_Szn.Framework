#if ADS_ADMOB
using GoogleMobileAds.Api;
using System;
using UnityEngine;

namespace SznFramework.AdMob
{
    public class InterstitialAdController
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly string adId;
        private InterstitialAd interstitialAd;
        private Action<bool> loadCallback;

        public void RegisterAdLoadCallback(Action<bool> InCallback)
        {
            if (null != InCallback)
                loadCallback += InCallback;
        }

        public void UnRegisterAdLoadCallback(Action<bool> InCallback)
        {
            if (null != loadCallback && null != InCallback)
                // ReSharper disable once DelegateSubtraction
                loadCallback -= InCallback;
        }
        
        public bool IsReady
        {
            get { return null != interstitialAd && interstitialAd.IsLoaded(); }
        }

        public InterstitialAdController(string InId, Action<bool> InLoadCallback = null)
        {
            adId = InId;

            interstitialAd = new InterstitialAd(adId);

            // Called when an ad request has successfully loaded.
            interstitialAd.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is shown.
            interstitialAd.OnAdOpening += HandleOnAdOpened;
            // Called when the ad is closed.
            interstitialAd.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
            
            if(null != InLoadCallback) RegisterAdLoadCallback(InLoadCallback);
        }

        public void Request()
        {
#if UNITY_IOS
            //Initialize an InterstitialAd.
            interstitialAd = new InterstitialAd(adId);

            //Called when an ad request has successfully loaded.
            interstitialAd.OnAdLoaded += HandleOnAdLoaded;
            //Called when an ad request failed to load.
            interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            //Called when an ad is shown.
            interstitialAd.OnAdOpening += HandleOnAdOpened;
            //Called when the ad is closed.
            interstitialAd.OnAdClosed += HandleOnAdClosed;
            //Called when the ad click caused the user to leave the application.
            interstitialAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
#endif

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitialAd.LoadAd(request);
        }

        public void HandleOnAdLoaded(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Interstitial Ad]    HandleAdLoaded event received");
            if (null != loadCallback)
                loadCallback.Invoke(true);
        }

        public void HandleOnAdFailedToLoad(object InSender, AdFailedToLoadEventArgs InArgs)
        {
            Debug.Log("[Interstitial Ad]    HandleFailedToReceiveAd event received with message: "
                      + InArgs.Message);
            if (null != loadCallback)
                loadCallback.Invoke(false);
        }

        public void HandleOnAdOpened(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Interstitial Ad]    HandleAdOpened event received");
        }

        public void HandleOnAdClosed(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Interstitial Ad]    HandleAdClosed event received");

            Request();
        }

        public void HandleOnAdLeavingApplication(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Interstitial Ad]    HandleAdLeavingApplication event received");
        }

        public void Show()
        {
            if (null == interstitialAd)
            {
                Debug.LogError("Interstitial has been destroyed.");
            }
            else
            {
                if (interstitialAd.IsLoaded())
                    interstitialAd.Show();
                else
                {
                    Debug.LogError("Interstitial Not Loaded.");
                    Request();
                }
            }
        }

        public void Destroy()
        {
            if (null != interstitialAd)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }
        }
    }
}
#endif