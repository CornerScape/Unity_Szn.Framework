using GoogleMobileAds.Api;
using System;
using UnityEngine;

namespace SznFramework.AdMob
{
    public class AdMobManager
    {
        private static AdMobManager instance;
        public static AdMobManager Instance { get { return instance ?? (instance = new AdMobManager()); } }

        public static int RETRY_TIME = 5;

        private string bannerAdId;
        private string interstitialAdId;
        private string rewardAdId;

        public void Init(string InAppId, string InBannerAdId = null, string InInterstitialAdId = null, string InRewardAdId = null)
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(InAppId);

            bannerAdId = InBannerAdId;
            interstitialAdId = InInterstitialAdId;
            rewardAdId = InRewardAdId;

            interstitialAd = new InterstitialAdController(interstitialAdId);

            rewardAd = new RewardAdController(rewardAdId);
        }

        #region Banner
        private BannerAdController bannerAd;
        public void ShowBannerAd()
        {
            if (bannerAd == null) bannerAd = new BannerAdController(bannerAdId, AdSize.Banner, AdPosition.Bottom);
            else if (!bannerAd.IsAdLoaded || !bannerAd.IsAdShow) bannerAd.RequestBanner();
        }

        public void CloseBannerAd()
        {
            if (bannerAd != null) bannerAd.Close();
        }
        #endregion

        #region Interstitial
        private InterstitialAdController interstitialAd;
        public void ShowInterstitialAd()
        {
            interstitialAd.Show();
        }

        public void CloseInterstitialAd()
        {
            interstitialAd.Close();
        }
        #endregion

        #region Reward
        private RewardAdController rewardAd;
        public void ShowRewardAd(Action<bool> InRewardAdCallback)
        {
            rewardAd.Show(InRewardAdCallback);
        }
        #endregion
    }

    public class BannerAdController
    {
        private readonly string bannerAdId;
        private BannerView bannerView;
        private readonly AdSize adSize;
        private readonly AdPosition adPosition;
        private int failedTime;
        public bool IsAdLoaded { get; private set; }
        public bool IsAdShow { get; private set; }

        public BannerAdController(string InBannerAdId, AdSize InBannerSize, AdPosition InAdPosition)
        {
            bannerAdId = InBannerAdId;

            failedTime = 0;
            if (bannerView != null && IsAdLoaded) bannerView.Destroy();
            adSize = InBannerSize;
            adPosition = InAdPosition;
            RequestBanner();
        }

        public void RequestBanner()
        {
            // Create a 320x50 banner at the top of the screen.
            bannerView = new BannerView(bannerAdId, adSize, adPosition);

            // Called when an ad request has successfully loaded.
            bannerView.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            bannerView.OnAdOpening += HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerView.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();

            // Load the banner with the request.
            bannerView.LoadAd(request);
        }

        public void HandleOnAdLoaded(object sender, EventArgs args)
        {
            IsAdLoaded = true;
            failedTime = 0;
            Debug.Log("[Banner Ad]    HandleAdLoaded event received");
        }

        public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            IsAdLoaded = false;
            if (failedTime < AdMobManager.RETRY_TIME)
            {
                ++failedTime;
                RequestBanner();
            }

            Debug.Log("[Banner Ad]    HandleFailedToReceiveAd event received with message: "
                                + args.Message);
        }

        public void HandleOnAdOpened(object sender, EventArgs args)
        {
            IsAdShow = true;
            Debug.Log("[Banner Ad]    HandleAdOpened event received");
        }

        public void HandleOnAdClosed(object sender, EventArgs args)
        {
            IsAdShow = false;
            Debug.Log("[Banner Ad]    HandleAdClosed event received");
        }

        public void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            Debug.Log("[Banner Ad]    HandleAdLeavingApplication event received");
        }

        public void Close()
        {
            if (IsAdLoaded)
            {
                bannerView.Destroy();
                IsAdLoaded = false;
                IsAdShow = false;
                bannerView = null;
            }
        }
    }

    public class InterstitialAdController
    {
        private InterstitialAd interstitial;
        private int retryTime;

        public InterstitialAdController(string InBannerAdId)
        {
            retryTime = 0;
#if UNITY_ANDROID
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(InBannerAdId);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
#endif

            RequestInterstitial();
        }

        private void RequestInterstitial()
        {
#if UNITY_IPHONE
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(AdConfig.INTERSTITIAL_AD_ID);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
#endif
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            interstitial.LoadAd(request);
        }

        public void HandleOnAdLoaded(object sender, EventArgs args)
        {
            retryTime = 0;
            Debug.Log("[Interstitial Ad]    HandleAdLoaded event received");
        }

        public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            if (retryTime < AdMobManager.RETRY_TIME)
            {
                ++retryTime;
                RequestInterstitial();
            }
            Debug.Log("[Interstitial Ad]    HandleFailedToReceiveAd event received with message: "
                                + args.Message);
        }

        public void HandleOnAdOpened(object sender, EventArgs args)
        {
            Debug.Log("[Interstitial Ad]    HandleAdOpened event received");
        }

        public void HandleOnAdClosed(object sender, EventArgs args)
        {
            RequestInterstitial();
            Debug.Log("[Interstitial Ad]    HandleAdClosed event received");
        }

        public void HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            Debug.Log("[Interstitial Ad]    HandleAdLeavingApplication event received");
        }

        public void Show()
        {
            if (interstitial.IsLoaded())
                interstitial.Show();
            else
            {
                retryTime = 0;
                RequestInterstitial();
            }
        }

        public void Close()
        {
            interstitial.Destroy();
        }
    }

    public class RewardAdController
    {
        private string rewardAdId;
        private RewardBasedVideoAd rewardBasedVideo;
        private int retryTime;
        private Action<bool> rewardAdCallback;

        public RewardAdController(string InRewardAdId)
        {
            rewardAdId = InRewardAdId;

            retryTime = 0;

            // Get singleton reward based video ad reference.
            rewardBasedVideo = RewardBasedVideoAd.Instance;

            // Called when an ad request has successfully loaded.
            rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
            // Called when an ad request failed to load.
            rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            // Called when an ad is shown.
            rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
            // Called when the ad starts to play.
            rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
            // Called when the user should be rewarded for watching a video.
            rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            // Called when the ad is closed.
            rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            // Called when the ad click caused the user to leave the application.
            rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

            RequestRewardBasedVideo();
        }

        private void RequestRewardBasedVideo()
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            rewardBasedVideo.LoadAd(request, rewardAdId);
        }

        public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
        {
            retryTime = 0;
            Debug.Log("[Reward Ad]    HandleRewardBasedVideoLoaded event received");
        }

        public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            if (retryTime < AdMobManager.RETRY_TIME)
            {
                ++retryTime;
                RequestRewardBasedVideo();
            }
            Debug.Log("[Reward Ad]    HandleRewardBasedVideoFailedToLoad event received with message: "
                                 + args.Message);
        }

        public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
        {
            Debug.Log("[Reward Ad]    HandleRewardBasedVideoOpened event received");
        }

        public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
        {
            Debug.Log("[Reward Ad]    HandleRewardBasedVideoStarted event received");
        }

        public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
        {
            Debug.Log("[Reward Ad]    HandleRewardBasedVideoClosed event received");

            rewardAdCallback.Invoke(false);
        }

        public void HandleRewardBasedVideoRewarded(object sender, Reward args)
        {
            RequestRewardBasedVideo();

            string type = args.Type;
            double amount = args.Amount;
            Debug.Log("[Reward Ad]    HandleRewardBasedVideoRewarded event received for "
                            + amount.ToString() + " " + type);

            rewardAdCallback.Invoke(true);
        }

        public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
        {
            Debug.Log("[Reward Ad]    HandleRewardBasedVideoLeftApplication event received");
        }

        public void Show(Action<bool> InRewardAdCallback)
        {
            rewardAdCallback = InRewardAdCallback;

#if UNITY_EDITOR
            rewardAdCallback.Invoke(true);
#else
        if (rewardBasedVideo.IsLoaded()) rewardBasedVideo.Show();
        else
        {
            retryTime = 0;
            RequestRewardBasedVideo();
            rewardAdCallback.Invoke(false);
        }
#endif
        }
    }
}