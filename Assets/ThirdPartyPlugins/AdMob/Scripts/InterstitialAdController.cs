using System;
using GoogleMobileAds.Api;
using SznFramework.AdMob;
using UnityEngine;

public class InterstitialAdController
{
    public InterstitialAdController(string InId)
    {
        
    }
//    private InterstitialAd interstitial;
//    private int retryTime;

//    public InterstitialAdController(string InBannerAdId)
//    {
//        retryTime = 0;
//#if UNITY_ANDROID
//        // Initialize an InterstitialAd.
//        interstitial = new InterstitialAd(InBannerAdId);

//        // Called when an ad request has successfully loaded.
//        interstitial.OnAdLoaded += HandleOnAdLoaded;
//        // Called when an ad request failed to load.
//        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
//        // Called when an ad is shown.
//        interstitial.OnAdOpening += HandleOnAdOpened;
//        // Called when the ad is closed.
//        interstitial.OnAdClosed += HandleOnAdClosed;
//        // Called when the ad click caused the user to leave the application.
//        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
//#endif

//        RequestInterstitial();
//    }

//    private void RequestInterstitial()
//    {
//#if UNITY_IPHONE
//        // Initialize an InterstitialAd.
//        interstitial = new InterstitialAd(AdMobConfig.INTERSTITIAL_AD_ID);

//        // Called when an ad request has successfully loaded.
//        interstitial.OnAdLoaded += HandleOnAdLoaded;
//        // Called when an ad request failed to load.
//        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
//        // Called when an ad is shown.
//        interstitial.OnAdOpening += HandleOnAdOpened;
//        // Called when the ad is closed.
//        interstitial.OnAdClosed += HandleOnAdClosed;
//        // Called when the ad click caused the user to leave the application.
//        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
//#endif
//        // Create an empty ad request.
//        AdRequest request = new AdRequest.Builder().Build();
//        // Load the interstitial with the request.
//        interstitial.LoadAd(request);
//    }

//    public void HandleOnAdLoaded(object sender, EventArgs args)
//    {
//        retryTime = 0;
//        Debug.Log("[Interstitial Ad]    HandleAdLoaded event received");
//    }

//    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
//    {
//        if (retryTime < AdMobManager.RETRY_TIME)
//        {
//            ++retryTime;
//            RequestInterstitial();
//        }
//        Debug.Log("[Interstitial Ad]    HandleFailedToReceiveAd event received with message: "
//                            + args.Message);
//    }

//    public void HandleOnAdOpened(object sender, EventArgs args)
//    {
//        Debug.Log("[Interstitial Ad]    HandleAdOpened event received");
//    }

//    public void HandleOnAdClosed(object sender, EventArgs args)
//    {
//        RequestInterstitial();
//        Debug.Log("[Interstitial Ad]    HandleAdClosed event received");
//    }

//    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
//    {
//        Debug.Log("[Interstitial Ad]    HandleAdLeavingApplication event received");
//    }

//    public void Show()
//    {
//        if (interstitial.IsLoaded())
//            interstitial.Show();
//        else
//        {
//            retryTime = 0;
//            RequestInterstitial();
//        }
//    }

//    public void Close()
//    {
//        interstitial.Destroy();
//    }
}
