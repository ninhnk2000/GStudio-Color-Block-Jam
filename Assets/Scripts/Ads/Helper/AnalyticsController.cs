using AppsFlyerSDK;
using Firebase.Analytics;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class AnalyticsController : MonoBehaviour
{
    #region Init
    static UnityEvent onFinishFirebaseInit = new UnityEvent();
    private static bool m_firebaseInitialized = false;
    public static bool firebaseInitialized
    {
        get
        {
            return m_firebaseInitialized;
        }
        set
        {
            m_firebaseInitialized = value;
            if (value == true)
            {
                if (onFinishFirebaseInit != null)
                {
                    onFinishFirebaseInit.Invoke();
                    onFinishFirebaseInit.RemoveAllListeners();
                }

                //SetUserProperties();
            }
        }
    }

    #endregion

    private static void LogBuyInappAdjust(string inappID, string trancstionID)
    {

    }

    public static void LogEventFirebase(string eventName, Parameter[] parameters)
    {

        if (firebaseInitialized)
        {

            FirebaseAnalytics.LogEvent(eventName, parameters);
        }
        else
        {
            onFinishFirebaseInit.AddListener(() =>
            {
                FirebaseAnalytics.LogEvent(eventName, parameters);
            });
        }
    }

    //     public static void LogEventFacebook(string eventName, Dictionary<string, object> parameters)
    //     {
    //         if (FB.IsInitialized)
    //         {
    // #if !ENV_PROD
    //             parameters["test"] = true;
    // #endif
    //             //FB.LogAppEvent(eventName, null, parameters);
    //         }
    //     }

    public static void SetUserProperties()
    {
        if (!firebaseInitialized) return;

        FirebaseAnalytics.SetUserProperty(StringHelper.RETENTION_D, UserData.RetentionD.ToString());
        FirebaseAnalytics.SetUserProperty(StringHelper.DAYS_PLAYED, UserData.DaysPlayed.ToString());
        FirebaseAnalytics.SetUserProperty(StringHelper.PAYING_TYPE, UserData.PayingType.ToString());
        FirebaseAnalytics.SetUserProperty(StringHelper.LEVEL, UserData.CurrentLevel.ToString());
    }

    #region Event
    public void LogWatchVideo(ActionWatchVideo action, bool isHasVideo, bool isHasInternet, string level)
    {
        if (!firebaseInitialized) return;
        // Parameter[] parameters = new Parameter[4]
        // {
        //     new Parameter("actionWatch", action.ToString()) ,
        //     new Parameter("has_ads", isHasVideo.ToString()) ,
        //     new Parameter("has_internet", isHasInternet.ToString()) ,
        //     new Parameter("level", level)
        // };

        // FirebaseAnalytics.LogEvent("watch_video_game", parameters);
    }

    public void LogWatchInter(string action, bool isHasVideo, bool isHasInternet, string level)
    {
        if (!firebaseInitialized) return;
        //Parameter[] parameters = new Parameter[4]
        //{
        //    new Parameter("actionWatch", action.ToString()) ,
        //     new Parameter("has_ads", isHasVideo.ToString()) ,
        //      new Parameter("has_internet", isHasInternet.ToString()) ,
        //      new Parameter("level", level)
        //};

        //FirebaseAnalytics.LogEvent("show_inter", parameters);
    }

    public static void LogBuyInapp(string inappID, string trancstionID)
    {
        try
        {
            LogBuyInappAdjust(inappID, trancstionID);
        }
        catch
        {

        }
        try
        {
            if (firebaseInitialized)
            {
                Parameter[] parameters = new Parameter[1]
                {
                new Parameter("id", inappID),
                };
                LogEventFirebase("inapp_event", parameters);
            }
        }
        catch
        {

        }
    }

    public void LogStartLevel(int level)
    {
        try
        {
            if (!firebaseInitialized) return;

            Parameter[] parameters = new Parameter[1]
            {
            new Parameter("level", level.ToString())
            };


            FirebaseAnalytics.LogEvent("level_start", parameters);
        }
        catch
        {

        }
    }

    public void LogLevelComplet(int level, int timePlayLevel)
    {
        try
        {
            if (firebaseInitialized)
            {
                Parameter[] parameters = new Parameter[1]
           {
            new Parameter("level", level.ToString())
           };


                FirebaseAnalytics.LogEvent("level_complete", parameters);
            }
        }
        catch
        {

        }

        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            paramas.Add("af_level", level.ToString());
            AppsFlyer.sendEvent("af_level_achieved", paramas);
        }
        catch
        {

        }

        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent(string.Format("level_{0}_unlock", level), paramas);
        }
        catch
        {

        }


        try
        {
            //DWHLog.Log.LevelLog(UseProfile.CurrentLevel, UseProfile.CurrentLevel, timePlayLevel, 0, "", LevelStatus.pass);//Gọi khi pass level
            //Debug.Log
        }
        catch
        {

        }
    }

    public void LogLevelFail(int level)
    {
        if (!firebaseInitialized) return;
        Parameter[] parameters = new Parameter[1]
       {
            new Parameter("level", level.ToString())
       };


        FirebaseAnalytics.LogEvent("level_fail", parameters);
    }

    public void LogRequestVideoReward(string placement)
    {
        try
        {
            //if (firebaseInitialized)
            //{
            //    Parameter[] parameters = new Parameter[1]
            //   {
            //new Parameter("placement", placement.ToString())
            //   };


            //    FirebaseAnalytics.LogEvent("ads_reward_offer", parameters);
            //}
        }
        catch
        {

        }
    }

    public void LogVideoRewardEligible()
    {
        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent("af_rewarded_ad_eligible", paramas);
        }
        catch
        {

        }
    }

    public void LogClickToVideoReward(string placement)
    {
        // if (!firebaseInitialized) return;
        // Parameter[] parameters = new Parameter[1]
        //{
        //     new Parameter("placement", placement.ToString())
        //};


        // FirebaseAnalytics.LogEvent("ads_reward_click", parameters);
    }

    public void LogVideoRewardShow(string placement)
    {
        try
        {
            //if (firebaseInitialized)
            //{
            //    Parameter[] parameters = new Parameter[1]
            //   {
            //new Parameter("placement", placement.ToString())
            //   };


            //    FirebaseAnalytics.LogEvent("ads_reward_show", parameters);
            //}
        }
        catch
        {

        }

        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent("af_rewarded_displayed", paramas);
        }
        catch
        {

        }
    }

    public void LogVideoRewardLoadFail(string placement, string errormsg)
    {
        // if (!firebaseInitialized) return;
        // Parameter[] parameters = new Parameter[2]
        //{
        //     new Parameter("placement", placement.ToString()),
        //     new Parameter("errormsg", errormsg.ToString())
        //};


        // FirebaseAnalytics.LogEvent("ads_reward_fail", parameters);
    }

    public void LogVideoRewardShowDone(string placement)
    {
        try
        {
            //if (firebaseInitialized)
            //{
            //    Parameter[] parameters = new Parameter[1]
            //   {
            //new Parameter("placement", placement.ToString()),
            //   };


            //    FirebaseAnalytics.LogEvent("ads_reward_complete", parameters);
            //}
        }
        catch
        {

        }

        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent("af_rewarded_ad_completed", paramas);
        }
        catch
        {

        }

        try
        {
            //DWHLog.Log.AdsLog("", AdsType.VideoReward.ToString(), AdmobAds.RewardedAdUnitId.ToString(), "", "", placement, StateAds.Rewarded.ToString());
            //DWHLog.Log.AdsLog(UseProfile.CurrentLevel, AdType.reward, placement);
        }
        catch
        {

        }
    }

    public void LogInterLoadFail(string errormsg)
    {
        // if (!firebaseInitialized) return;
        // Parameter[] parameters = new Parameter[1]
        //{
        //     new Parameter("errormsg", errormsg.ToString())
        //};


        // FirebaseAnalytics.LogEvent("ad_inter_fail", parameters);
    }

    public void LogInterLoad()
    {
        try
        {
            //if (firebaseInitialized)
            //    FirebaseAnalytics.LogEvent("ad_inter_load");
        }
        catch
        {

        }


    }

    public void LoadInterEligible()
    {
        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent("af_inters_ad_eligible", paramas);
        }
        catch
        {

        }
    }

    public void LogInterShow(string actionWatchLog = "other")
    {
        try
        {
            if (firebaseInitialized)
            {
                FirebaseAnalytics.LogEvent("ad_inter_show");
            }

        }
        catch
        {

        }

        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent("af_inters_displayed", paramas);
        }
        catch
        {

        }
    }

    public void LogInterClick()
    {
        //if (!firebaseInitialized) return;
        //FirebaseAnalytics.LogEvent("ad_inter_click");
    }

    public void LogInterReady()
    {
        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent("af_inters_api_called", paramas);
        }
        catch
        {

        }
    }

    public void LogVideoRewardReady()
    {
        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            AppsFlyer.sendEvent("af_rewarded_api_called", paramas);
        }
        catch
        {

        }
    }

    public void LogTutLevelStart(int level)
    {
        try
        {
            if (firebaseInitialized)
                FirebaseAnalytics.LogEvent(string.Format("tutorial_start_{0}", level));

        }
        catch
        {

        }
    }

    public void LogTutLevelEnd(int level)
    {
        try
        {
            if (firebaseInitialized)
                FirebaseAnalytics.LogEvent(string.Format("tutorial_end_{0}", level));

        }
        catch
        {

        }
        try
        {
            Dictionary<string, string> paramas = new Dictionary<string, string>();
            paramas.Add("af_success", level.ToString());
            paramas.Add("af_tutorial_id", level.ToString());
            AppsFlyer.sendEvent("af_tutorial_completion", paramas);
        }
        catch
        {

        }
    }

    public void LogRevenueDay01(float revenue)
    {
        if (UserData.RetentionD <= 1)
        {
            UserData.RevenueD0_D1 += revenue;
            int cents = (int)(UserData.RevenueD0_D1 * 100);
            if (cents >= 1 && cents <= 60)
            {
                if (cents != UserData.PreviousRevenueD0_D1)
                {
                    UserData.PreviousRevenueD0_D1 = cents;
                    try
                    {
                        if (firebaseInitialized)
                        {
                            Firebase.Analytics.FirebaseAnalytics.LogEvent(string.Format("RevD0_{1}_Cents", cents));
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    try
                    {
                        Dictionary<string, string> paramas = new Dictionary<string, string>();
                        AppsFlyer.sendEvent(string.Format("RevD0+D1_{1}_Cents", cents), paramas);
                    }
                    catch
                    {
                        //FB.LogAppEvent(string.Format("RevD0+D1_{1}_Cents", cents));
                    }
                }
            }
        }
    }
    public void LogDisplayedInterstitialDay01()
    {
        if (UserData.RetentionD <= 1)
        {
            if (UserData.NumberOfDisplayedInterstitialD0_D1 >= 3 && UserData.NumberOfDisplayedInterstitialD0_D1 <= 30)
            {
                try
                {
                    if (firebaseInitialized)
                    {
                        Firebase.Analytics.FirebaseAnalytics.LogEvent(string.Format("D0+D1_{0}_Interstitals", UserData.NumberOfDisplayedInterstitialD0_D1));
                    }
                }
                catch
                { }
                try
                {
                    Dictionary<string, string> paramas = new Dictionary<string, string>();
                    AppsFlyer.sendEvent(string.Format("D0+D1_{0}_Interstitals", UserData.NumberOfDisplayedInterstitialD0_D1), paramas);
                }
                catch
                { }
                try
                {
                    //FB.LogAppEvent(string.Format("D0+D1_{0}_Interstitals", UseProfile.NumberOfDisplayedInterstitialD0_D1));
                }
                catch { }
            }
        }
    }

    public static void LogAdRevenueAppsflyer(string monetizationNetwork, AppsFlyerAdRevenueMediationNetworkType mediationNetwork, double revenue, string revenueCurrency, Dictionary<string, string> parameters)
    {
        AppsFlyerAdRevenue.logAdRevenue(monetizationNetwork, mediationNetwork, revenue, revenueCurrency, parameters);
    }
    public static void LogAdRevenueAppsflyer(MaxSdk.AdInfo impressionData)
    {
        Dictionary<string, string> afParams = new Dictionary<string, string>
        {
            { "ad_unit_name", impressionData.AdUnitIdentifier },
            { "ad_format", impressionData.AdFormat }
        };

        AppsFlyerAdRevenue.logAdRevenue(impressionData.NetworkName, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, impressionData.Revenue, "USD", afParams);
    }
    public static void LogAdMaxRevenueAppsflyer(MaxSdk.AdInfo impressionData)
    {
        Dictionary<string, string> afParams = new Dictionary<string, string>
        {
            { "ad_unit_name", impressionData.AdUnitIdentifier },
            { "ad_format", impressionData.AdFormat }
        };

        AppsFlyerAdRevenue.logAdRevenue(impressionData.NetworkName, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, impressionData.Revenue, "USD", afParams);
    }
    //public static void LogAdRevenueAppsflyer(IronSourceImpressionData impressionData)
    //{
    //    Dictionary<string, string> afParams = new Dictionary<string, string>
    //    {
    //        { "country", impressionData.country },
    //        { "ad_unit", impressionData.instanceName },
    //        { "ad_type", impressionData.adUnit}
    //    };
    //    // if (impressionData.adUnit == IronSourceAdUnits.REWARDED_VIDEO)
    //    //     adType = "rewarded_video";
    //    // else if(impressionData.adUnit == IronSourceAdUnits.INTERSTITIAL)
    //    //     adType = "interstitial";
    //    // else if (impressionData.adUnit == IronSourceAdUnits.BANNER)
    //    //     adType = "banner";
    //    // if(adType != "")
    //    //     afParams.Add("ad_type", adType);
    //    AppsFlyerAdRevenue.logAdRevenue(impressionData.adNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, impressionData.revenue.Value, "USD", afParams);
    //}
    //public static void LogAdMaxRevenueAppsflyer(IronSourceImpressionData impressionData)
    //{
    //    Dictionary<string, string> afParams = new Dictionary<string, string>
    //    {
    //        { "country", impressionData.country },
    //        { "ad_unit", impressionData.instanceName },
    //        { "ad_type", impressionData.adUnit}
    //    };
    //    // if (impressionData.adUnit == IronSourceAdUnits.REWARDED_VIDEO)
    //    //     adType = "rewarded_video";
    //    // else if(impressionData.adUnit == IronSourceAdUnits.INTERSTITIAL)
    //    //     adType = "interstitial";
    //    // else if (impressionData.adUnit == IronSourceAdUnits.BANNER)
    //    //     adType = "banner";
    //    // if(adType != "")
    //    //     afParams.Add("ad_type", adType);
    //    AppsFlyerAdRevenue.logAdRevenue(impressionData.adNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, impressionData.revenue.Value, "USD", afParams);
    //}
    #endregion

    public static void AppsFlyerPurchaseEvent(UnityEngine.Purchasing.Product product)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add("af_revenue", GetAppsflyerRevenue(product.metadata.localizedPrice));
        eventValue.Add("af_content_id", product.definition.id);
        eventValue.Add("af_currency", product.metadata.isoCurrencyCode);
        AppsFlyer.sendEvent("af_purchase", eventValue);
    }

    private static string GetAppsflyerRevenue(decimal amount)
    {
        decimal val = decimal.Multiply(amount, 0.7m);
        return val.ToString("#.#####", CultureInfo.InvariantCulture);
    }

    private void OnApplicationQuit()
    {
        SetUserProperties();
    }
}

public enum ActionClick
{
    None = 0,
    Play = 1,
    Rate = 2,
    Share = 3,
    Policy = 4,
    Feedback = 5,
    Term = 6,
    NoAds = 10,
    Settings = 11,
    ReplayLevel = 12,
    SkipLevel = 13,
    Return = 14,
    BuyStand = 15
}

public enum ActionWatchVideo
{
    None = 0,
    AddHole,
    BreakObject,
    ClearHoles,
    UnlockScrewBox,
    AddTime
}

public enum ActionShowInter
{
    None = 0,
    Skip_level = 1,
    Return = 2,
    BuyStand = 3,

    EndGame = 4,
    Click_Setting = 5,
    Click_Replay = 6
}
