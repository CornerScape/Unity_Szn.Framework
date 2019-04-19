using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class BannerAdController
{
    private readonly string bannerAdId;
    private BannerView bannerView;
    private readonly AdSize adSize;
    private readonly AdPosition adPosition;

    private bool isLoading;
    private bool isClosed;
    public bool IsLoaded { get; private set; }
    public bool IsPlaying { get; private set; }
    private Action<bool> initCallback;

    public BannerAdController(string InBannerAdId, AdSize InBannerSize, AdPosition InAdPosition)
    {
        isLoading = false;
        IsLoaded = false;
        IsPlaying = false;
        isClosed = false;

        bannerAdId = InBannerAdId;
        adSize = InBannerSize;
        adPosition = InAdPosition;
    }

    public void Init(Action<bool> InCallback = null)
    {
        if (isLoading)
        {
            Debug.LogError("Banner Ad is loading, Please try again later.");
        }
        else if (IsLoaded)
        {
            Debug.LogError("Banner Ad already loaded, If you want to refresh, please use the 'Refresh' function.");
        }
        else
        {
            initCallback = InCallback;
            RequestBanner();
        }
    }

    public void Refresh(Action<bool> InCallback = null)
    {
        if (isLoading)
        {
            Debug.LogError("Banner Ad is loading, Please try again later.");
        }
        else
        {
            initCallback = InCallback;

            if (bannerView != null)
            {
                bannerView.Destroy();
                bannerView = null;
            }
            RequestBanner();
        }
    }

    public void RequestBanner()
    {
        isLoading = true;
        IsLoaded = false;
        isClosed = false;
        IsPlaying = false;

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

    public void HandleOnAdLoaded(object InSender, EventArgs InArgs)
    {
#if AD_DEBUG
        Debug.Log("[Banner Ad]    HandleAdLoaded event received");
#endif
        if (isClosed)
        {
            Close();
            if (initCallback != null) initCallback.Invoke(false);
        }
        else
        {
            isLoading = false;
            IsLoaded = true;
            bannerView.Hide();
            if (initCallback != null) initCallback.Invoke(true);
        }
    }

    public void HandleOnAdFailedToLoad(object InSender, AdFailedToLoadEventArgs InArgs)
    {
#if AD_DEBUG
        Debug.Log("[Banner Ad]    HandleFailedToReceiveAd event received with message: "
                            + InArgs.Message);
#endif
        isLoading = false;
        IsLoaded = false;
        if (initCallback != null) initCallback.Invoke(false);
    }

    public void HandleOnAdOpened(object InSender, EventArgs InArgs)
    {
#if AD_DEBUG
        Debug.Log("[Banner Ad]    HandleAdOpened event received");
#endif
    }

    public void HandleOnAdClosed(object InSender, EventArgs InArgs)
    {
#if AD_DEBUG
        Debug.Log("[Banner Ad]    HandleAdClosed event received");
#endif
    }

    public void HandleOnAdLeavingApplication(object InSender, EventArgs InArgs)
    {
#if AD_DEBUG
        Debug.Log("[Banner Ad]    HandleAdLeavingApplication event received");
#endif
    }

    public void Show()
    {
        if (IsLoaded)
        {
            if (IsPlaying) Debug.LogError("Banner Ad is playing.");
            else
            {
                IsPlaying = true;
                bannerView.Show();
            }
        }
        else
        {
            Debug.LogError("Banner Ad Not loaded!");
        }
    }

    public void Hide()
    {
        if (IsLoaded)
        {
            if (IsPlaying)
            {
                bannerView.Hide();
                IsPlaying = false;
            }
            else Debug.LogError("Banner Ad is not playing.");
        }
        else
        {
            Debug.LogError("Banner Ad Not loaded!");
        }
    }

    public void Close()
    {
        if (IsLoaded)
        {
            isClosed = true;
            
            if (IsPlaying) bannerView.Hide();

            IsPlaying = false;
            IsLoaded = false;

            bannerView.Destroy();
            bannerView = null;
        }
    }
}
