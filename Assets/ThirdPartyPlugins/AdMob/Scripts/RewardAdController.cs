using GoogleMobileAds.Api;
using System;
using System.Globalization;
using UnityEngine;

namespace SznFramework.AdMob
{
    public class RewardAdController
    {
        private readonly string adId;
        private readonly RewardBasedVideoAd rewardVideoAd;
        private Action<bool> initCallback;
        private Action<bool> showCallback;

        public bool IsReady
        {
            get { return null != rewardVideoAd && rewardVideoAd.IsLoaded(); }
        }
        
        public RewardAdController(string InId)
        {
            adId = InId;

            // Get singleton reward based video ad reference.
            rewardVideoAd = RewardBasedVideoAd.Instance;

            // Called when an ad request has successfully loaded.
            rewardVideoAd.OnAdLoaded += HandleRewardVideoAdLoaded;
            // Called when an ad request failed to load.
            rewardVideoAd.OnAdFailedToLoad += HandleRewardVideoAdFailedToLoad;
            // Called when an ad is shown.
            rewardVideoAd.OnAdOpening += HandleRewardVideoAdOpened;
            // Called when the ad starts to play.
            rewardVideoAd.OnAdStarted += HandleRewardVideoAdStarted;
            // Called when the user should be rewarded for watching a video.
            rewardVideoAd.OnAdRewarded += HandleRewardVideoAdRewarded;
            // Called when the ad is closed.
            rewardVideoAd.OnAdClosed += HandleRewardVideoAdClosed;
            // Called when the ad click caused the user to leave the application.
            rewardVideoAd.OnAdLeavingApplication += HandleRewardVideoAdLeftApplication;
        }

        public void Init(Action<bool> InInitCallback)
        {
            initCallback = InInitCallback;

            RequestRewardBasedVideo();
        }

        private void RequestRewardBasedVideo()
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            rewardVideoAd.LoadAd(request, adId);
        }

        public void HandleRewardVideoAdLoaded(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Reward Ad]    HandleRewardVideoAdLoaded event received");
            if (null != initCallback) initCallback.Invoke(true);
        }

        public void HandleRewardVideoAdFailedToLoad(object InSender, AdFailedToLoadEventArgs InArgs)
        {
            Debug.Log("[Reward Ad]    HandleRewardVideoAdFailedToLoad event received with message: "
                      + InArgs.Message);
            if (null != initCallback) initCallback.Invoke(false);
        }

        public void HandleRewardVideoAdOpened(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Reward Ad]    HandleRewardVideoAdOpened event received");
        }

        public void HandleRewardVideoAdStarted(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Reward Ad]    HandleRewardVideoAdStarted event received");
        }

        public void HandleRewardVideoAdClosed(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Reward Ad]    HandleRewardVideoAdClosed event received");

            if (null != showCallback)
            {
                showCallback.Invoke(false);
                showCallback = null;
            }
        }

        public void HandleRewardVideoAdRewarded(object InSender, Reward InArgs)
        {
            string type = InArgs.Type;
            double amount = InArgs.Amount;
            Debug.Log("[Reward Ad]    HandleRewardVideoAdRewarded event received for "
                      + amount.ToString(CultureInfo.InvariantCulture) + " " + type);

            if (null != showCallback)
            {
                showCallback.Invoke(true);
                showCallback = null;
            }
        }

        public void HandleRewardVideoAdLeftApplication(object InSender, EventArgs InArgs)
        {
            Debug.Log("[Reward Ad]    HandleRewardVideoAdLeftApplication event received");
        }

        public void Show(Action<bool> InShowAdCallback)
        {
            showCallback = InShowAdCallback;

            if (null == rewardVideoAd)
            {
                Debug.LogError("Reward Ad Not Init.");
                showCallback.Invoke(false);
            }
            else
            {
                if (rewardVideoAd.IsLoaded()) rewardVideoAd.Show();
                else
                {
                    Debug.LogError("Reward Ad Not Loaded.");
                    showCallback.Invoke(false);
                }
            }
        }
    }
}