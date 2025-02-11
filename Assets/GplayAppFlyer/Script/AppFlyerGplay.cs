// Copyright (c) 2024 GplayJSC (gplayjsc.com)
// 
// Author: Axolotl (lamanh.w@gmail.com)
// 
// Created: 04/06/2024
// 
// File: AppFlyerGplay.cs
// 
// Note:

using System;
using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

public class AppFlyerGplay : MonoBehaviour, IAppsFlyerConversionData
{
    private static string devKey = "XM6HPCReBAqLH5uCaQHRDY";
    [SerializeField] private string appId;
    [SerializeField] private bool isDebug;
    [SerializeField] private bool getConversionData;

    private static AppFlyerGplay _instance;


    public void Awake()
    {
        //  Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        _instance = this;
    }


    public void Start()
    {
        AppsFlyerAdRevenue.setIsDebug(isDebug);
        AppsFlyer.setIsDebug(isDebug);
        AppsFlyer.initSDK(devKey, appId, getConversionData ? this : null);
#if UNITY_IOS && !UNITY_EDITOR
        AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(60);
#endif
        AppsFlyerAdRevenue.start();
        AppsFlyer.startSDK();
        Debug.Log("Start AppFlyer");
    }

    // Mark AppsFlyer CallBacks
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        if (isDebug) Debug.Log(conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }

    //     private static void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    //     {
    // #if UNITY_ANDROID
    //         AppsFlyer.updateServerUninstallToken(token.Token);
    // #endif
    //     }

    public static void LogRevenue(string placement, string network, string unit,
        string format, double value, string currency)
    {
        Dictionary<string, string> additionalParams = new Dictionary<string, string>();
        additionalParams.Add(AdRevenueScheme.COUNTRY, "USA");
        additionalParams.Add(AdRevenueScheme.AD_UNIT, unit);
        additionalParams.Add(AdRevenueScheme.AD_TYPE, format);
        additionalParams.Add(AdRevenueScheme.PLACEMENT, placement);

        var logRevenue = new AFAdRevenueData("monetizationNetworkEx", Parser(), currency, value);
        
        AppsFlyer.logAdRevenue(logRevenue, additionalParams);

        // Dictionary<string, string> addParams = new Dictionary<string, string>()
        // {
        //     { AFAdRevenueEvent.AD_UNIT, unit },
        //     { AFAdRevenueEvent.AD_TYPE, format },
        //     { AFAdRevenueEvent.PLACEMENT, placement }
        // };
        // AppsFlyerAdRevenue.logAdRevenue(
        //     network,
        //     Parser(),
        //     value,
        //     currency,
        //     addParams
        // );
        // return;

        // AppsFlyerAdRevenueMediationNetworkType Parser()
        // {
        //     network = network.ToLower();
        //     if (network.Contains("admob"))
        //         return AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob;
        //     if (network.Contains("max"))
        //         return AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax;
        //     return AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeCustomMediation;
        // }

        MediationNetwork Parser()
        {
            network = network.ToLower();
            if (network.Contains("admob"))
                return MediationNetwork.GoogleAdMob;
            if (network.Contains("max"))
                return MediationNetwork.ApplovinMax;
            return MediationNetwork.Custom;
        }
    }
}