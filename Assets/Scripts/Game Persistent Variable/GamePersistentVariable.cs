using UnityEngine;

public static class GamePersistentVariable
{
    public static Vector2 canvasSize;
    public static Vector2 canvasScale;
    public static string iapInitializeFailReason;

    public static float tileSize;
    public static float tileDistance;
    public static Vector2 screenSizeWorld;

    #region GAME FLOW
    public static bool isPendingReplay;
    #endregion

    #region LIVES
    public static LivesData livesData;
    public static bool isLevelDirty;
    #endregion

    #region SETTING
    public static bool IsVibrate;
    #endregion
}
