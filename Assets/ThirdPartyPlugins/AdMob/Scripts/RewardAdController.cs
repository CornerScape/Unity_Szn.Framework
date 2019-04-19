using System;
using GoogleMobileAds.Api;
using SznFramework.AdMob;
using UnityEngine;

public class RewardAdController
{
    public RewardAdController(string InId)
    {
        
    }
//    private string rewardAdId;
//    private RewardBasedVideoAd rewardBasedVideo;
//    private int retryTime;
//    private Action<bool> rewardAdCallback;

//    public RewardAdController(string InRewardAdId)
//    {
//        rewardAdId = InRewardAdId;

//        retryTime = 0;

//        // Get singleton reward based video ad reference.
//        rewardBasedVideo = RewardBasedVideoAd.Instance;

//        // Called when an ad request has successfully loaded.
//        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
//        // Called when an ad request failed to load.
//        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
//        // Called when an ad is shown.
//        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
//        // Called when the ad starts to play.
//        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
//        // Called when the user should be rewarded for watching a video.
//        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
//        // Called when the ad is closed.
//        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
//        // Called when the ad click caused the user to leave the application.
//        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

//        RequestRewardBasedVideo();
//    }

//    private void RequestRewardBasedVideo()
//    {
//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load the rewarded video ad with the request.
//        rewardBasedVideo.LoadAd(request, rewardAdId);
//    }

//    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
//    {
//        retryTime = 0;
//        Debug.Log("[Reward Ad]    HandleRewardBasedVideoLoaded event received");
//    }

//    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//    {
//        if (retryTime < AdMobManager.RETRY_TIME)
//        {
//            ++retryTime;
//            RequestRewardBasedVideo();
//        }
//        Debug.Log("[Reward Ad]    HandleRewardBasedVideoFailedToLoad event received with message: "
//                             + args.Message);
//    }

//    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
//    {
//        Debug.Log("[Reward Ad]    HandleRewardBasedVideoOpened event received");
//    }

//    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
//    {
//        Debug.Log("[Reward Ad]    HandleRewardBasedVideoStarted event received");
//    }

//    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
//    {
//        Debug.Log("[Reward Ad]    HandleRewardBasedVideoClosed event received");

//        rewardAdCallback.Invoke(false);
//    }

//    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
//    {
//        RequestRewardBasedVideo();

//        string type = args.Type;
//        double amount = args.Amount;
//        Debug.Log("[Reward Ad]    HandleRewardBasedVideoRewarded event received for "
//                        + amount.ToString() + " " + type);

//        rewardAdCallback.Invoke(true);
//    }

//    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
//    {
//        Debug.Log("[Reward Ad]    HandleRewardBasedVideoLeftApplication event received");
//    }

//    public void Show(Action<bool> InRewardAdCallback)
//    {
//        rewardAdCallback = InRewardAdCallback;

//#if UNITY_EDITOR
//        rewardAdCallback.Invoke(true);
//#else
//        if (rewardBasedVideo.IsLoaded()) rewardBasedVideo.Show();
//        else
//        {
//            retryTime = 0;
//            RequestRewardBasedVideo();
//            rewardAdCallback.Invoke(false);
//        }
//#endif
//    }
}