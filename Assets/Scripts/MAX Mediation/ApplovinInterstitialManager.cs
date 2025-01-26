using System;
using UnityEngine;

public class ApplovinInterstitialManager : MonoBehaviour
{
// #if UNITY_IOS
// string adUnitId = "«iOS-ad-unit-ID»";
// #else
//     string adUnitId = "«Android-ad-unit-ID»";
// #endif

//     int retryAttempt;

//     public void InitializeInterstitialAds()
//     {
//         MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
//         MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
//         MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
//         MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
//         MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
//         MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
//         MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialAdRevenuePaidEvent;

//         LoadInterstitial();
//     }

//     private void LoadInterstitial()
//     {
//         MaxSdk.LoadInterstitial(adUnitId);
//     }

//     public void ShowInterstitial()
//     {
//         if (MaxSdk.IsInterstitialReady(adUnitId))
//         {
//             MaxSdk.ShowInterstitial(adUnitId);
//         }
//     }

//     private void OnInterstitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {

//     }

//     private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         retryAttempt = 0;
//     }

//     private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//     {
//         retryAttempt++;
//         double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

//         Invoke("LoadInterstitial", (float)retryDelay);
//     }

//     private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
//     {
//         LoadInterstitial();
//     }

//     private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         LoadInterstitial();
//     }
}
