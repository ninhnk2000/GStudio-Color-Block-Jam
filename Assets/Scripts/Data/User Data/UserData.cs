using System.Threading.Tasks;
using UnityEngine;

public static class UserData
{
    public static bool IsFirstTimeOpenApp
    {
        get
        {
            return DataUtility.Load(GameConstants.IS_FIRST_TIME_OPEN_APP, true);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.IS_FIRST_TIME_OPEN_APP, value);
        }
    }

    public static int CurrentLevel
    {
        get
        {
            return DataUtility.Load(GameConstants.CURRENT_LEVEL, 1);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.CURRENT_LEVEL, value);
        }
    }

    public static int CurrentLevelPlay
    {
        get
        {
            return DataUtility.Load(GameConstants.CURRENT_LEVEL_PLAY, 1);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.CURRENT_LEVEL_PLAY, value);
        }
    }

    public static bool IsLoopLevel
    {
        get
        {
            return DataUtility.Load(GameConstants.IS_LOOP_LEVEL, false);
        }
        // set
        // {
        //     DataUtility.SaveAsync(GameConstants.IS_LOOP_LEVEL, value);
        // }
    }

    public async static Task SetIsLoopLevel(bool value)
    {
        await DataUtility.SaveAsync(GameConstants.IS_LOOP_LEVEL, value);
    }

    public static int LastLevelBeforeLoop
    {
        get
        {
            return DataUtility.Load(GameConstants.LAST_LEVEL_BEFORE_LOOP, 1);
        }
        // set
        // {
        //     DataUtility.SaveAsync(GameConstants.LAST_LEVEL_BEFORE_LOOP, value);
        // }
    }

    public async static Task SetIsLastLevelBeforeLoop(int value)
    {
        await DataUtility.SaveAsync(GameConstants.LAST_LEVEL_BEFORE_LOOP, value);
    }

    public static int NumLevelLoop
    {
        get
        {
            return DataUtility.Load(GameConstants.NUM_LEVEL_LOOP, 0);
        }
    }

    public async static Task SetNumLevelLoop(int value)
    {
        await DataUtility.SaveAsync(GameConstants.NUM_LEVEL_LOOP, value);
    }

    public static bool IsRemoveAds
    {
        get
        {
            return DataUtility.Load(GameConstants.IS_REMOVE_ADS, false);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.IS_REMOVE_ADS, value);
        }
    }

    public static int RetentionD
    {
        get
        {
            return DataUtility.Load(GameConstants.RETENTION_D, 0);
        }
        set
        {
            if (value < 0)
                value = 0;

            DataUtility.SaveAsync(GameConstants.RETENTION_D, value);
        }
    }

    public static int PreviousRevenueD0_D1
    {
        get
        {
            return DataUtility.Load(GameConstants.PREVIOUS_REVENUE_D0_D1, 0);
        }
        set
        {
            if (value < 0)
                value = 0;

            DataUtility.SaveAsync(GameConstants.PREVIOUS_REVENUE_D0_D1, value);
        }
    }

    public static float RevenueD0_D1
    {
        get
        {
            return DataUtility.Load(GameConstants.REVENUE_D0_D1, 0);
        }
        set
        {
            if (value < 0)
                value = 0;

            DataUtility.SaveAsync(GameConstants.REVENUE_D0_D1, value);
        }
    }

    public static int NumberOfDisplayedInterstitialD0_D1
    {
        get
        {
            return DataUtility.Load(GameConstants.NUMBER_OF_DISPLAYED_INTERSTITIAL_D0_D1, 0);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.NUMBER_OF_DISPLAYED_INTERSTITIAL_D0_D1, value);
        }
    }

    public static int DaysPlayed
    {
        get
        {
            return DataUtility.Load(GameConstants.DAYS_PLAYED, 1);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.DAYS_PLAYED, value);
        }
    }

    public static int PayingType
    {
        get
        {
            return DataUtility.Load(GameConstants.PAYING_TYPE, 0);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.PAYING_TYPE, value);
        }
    }

    public static int NumberOfAdsInPlay
    {
        get
        {
            return DataUtility.Load(GameConstants.NUMBER_OF_ADS_IN_PLAY, 0);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.NUMBER_OF_ADS_IN_PLAY, value);
        }
    }

    public static int NumberOfAdsInDay
    {
        get
        {
            return DataUtility.Load(GameConstants.NUMBER_OF_ADS_IN_DAY, 0);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.NUMBER_OF_ADS_IN_DAY, value);
        }
    }

    public static string FlagLink
    {
        get
        {
            return DataUtility.Load(GameConstants.FLAG_LINK, "");
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.FLAG_LINK, value);
        }
    }

    public static bool IsTrackedPremission
    {
        get
        {
            return DataUtility.Load(GameConstants.IS_TRACKED_PREMISSION, false);
        }
        set
        {
            DataUtility.SaveAsync(GameConstants.IS_TRACKED_PREMISSION, value);
        }
    }
}
