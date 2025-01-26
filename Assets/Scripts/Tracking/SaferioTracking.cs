using Firebase.Analytics;
using UnityEngine;
using static GameEnum;

public static class SaferioTracking
{
    public static void TrackLevelFirstWin(int level, LevelBoosterObserver levelBoosterObserver)
    {
#if !UNITY_EDITOR
        Parameter[] parameters =
        {
            new Parameter("level", level.ToString()),
            new Parameter("add_hole_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[0]),
            new Parameter("break_object_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[1]),
            new Parameter("clear_holes_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[2]),
            new Parameter("unlock_screw_box_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[3]),
        };

        FirebaseAnalytics.LogEvent("level_first_win", parameters);
#endif
    }

    public static void TrackLevelWin(int level, LevelBoosterObserver levelBoosterObserver)
    {
#if !UNITY_EDITOR
        Parameter[] parameters =
        {
            new Parameter("level", level.ToString()),
            new Parameter("add_hole_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[0]),
            new Parameter("break_object_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[1]),
            new Parameter("clear_holes_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[2]),
            new Parameter("unlock_screw_box_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[3]),
        };

        FirebaseAnalytics.LogEvent("level_win", parameters);
#endif
    }

    public static void TrackLevelLose(int level, float progress, LevelBoosterObserver levelBoosterObserver, string reason)
    {
#if !UNITY_EDITOR
        Parameter[] parameters =
        {
            new Parameter("level", level.ToString()),
            new Parameter("progress", progress.ToString()),
            new Parameter("add_hole_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[0]),
            new Parameter("break_object_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[1]),
            new Parameter("clear_holes_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[2]),
            new Parameter("unlock_screw_box_booster_used", levelBoosterObserver.BoosterQuantitiesUsed[3]),
            new Parameter("reason", reason)
        };

        FirebaseAnalytics.LogEvent("level_lose", parameters);
#endif
    }

    public static void TrackBoosterUsage(BoosterType boosterType)
    {
#if !UNITY_EDITOR
        Parameter[] parameters =
        {
            new Parameter("booster_type", boosterType.ToString())
        };

        FirebaseAnalytics.LogEvent("use_booster", parameters);
#endif
    }
}
