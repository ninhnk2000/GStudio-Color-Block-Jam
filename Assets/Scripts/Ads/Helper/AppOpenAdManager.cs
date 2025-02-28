// Copyright 2021 Google LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Events;
using AppsFlyerSDK;
using Firebase.Analytics;
using System.Collections.Generic;

public class AppOpenAdManager
{
    // #if UNITY_ANDROID
    //     private const string AD_UNIT_ID = "ca-app-pub-9457878244675693/3614647346";
    // #elif UNITY_IOS
    //     private const string AD_UNIT_ID = "ca-app-pub-9457878244675693/5619876469";
    // #else
#if UNITY_ANDROID
    private string AD_UNIT_ID = "ca-app-pub-8467610367562059/8314365051";
#elif UNITY_IPHONE
    string AD_UNIT_ID = "ca-app-pub-3940256099942544/5575463023";
#else
    private const string AD_UNIT_ID = "unexpected_platform";
#endif

    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    private bool isShowingAd = false;

    // COMPLETE: Add loadTime field
    private DateTime loadTime;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    public bool IsAdAvailable
    {
        get
        {
            // COMPLETE: Consider ad expiration
            return ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
        }
    }

    public void LoadAd(UnityAction actionLoadDone = null)
    {
        AdRequest request = new AdRequest();

        // Load an app open ad for portrait orientation
        AppOpenAd.Load(AD_UNIT_ID, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.GetMessage());
                return;
            }

            // App open ad is loaded
            ad = appOpenAd;
            Debug.Log("App open ad loaded");

            // COMPLETE: Keep track of time when the ad is loaded.
            loadTime = DateTime.UtcNow;

            actionLoadDone?.Invoke();
        }));
    }

    public void ShowAdIfAvailable()
    {  
        if (UserData.IsRemoveAds)
        {
            return;
        }

        if (!IsAdAvailable || isShowingAd)
        {
            LoadAd();

            return;
        }

        ad.OnAdFullScreenContentClosed += HandleAdDidDismissFullScreenContent;
        ad.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdFullScreenContentOpened += HandleAdDidPresentFullScreenContent;
        ad.OnAdImpressionRecorded += HandleAdDidRecordImpression;
        ad.OnAdPaid += HandlePaidEvent;
        ad.Show();
    }

    private void HandleAdDidDismissFullScreenContent()
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;

        LoadAd();
        //GameController.Instance.admobAds.ShowBanner();
    }

    private void HandleAdFailedToPresentFullScreenContent(AdError adError)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", adError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent()
    {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;
        // GameController.Instance.admobAds.DestroyBanner();
    }

    private void HandleAdDidRecordImpression()
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(AdValue adValue)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
            adValue.CurrencyCode, adValue.Value);

        //try
        //{
        //    if (adValue == null) return;
        //    double value = adValue.Value * 0.000001f;

        //    Firebase.Analytics.Parameter[] adParameters =
        //    {
        //             new Firebase.Analytics.Parameter("ad_source", "admob"),
        //             new Firebase.Analytics.Parameter("ad_format", "app_open_ads"),
        //             new Firebase.Analytics.Parameter("currency","USD"),
        //             new Firebase.Analytics.Parameter("value", value)
        //        };
        //    FirebaseAnalytics.LogEvent("ad_impression", adParameters);

        //    var dic = new Dictionary<string, string>
        //        {
        //            { "ad_format", "app_open_ads" }
        //        };
        //    AppsFlyerAdRevenue.logAdRevenue("Admob", AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob, value, "USD", dic);
        //}
        //catch
        //{

        //}

    }
}