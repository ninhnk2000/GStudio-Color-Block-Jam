// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System;
// using System.Globalization;
// using Newtonsoft.Json;

// public class UseProfile : MonoBehaviour
// {
//     // public static bool IsLoggedInByFBorGG
//     // {
//     //     get
//     //     {
//     //         return IsLoggedIn;
//     //     }
//     // }

//     public static int CurrentLevel
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL, 1);
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     public int CurrentLevelPlay
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.CURRENT_LEVEL_PLAY, 1);
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.CURRENT_LEVEL_PLAY, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     public bool IsRemoveAds
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.REMOVE_ADS, 0) == 1;
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.REMOVE_ADS, value ? 1 : 0);
//             GPlayerPrefs.Save();
//         }
//     }

//     // public bool OnVibration
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.ONOFF_VIBRATION, 1) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.ONOFF_VIBRATION, value ? 1 : 0);
//     //         //MMVibrationManager.SetHapticsActive(value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public bool OnSound
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.ONOFF_SOUND, 1) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.ONOFF_SOUND, value ? 1 : 0);
//     //         GameController.Instance.musicManager.SetSoundVolume(value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public bool OnMusic
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.ONOFF_MUSIC, 1) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.ONOFF_MUSIC, value ? 1 : 0);
//     //         GameController.Instance.musicManager.SetMusicVolume(value ? 0.7f : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static bool OnNotification
//     // {
//     //     get => PlayerPrefs.GetInt(StringHelper.ONOFF_NOTIFICATION, 1) == 1;
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.ONOFF_NOTIFICATION, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static bool IsFirstTimeInstall
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.FIRST_TIME_INSTALL, 1) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.FIRST_TIME_INSTALL, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static bool IsReInstallSyncData
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.IS_REINSTALL_SYNC_DATA, 1) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.IS_REINSTALL_SYNC_DATA, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime LastTimeSyncName
//     // {
//     //     get => PlayerPrefs.GetString(StringHelper.LastTimeSyncName, DateTime.MinValue.ToString(CultureInfo.InvariantCulture)).ToDateTime();
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.LastTimeSyncName, value.ToString(CultureInfo.InvariantCulture));
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     public static int RetentionD
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.RETENTION_D, 0);
//         }
//         set
//         {
//             if (value < 0)
//                 value = 0;

//             GPlayerPrefs.SetInt(StringHelper.RETENTION_D, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     // public static bool LoggedRevenueCents
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.LOGGED_REVENUE_CENTS, 0) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.LOGGED_REVENUE_CENTS, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     public static int PreviousRevenueD0_D1
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.PREVIOUS_REVENUE_D0_D1, 0);
//         }
//         set
//         {
//             if (value < 0)
//                 value = 0;

//             GPlayerPrefs.SetInt(StringHelper.PREVIOUS_REVENUE_D0_D1, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     public static float RevenueD0_D1
//     {
//         get
//         {
//             return PlayerPrefs.GetFloat(StringHelper.REVENUE_D0_D1, 0);
//         }
//         set
//         {
//             if (value < 0f)
//                 value = 0f;

//             GPlayerPrefs.SetFloat(StringHelper.REVENUE_D0_D1, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     public static int NumberOfDisplayedInterstitialD0_D1
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERSTITIAL_D0_D1, 0);
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERSTITIAL_D0_D1, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     // public static int PreviousRevenueD1
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.PREVIOUS_REVENUE_D1, 0);
//     //     }
//     //     set
//     //     {
//     //         if (value < 0)
//     //             value = 0;

//     //         GPlayerPrefs.SetInt(StringHelper.PREVIOUS_REVENUE_D1, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static float RevenueD1
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetFloat(StringHelper.REVENUE_D1, 0);
//     //     }
//     //     set
//     //     {
//     //         if (value < 0f)
//     //             value = 0f;

//     //         GPlayerPrefs.SetFloat(StringHelper.REVENUE_D1, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public static int NumberOfDisplayedInterstitialD1
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERSTITIAL_D1, 0);
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.NUMBER_OF_DISPLAYED_INTERSTITIAL_D1, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     public static int DaysPlayed
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.DAYS_PLAYED, 1);
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.DAYS_PLAYED, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     public static int PayingType
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.PAYING_TYPE, 0);
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.PAYING_TYPE, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     // public static DateTime LastTimeOpenMinigame
//     // {
//     //     get
//     //     {
//     //         if (PlayerPrefs.HasKey(StringHelper.LAST_TIME_MINIGAME_SHOW_HOME))
//     //         {
//     //             var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.LAST_TIME_MINIGAME_SHOW_HOME));
//     //             return DateTime.FromBinary(temp);
//     //         }
//     //         else
//     //         {
//     //             var newDateTime = UnbiasedTime.Instance.Now().AddMinutes(-35);
//     //             PlayerPrefs.SetString(StringHelper.LAST_TIME_MINIGAME_SHOW_HOME, newDateTime.ToBinary().ToString());
//     //             PlayerPrefs.Save();
//     //             return newDateTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetString(StringHelper.LAST_TIME_MINIGAME_SHOW_HOME, value.ToBinary().ToString());
//     //         PlayerPrefs.Save();
//     //     }
    
//     // }
//     // public static DateTime FirstTimeOpenGame
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.FIRST_TIME_OPEN_GAME))
//     //         {
//     //             var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.FIRST_TIME_OPEN_GAME));
//     //             return DateTime.FromBinary(temp);
//     //         }
//     //         else
//     //         {
//     //             var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.FIRST_TIME_OPEN_GAME, newDateTime.ToBinary().ToString());
//     //             GPlayerPrefs.Save();
//     //             return newDateTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.FIRST_TIME_OPEN_GAME, value.ToBinary().ToString());
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime LastTimeOpenGame
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.LAST_TIME_OPEN_GAME))
//     //         {
//     //             var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.LAST_TIME_OPEN_GAME));
//     //             return DateTime.FromBinary(temp);
//     //         }
//     //         else
//     //         {
//     //             var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.LAST_TIME_OPEN_GAME, newDateTime.ToBinary().ToString());
//     //             GPlayerPrefs.Save();
//     //             return newDateTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.LAST_TIME_OPEN_GAME, value.ToBinary().ToString());
//     //         GPlayerPrefs.Save();
//     //     }
//     // }


//     // public static bool CanShowRate
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.CAN_SHOW_RATE, 1) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.CAN_SHOW_RATE, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static int LevelShowedRate
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.LEVEL_SHOWED_RATE, 0);
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.LEVEL_SHOWED_RATE, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     public static int NumberOfAdsInPlay;
//     public static int NumberOfAdsInDay
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.NUMBER_OF_ADS_IN_DAY, 0);
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.NUMBER_OF_ADS_IN_DAY, value);
//             GPlayerPrefs.Save();
//         }
//     }

//     // public static int NumberInterShowed
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.NUMBER_INTER_SHOWED, 0);
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.NUMBER_INTER_SHOWED, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static int NumberRewardShowed
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.NUMBER_REWARD_SHOWED, 0);
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.NUMBER_REWARD_SHOWED, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime LastTimeOnline
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.LAST_TIME_ONLINE))
//     //         {
//     //             var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.LAST_TIME_ONLINE));
//     //             return DateTime.FromBinary(temp);
//     //         }
//     //         else
//     //         {
//     //             var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.LAST_TIME_ONLINE, newDateTime.ToBinary().ToString());
//     //             GPlayerPrefs.Save();
//     //             return newDateTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.LAST_TIME_ONLINE, value.ToBinary().ToString());
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime LastTimePlayFab
//     // {
//     //     get => DateTime.Parse(PlayerPrefs.GetString(StringHelper.LAST_TIME_PLAYFAB, new DateTime().ToString()));
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.LAST_TIME_PLAYFAB, value.ToString());
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static string PieceData
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString(StringHelper.PIECE_DATA, "");
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.PIECE_DATA, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static Action evtCoinChange;
//     // public static int Coin
//     // {
//     //     get
//     //     {

//     //         return PlayerPrefs.GetInt(StringHelper.COIN, GiftDatabase.DefaultItem[GiftType.Coin]);
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.COIN, value);
//     //         evtCoinChange?.Invoke();
//     //         GameUtils.RaiseMessage(HomeMessages.GoldMessage.Instance);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public static int CoinSync
//     // {
//     //     get
//     //     {

//     //         return PlayerPrefs.GetInt(StringHelper.COIN, GiftDatabase.DefaultItem[GiftType.Coin]);
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.COIN, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public static long VersionData
//     // {
//     //     get
//     //     {
//     //         //Debug.Log("========== VersionData gggggggg ");
//     //         long value = -1;
//     //         System.Int64.TryParse(PlayerPrefs.GetString(StringHelper.VERSION_DATA, "0"), out value);
//     //         return value;
//     //     }
//     //     set
//     //     {
//     //        // Debug.Log("========== VersionData sssss " + value);
//     //         PlayerPrefs.SetString(StringHelper.VERSION_DATA, value.ToString());
//     //         PlayerPrefs.Save();
//     //     }
//     // }

//     // public static string NamePlayer
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString("FB_NAME_v2", "");
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString("FB_NAME_v2", value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public static string AvatarLink
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString(StringHelper.AVATAR_LINK, "");
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.AVATAR_LINK, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     public static string FlagLink
//     {
//         get
//         {
//             return PlayerPrefs.GetString(StringHelper.FLAG_LINK, "");
//         }
//         set
//         {
//             GPlayerPrefs.SetString(StringHelper.FLAG_LINK, value);
//             GPlayerPrefs.Save();
//         }
//     }
//     // public static int AvatarIDLocal
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt("Avatar_ID");
//     //     }
//     //     set
//     //     {

//     //         GPlayerPrefs.SetInt("Avatar_ID", value);
//     //         GPlayerPrefs.Save();

//     //     }
//     // }

//     // public static int ScorePvP
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt("ScorePvP");
//     //     }
//     //     set
//     //     {

//     //         GPlayerPrefs.SetInt("ScorePvP", value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static string JsonDataPlay
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString(StringHelper.JSON_DATA_PLAY, "");
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.JSON_DATA_PLAY, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public static string PlayFabId
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString("PlayFabId", string.Empty);
//     //     }
//     //     set
//     //     {

//     //         GPlayerPrefs.SetString("PlayFabId", value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public static bool IsLoggedIn
//     // {
//     //     get
//     //     {

//     //         return PlayerPrefs.GetInt(StringHelper.IS_LOGGED_IN, 0) == 1;

//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.IS_LOGGED_IN, value ? 1 : 0);
//     //         GPlayerPrefs.Save();

//     //     }
//     // }

//     // public static string AuthCodeGG
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString(StringHelper.AUTH_CODE_GG, "");
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.AUTH_CODE_GG, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static long LastTimeSync
//     // {
//     //     get
//     //     {
//     //         return long.TryParse(PlayerPrefs.GetString(StringHelper.LAST_TIME_SYNC), out long rt) ? rt : 0;
//     //     }
//     // }
//     // public static string LinkedId
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString(StringHelper.LINKED_ID, "");
//     //     }
//     //     set
//     //     {
//     //         Debug.Log("ERR: " + value);
//     //         GPlayerPrefs.SetString(StringHelper.LINKED_ID, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     public static bool IsTrackedPremission
//     {
//         get
//         {
//             return PlayerPrefs.GetInt(StringHelper.IS_TRACKED_PREMISSION, 0) == 1;
//         }
//         set
//         {
//             GPlayerPrefs.SetInt(StringHelper.IS_TRACKED_PREMISSION, value ? 1 : 0);
//             GPlayerPrefs.Save();
//         }
//     }

//     // public static bool IsAcceptTracker
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.IS_ACCEPT_TRACKED_PREMISSION, 0) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.IS_ACCEPT_TRACKED_PREMISSION, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // #region Item
//     // public static int GetNumItem(GiftType typeItem, int defaultNum = 3)
//     // {
//     //     return PlayerPrefs.GetInt(StringHelper.NUM_ITEM + typeItem.ToString(),
//     //         RemoteConfigController.GetIntConfig(String.Format(FirebaseConfig.DEFAULT_NUM_ITEM, typeItem.ToString()),
//     //      GiftDatabase.DefaultItem.ContainsKey(typeItem) ? GiftDatabase.DefaultItem[typeItem] : defaultNum));
//     // }

//     // public static void SetItem(GiftType typeItem, int amount)
//     // {
//     //     GPlayerPrefs.SetInt(StringHelper.NUM_ITEM + typeItem.ToString(), amount);
//     // }

//     // public static void AddItem(GiftType typeItem, int amount)
//     // {
//     //     if (amount < 0)
//     //     {
//     //         if (IsUnlimitedItem(typeItem))
//     //             return;
//     //     }
//     //     int current = GetNumItem(typeItem);
//     //     current += amount;

//     //     GPlayerPrefs.SetInt(StringHelper.NUM_ITEM + typeItem.ToString(), current);
//     // }

//     // public static bool IsUseItem(GiftType typeItem)
//     // {
//     //     if (!IsUnlimitedItem(typeItem) && GetNumItem(typeItem) <= 0)
//     //     {
//     //         UseItem(typeItem, false);
//     //         return false;
//     //     }

//     //     return PlayerPrefs.GetInt(StringHelper.USE_ITEM + typeItem.ToString(), 0) == 0 ? false : true;
//     // }

//     // public static void UseItem(GiftType typeItem, bool isUse)
//     // {
//     //     GPlayerPrefs.SetInt(StringHelper.USE_ITEM + typeItem.ToString(), isUse ? 1 : 0);
//     // }

//     // public static bool IsUnlimitedItem(GiftType typeItem)
//     // {
//     //     if (typeItem == GiftType.SuggetItem_BirdSort || typeItem == GiftType.ReturnItem_BirdSort || typeItem == GiftType.ShuffleItem_BirdSort || typeItem == GiftType.IceItem_BirdSort || typeItem == GiftType.BreakTheRule_BirdSort)
//     //         return false;

//     //     return GetTimeEndUnlimitedItem(typeItem) > UnbiasedTime.Instance.Now();
//     // }

//     // public static System.DateTime GetTimeEndUnlimitedItem(GiftType typeItem)
//     // {
//     //     if (GPlayerPrefs.HasKey(StringHelper.TIME_END_UNLIMITED_ITEM + typeItem))
//     //     {
//     //         var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.TIME_END_UNLIMITED_ITEM + typeItem));
//     //         return DateTime.FromBinary(temp);
//     //     }
//     //     else
//     //     {
//     //         var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //         GPlayerPrefs.SetString(StringHelper.TIME_END_UNLIMITED_ITEM + typeItem, newDateTime.ToBinary().ToString());
//     //         GPlayerPrefs.Save();
//     //         return newDateTime;
//     //     }
//     // }

//     // public static void AddTimeEndUnlimitedItem(GiftType typeItem, int seccons)
//     // {
//     //     var time = GetTimeEndUnlimitedItem(typeItem);
//     //     if (time < UnbiasedTime.Instance.Now())
//     //         time = UnbiasedTime.Instance.Now();
//     //     time = time.AddSeconds(seccons);
//     //     GPlayerPrefs.SetString(StringHelper.TIME_END_UNLIMITED_ITEM + typeItem, time.ToBinary().ToString());
//     //     GPlayerPrefs.Save();
//     // }

//     // public static void SetTimeEndUnlimitedItem(GiftType typeItem, long timeEnd)
//     // {
//     //     GPlayerPrefs.SetString(StringHelper.TIME_END_UNLIMITED_ITEM + typeItem, timeEnd.ToString());
//     //     GPlayerPrefs.Save();
//     // }

//     // public static int GetCurrentTurnSuggetBuyItemVideo(GiftType typeItem)
//     // {
//     //     return PlayerPrefs.GetInt(StringHelper.CURRENT_TURN_SUGGET_VIDEO_BUY_ITEM + typeItem, 0);
//     // }

//     // public static void AddTurnSuggetBuyItemVideo(GiftType typeItem, int value)
//     // {
//     //     int current = GetCurrentTurnSuggetBuyItemVideo(typeItem);
//     //     GPlayerPrefs.SetInt(StringHelper.CURRENT_TURN_SUGGET_VIDEO_BUY_ITEM + typeItem, current + value);
//     //     GPlayerPrefs.Save();
//     // }

//     // public static void SetTurnSuggetBuyItemVideo(GiftType typeItem, int value)
//     // {
//     //     GPlayerPrefs.SetInt(StringHelper.CURRENT_TURN_SUGGET_VIDEO_BUY_ITEM + typeItem, value);
//     //     GPlayerPrefs.Save();
//     // }

//     // public static DateTime GetLastTimeOverSuggetVideoBuyItem(GiftType typeItem)
//     // {

//     //     if (GPlayerPrefs.HasKey(StringHelper.LAST_TIME_OVER_SUGGET_VIDEO_BUY_ITEM + typeItem))
//     //     {
//     //         var temp = Convert.ToInt64(GPlayerPrefs.GetString(StringHelper.LAST_TIME_OVER_SUGGET_VIDEO_BUY_ITEM + typeItem));
//     //         return DateTime.FromBinary(temp);
//     //     }
//     //     else
//     //     {
//     //         var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //         GPlayerPrefs.SetString(StringHelper.LAST_TIME_OVER_SUGGET_VIDEO_BUY_ITEM + typeItem, newDateTime.ToBinary().ToString());
//     //         GPlayerPrefs.Save();
//     //         return newDateTime;
//     //     }
//     // }

//     // public static void SetLastTimeOverSuggetVideoBuyItem(GiftType typeItem, DateTime time)
//     // {
//     //     GPlayerPrefs.SetString(StringHelper.LAST_TIME_OVER_SUGGET_VIDEO_BUY_ITEM + typeItem, time.ToBinary().ToString());
//     //     GPlayerPrefs.Save();
//     // }
//     // #endregion

//     // #region Health
//     // public static int Health
//     // {
//     //     get
//     //     {
//     //         //if (IsUnlimitedHealth)
//     //         //    return GiftDatabase.DefaultItem[GiftType.Health];

//     //         //var timeRefill = GameController.Instance.segmentController.GetCurrentSegment().coreGameData.config.healthConfig.timeReFillHealth;
//     //         //int currentHealth = PlayerPrefs.GetInt(StringHelper.HEALTH, GiftDatabase.DefaultItem[GiftType.Health]);
//     //         //if (currentHealth <= 0
//     //         //    && TimeManager.CaculateTime(TimeLastOverHealth.AddSeconds(timeRefill), UnbiasedTime.Instance.Now()) >= timeRefill)
//     //         //{
//     //         //    GPlayerPrefs.SetInt(StringHelper.HEALTH, GiftDatabase.DefaultItem[GiftType.Health]);
//     //         //    UseProfile.IsRefillHealth = false;
//     //         //    return GiftDatabase.DefaultItem[GiftType.Health];
//     //         //}

//     //         //return currentHealth;

//     //         return 100;
//     //     }
//     //     set
//     //     {
//     //         if (!IsUnlimitedHealth)
//     //         {
//     //             GPlayerPrefs.SetInt(StringHelper.HEALTH, value);
//     //             evtCoinChange?.Invoke();
//     //             GPlayerPrefs.Save();
//     //         }
//     //     }
//     // }

//     // public static DateTime TimeLastOverHealth
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.TIME_LAST_OVER_HEALTH))
//     //         {
//     //             var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.TIME_LAST_OVER_HEALTH));
//     //             return DateTime.FromBinary(temp);
//     //         }
//     //         else
//     //         {
//     //             var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.TIME_LAST_OVER_HEALTH, newDateTime.ToBinary().ToString());
//     //             GPlayerPrefs.Save();
//     //             return newDateTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.TIME_LAST_OVER_HEALTH, value.ToBinary().ToString());
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static bool IsRefillHealth
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.IS_REFILL_HEALTH, 0) == 1;
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.IS_REFILL_HEALTH, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime TimeEndUnlimitedHealth
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.TIME_END_UNLIMITED_HEALTH))
//     //         {
//     //             var temp = Convert.ToInt64(PlayerPrefs.GetString(StringHelper.TIME_END_UNLIMITED_HEALTH));
//     //             return DateTime.FromBinary(temp);
//     //         }
//     //         else
//     //         {
//     //             var newDateTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.TIME_END_UNLIMITED_HEALTH, newDateTime.ToBinary().ToString());
//     //             GPlayerPrefs.Save();
//     //             return newDateTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.TIME_END_UNLIMITED_HEALTH, value.ToBinary().ToString());
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static void AddTimeEndUnlimitedHealth(int seconds)
//     // {
//     //     if (TimeEndUnlimitedHealth < UnbiasedTime.Instance.Now())
//     //     {
//     //         TimeEndUnlimitedHealth = UnbiasedTime.Instance.Now();
//     //     }

//     //     TimeEndUnlimitedHealth = TimeEndUnlimitedHealth.AddSeconds(seconds);
//     // }

//     // public static bool IsUnlimitedHealth
//     // {
//     //     get
//     //     {
//     //         return UseProfile.TimeEndUnlimitedHealth > UnbiasedTime.Instance.Now();
//     //     }
//     // }
//     // #endregion

//     // public static int IQScore
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.IQ_SCORE, 0);
//     //     }
//     //     set
//     //     {

//     //         GPlayerPrefs.SetInt(StringHelper.IQ_SCORE, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }

//     // public static int NumberLoseLevelInDay
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.NUM_LOSE_LEVEL_TO_DAY, 0);
//     //     }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.NUM_LOSE_LEVEL_TO_DAY, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }


//     // public static bool GDPRContentValue;

//     // public static bool IsClickTutPlayHome
//     // {
//     //     get => PlayerPrefs.GetInt("IsClickTutPlayHome", 0) == 1;
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt("IsClickTutPlayHome", value ? 1 : 0);
//     //         PlayerPrefs.Save();
//     //     }
//     // }

//     // public static int NumPlayedInDay
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.NUM_PLAYED_IN_DAY, 0);
//     //     }
//     //     set
//     //     {

//     //         PlayerPrefs.SetInt(StringHelper.NUM_PLAYED_IN_DAY, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }
//     // public static bool hadBuySuggest { get; set; }
    
//     // public string OwnedBranchSkin
//     // {
//     //     get { return GPlayerPrefs.GetString(StringHelper.OWNED_BRANCH_SKIN, ""); }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.OWNED_BRANCH_SKIN, value);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public static bool StarterPackIsCompleted
//     // {
//     //     get { return GPlayerPrefs.GetInt(StringHelper.STARTER_PACK_IS_COMPLETED, 0) == 1; }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetInt(StringHelper.STARTER_PACK_IS_COMPLETED, value ? 1 : 0);
//     //         GPlayerPrefs.Save();
//     //     }
//     // }
//     // public string OwnedThemeSkin
//     // {
//     //     get { return GPlayerPrefs.GetString(StringHelper.OWNED_THEME, ""); }
//     //     set
//     //     {
//     //         GPlayerPrefs.SetString(StringHelper.OWNED_THEME, value);
//     //         GPlayerPrefs.Save();
//     //     } 
//     // }
    
//     // #region MiniGame
//     // public static int ConnectBirdTurnBuy
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.CONNECT_BIRD_TURN_BUY, 0);
//     //     }
//     //     set
//     //     {
//     //         Debug.Log("Set turnBuy " + value);
//     //         PlayerPrefs.SetInt(StringHelper.CONNECT_BIRD_TURN_BUY, value);
//     //         PlayerPrefs.Save();
//     //         // GameController.Instance.playerData.SaveData();
//     //     }
//     // }
//     // public static int Version
//     // {

//     //     get { return PlayerPrefs.GetInt(StringHelper.VERSION_MINI_GAME_MINIGAME, 0); }
//     //     set { PlayerPrefs.SetInt(StringHelper.VERSION_MINI_GAME_MINIGAME, value); }
//     // }
//     // public static int PointConnectBirdMiniGame
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.POINT_MINIGAME_CONNECT_BIRD, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.POINT_MINIGAME_CONNECT_BIRD, value);
//     //         // GameController.Instance.playerData.SaveData();
//     //     }
//     // }
//     // public static string lsRewardConnectBirdMNGOfPlayer
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetString(StringHelper.LIST_REWARD_MINI_GAME_CONNECT_BIRD, "[]");
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetString(StringHelper.LIST_REWARD_MINI_GAME_CONNECT_BIRD, value);
//     //         // GameController.Instance.playerData.SaveData();
//     //     }
//     // }
//     // public static int WasCommpleteConnectBirdMiniGame
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.WAS_COMMPLETE_MINI_GAME_CONNECT_BIRD, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.WAS_COMMPLETE_MINI_GAME_CONNECT_BIRD, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }
//     // public static int ConnectBirdTurnFree
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.CONNECT_BIRD_TURN_FREE, GameController.Instance.segmentController.GetCurrentSegment().minigameConnectSegmentData.config.num_turn_free);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.CONNECT_BIRD_TURN_FREE, value);
//     //         // GameController.Instance.playerData.SaveData();
//     //     }
//     // }
//     // public static int LimitNumbWatchVideoInDay
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.LIMIT_NUMB_WATCH_VIDEO_IN_DAY, GameController.Instance.segmentController.GetCurrentSegment().minigameConnectSegmentData.config.num_watch_video_buy_egg);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.LIMIT_NUMB_WATCH_VIDEO_IN_DAY, value);
//     //     }
//     // }
//     // public static int FirstOpenConnectBirdMNGame
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.FIRST_OPEN_CONNECT_BIRD_MINI_GAME, 0);
//     //     }
//     //     set
//     //     {

//     //         PlayerPrefs.SetInt(StringHelper.FIRST_OPEN_CONNECT_BIRD_MINI_GAME, value);
//     //         PlayerPrefs.Save();
//     //         // GameController.Instance.playerData.SaveData();
//     //     }
//     // }
//     // public static int FirstShowHand
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.FIRST_SHOW_HAND, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.FIRST_SHOW_HAND, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }
//     // public static int CurrentNumWatchVideoInDay
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.CURRENT_NUM_WATCH_VIDEO_IN_DAY_MINI_GAME_CONNECT_BIRD, 3);
//     //     }
//     //     set
//     //     {
//     //         Debug.Log("Curent num watch video " + value);
//     //         PlayerPrefs.SetInt(StringHelper.CURRENT_NUM_WATCH_VIDEO_IN_DAY_MINI_GAME_CONNECT_BIRD, value);
//     //     }
//     // }
//     // public static DateTime DataToResetMiniGame
//     // {
//     //     get
//     //     {
//     //         if (!PlayerPrefs.HasKey(StringHelper.DATA_TO_RESET_MINI_GAME_CONNECT_BIRD))
//     //         {

//     //             var temp = JsonConvert.SerializeObject(UnbiasedTime.Instance.Now());
//     //             PlayerPrefs.SetString(StringHelper.DATA_TO_RESET_MINI_GAME_CONNECT_BIRD, temp);
//     //             PlayerPrefs.Save();
//     //             return UnbiasedTime.Instance.Now();
//     //         }
//     //         else
//     //         {
//     //             var oldata = JsonConvert.DeserializeObject<DateTime>(PlayerPrefs.GetString(StringHelper.DATA_TO_RESET_MINI_GAME_CONNECT_BIRD));
//     //             return oldata;
//     //         }
//     //     }
//     //     set
//     //     {

//     //         var temp = JsonConvert.SerializeObject(value);
//     //         PlayerPrefs.SetString(StringHelper.DATA_TO_RESET_MINI_GAME_CONNECT_BIRD, temp);
//     //     }
//     // }
//     // public static int FirstWeek
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.FIRST_WEEK, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.FIRST_WEEK, value);
//     //     }
//     // }
//     // public static DateTime DataToResetShopMiniGame
//     // {
//     //     get
//     //     {
//     //         if (!PlayerPrefs.HasKey(StringHelper.DATA_TO_RESET_MINI_GAME_SHOP_CONNECT_BIRD))
//     //         {

//     //             var temp = JsonConvert.SerializeObject(UnbiasedTime.Instance.Now());
//     //             PlayerPrefs.SetString(StringHelper.DATA_TO_RESET_MINI_GAME_SHOP_CONNECT_BIRD, temp);
//     //             PlayerPrefs.Save();
//     //             return UnbiasedTime.Instance.Now();
//     //         }
//     //         else
//     //         {
//     //             var oldata = JsonConvert.DeserializeObject<DateTime>(PlayerPrefs.GetString(StringHelper.DATA_TO_RESET_MINI_GAME_SHOP_CONNECT_BIRD));
//     //             return oldata;
//     //         }
//     //     }
//     //     set
//     //     {

//     //         Debug.Log("Current num " + value );
//     //         var temp = JsonConvert.SerializeObject(value);
//     //         PlayerPrefs.SetString(StringHelper.DATA_TO_RESET_MINI_GAME_SHOP_CONNECT_BIRD, temp);
//     //     }
//     // }
//     // public static void ResetDayMiniGame()
//     // {
//     //     ConnectBirdTurnFree = GameController.Instance.segmentController.GetCurrentSegment().minigameConnectSegmentData
//     //         .config.num_turn_free;
//     //     CurrentNumWatchVideoInDay = LimitNumbWatchVideoInDay = GameController.Instance.segmentController.GetCurrentSegment()
//     //         .minigameConnectSegmentData
//     //         .config.num_watch_video_buy_egg;
//     //     var nowTime = UnbiasedTime.Instance.Now().AddDays(1);
//     //     var newDayStartTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0);
//     //     DataToResetShopMiniGame = newDayStartTime;
//     // }
//     // #endregion

//     // #region IAPPAck

//     // public static int LevelPlayInDay
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.NUM_PLAYED_LEVEL_IN_DAY, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.NUM_PLAYED_LEVEL_IN_DAY, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime DailySaleTimeEnd
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.DAILY_TIME_END))
//     //         {
//     //             return DateTime.Parse(PlayerPrefs.GetString(StringHelper.DAILY_TIME_END));
//     //         }
//     //         else
//     //         {
//     //             var defaultTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.DAILY_TIME_END, defaultTime.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             GPlayerPrefs.Save();
//     //             return defaultTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         var nowTime = UnbiasedTime.Instance.Now();
//     //         var newDayStartTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0);
//     //         if (DailySaleTimeEnd < newDayStartTime)
//     //         {
//     //             PlayerPrefs.SetString(StringHelper.DAILY_TIME_END, value.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             PlayerPrefs.Save();
//     //         }
//     //     }
//     // }

//     // public static int DailyBought
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.DAILY_BUY_TIME, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.DAILY_BUY_TIME, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime CoinSaleTimeEnd
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.COIN_SALE_TIME_END))
//     //         {
//     //             return DateTime.Parse(PlayerPrefs.GetString(StringHelper.COIN_SALE_TIME_END));
//     //         }
//     //         else
//     //         {
//     //             var defaultTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.COIN_SALE_TIME_END, defaultTime.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             GPlayerPrefs.Save();
//     //             return defaultTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         var nowTime = UnbiasedTime.Instance.Now();
//     //         var newDayStartTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0);
//     //         if (CoinSaleTimeEnd < newDayStartTime)
//     //         {
//     //             PlayerPrefs.SetString(StringHelper.COIN_SALE_TIME_END, value.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             PlayerPrefs.Save();
//     //         }
//     //     }
//     // }

//     // public static int CoinBought
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.COIN_BUY_TIME, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.COIN_BUY_TIME, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime ItemSaleTimeEnd
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.ITEM_SALE_TIME_END))
//     //         {
//     //             return DateTime.Parse(PlayerPrefs.GetString(StringHelper.ITEM_SALE_TIME_END));
//     //         }
//     //         else
//     //         {
//     //             var defaultTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.ITEM_SALE_TIME_END, defaultTime.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             GPlayerPrefs.Save();
//     //             return defaultTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         var nowTime = UnbiasedTime.Instance.Now();
//     //         var newDayStartTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0);
//     //         if (ItemSaleTimeEnd < newDayStartTime)
//     //         {
//     //             PlayerPrefs.SetString(StringHelper.ITEM_SALE_TIME_END, value.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             PlayerPrefs.Save();
//     //         }
//     //     }
//     // }

//     // public static int ItemBought
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.ITEM_BUY_TIME, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.ITEM_BUY_TIME, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }

//     // public static DateTime WeekendSaleTimeEnd
//     // {
//     //     get
//     //     {
//     //         if (GPlayerPrefs.HasKey(StringHelper.WEEKEND_SALE_TIME_END))
//     //         {
//     //             return DateTime.Parse(PlayerPrefs.GetString(StringHelper.WEEKEND_SALE_TIME_END));
//     //         }
//     //         else
//     //         {
//     //             var defaultTime = UnbiasedTime.Instance.Now().AddDays(-1);
//     //             GPlayerPrefs.SetString(StringHelper.WEEKEND_SALE_TIME_END, defaultTime.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             GPlayerPrefs.Save();
//     //             return defaultTime;
//     //         }
//     //     }
//     //     set
//     //     {
//     //         var nowTime = UnbiasedTime.Instance.Now().AddDays(6);
//     //         var newDayStartTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 0, 0, 0);
//     //         if (WeekendSaleTimeEnd < newDayStartTime)
//     //         {
//     //             PlayerPrefs.SetString(StringHelper.WEEKEND_SALE_TIME_END, value.ToString("yyyy-MM-dd HH:mm:ss"));
//     //             PlayerPrefs.Save();
//     //         }
//     //     }
//     // }

//     // public static int WeekendBought
//     // {
//     //     get
//     //     {
//     //         return PlayerPrefs.GetInt(StringHelper.WEEKEND_BUY_TIME, 0);
//     //     }
//     //     set
//     //     {
//     //         PlayerPrefs.SetInt(StringHelper.WEEKEND_BUY_TIME, value);
//     //         PlayerPrefs.Save();
//     //     }
//     // }

//     // #endregion

// }



