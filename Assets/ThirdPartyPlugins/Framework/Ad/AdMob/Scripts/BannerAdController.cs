#if ADS_ADMOB
using GoogleMobileAds.Api;
using System;
using UnityEngine;

namespace Szn.Framework.AdMob
{
    public class BannerAdController
    {
        private readonly string bannerAdId;
        private BannerView bannerView;
        private readonly AdSize adSize;
        private readonly AdPosition adPosition;

        private bool isLoading;
        private bool isClosed;
        private bool isLoaded;
        private bool isPlaying;

        public bool IsLoaded
        {
            get { return bannerView != null && isLoaded; }
        }

        public bool IsPlaying
        {
            get { return bannerView != null && isPlaying; }
        }

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

        public BannerAdController(string InBannerAdId, AdSize InBannerSize, AdPosition InAdPosition,
            Action<bool> InLoadCallback = null)
        {
            isLoading = false;
            isLoaded = false;
            isPlaying = false;
            isClosed = false;

            bannerAdId = InBannerAdId;
            adSize = InBannerSize;
            adPosition = InAdPosition;

            if (null != InLoadCallback) RegisterAdLoadCallback(InLoadCallback);

            RequestBanner();
        }

        public void Request()
        {
            if (isLoading)
            {
                Debug.LogError("Banner Ad is loading, Please try again later.");
            }
            else if (isPlaying)
            {
                Debug.LogError("Banner Ad is playing, Please hide then try again.");
            }
            else
            {
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
            isLoaded = false;
            isClosed = false;
            isPlaying = false;

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
                Destroy();
                if (loadCallback != null) loadCallback.Invoke(false);
            }
            else
            {
                isLoading = false;
                isLoaded = true;
                bannerView.Hide();
                if (loadCallback != null) loadCallback.Invoke(true);
            }
        }

        public void HandleOnAdFailedToLoad(object InSender, AdFailedToLoadEventArgs InArgs)
        {
#if AD_DEBUG
            Debug.Log("[Banner Ad]    HandleFailedToReceiveAd event received with message: "
                      + InArgs.Message);
#endif
            isLoading = false;
            isLoaded = false;
            if (loadCallback != null) loadCallback.Invoke(false);
        }

        public void HandleOnAdOpened(object InSender, EventArgs InArgs)
        {
#if AD_DEBUG
            Debug.Log("[Banner Ad]    HandleAdOpened event received");
#endif
            isPlaying = true;
        }

        public void HandleOnAdClosed(object InSender, EventArgs InArgs)
        {
#if AD_DEBUG
            Debug.Log("[Banner Ad]    HandleAdClosed event received");
#endif
            isPlaying = false;
        }

        public void HandleOnAdLeavingApplication(object InSender, EventArgs InArgs)
        {
#if AD_DEBUG
            Debug.Log("[Banner Ad]    HandleAdLeavingApplication event received");
#endif
        }

        public void Show()
        {
            if (isLoaded)
            {
                if (isPlaying) Debug.LogError("Banner Ad is playing.");
                else
                {
                    bannerView.Show();
                }
            }
            else
            {
                Debug.LogError("Banner Ad Not loaded!");
                Request();
            }
        }

        public void Hide()
        {
            if (isLoaded)
            {
                if (isPlaying)
                {
                    bannerView.Hide();
                    isPlaying = false;
                }
                else Debug.LogError("Banner Ad is not playing.");
            }
            else
            {
                Debug.LogError("Banner Ad Not loaded!");
                Request();
            }
        }

        public void Destroy()
        {
            if (isLoaded)
            {
                isClosed = true;

                if (isPlaying) bannerView.Hide();

                isPlaying = false;
                isLoaded = false;

                bannerView.Destroy();
                bannerView = null;
            }
        }
    }
}
#endif