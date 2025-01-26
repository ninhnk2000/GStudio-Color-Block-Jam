using UnityEngine;

public class ApplovinBannerManager : MonoBehaviour
{
// #if UNITY_IOS
// string bannerAdUnitId = "«iOS-ad-unit-ID»"; // Retrieve the ID from your account
// #else // UNITY_ANDROID
//     string bannerAdUnitId = "«Android-ad-unit-ID»"; // Retrieve the ID from your account
// #endif

//     private void Awake()
//     {
//         InitializeBannerAds();
//     }

//     public void InitializeBannerAds()
//     {
//         MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

//         // // Set background or background color for banners to be fully functional
//         // MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, «banner - background - color»);

//         MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
//         MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
//         MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
//         MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
//         MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
//         MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;
//     }

//     public void ShowBanner()
//     {
//         MaxSdk.ShowBanner(bannerAdUnitId);
//     }

//     public void DestroyBanner()
//     {
//         MaxSdk.DestroyBanner(bannerAdUnitId);
//     }

//     private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { }

//     private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }
}
