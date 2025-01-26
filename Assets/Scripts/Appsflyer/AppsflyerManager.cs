using System.Collections.Generic;
using AppsFlyerSDK;
using UnityEngine;

public static class AppsflyerManager
{
    public static void TrackRevenue()
    {
        Dictionary<string, string> additionalParams = new Dictionary<string, string>();
        additionalParams.Add(AdRevenueScheme.COUNTRY, "USA");
        additionalParams.Add(AdRevenueScheme.AD_UNIT, "89b8c0159a50ebd1");
        additionalParams.Add(AdRevenueScheme.AD_TYPE, "Banner");
        additionalParams.Add(AdRevenueScheme.PLACEMENT, "place");
        var logRevenue = new AFAdRevenueData("monetizationNetworkEx", MediationNetwork.GoogleAdMob, "USD", 0.99);
        AppsFlyer.logAdRevenue(logRevenue, additionalParams);
    }
}
