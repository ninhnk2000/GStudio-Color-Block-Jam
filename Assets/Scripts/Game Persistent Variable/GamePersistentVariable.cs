using UnityEngine;

public static class GamePersistentVariable
{
    public static Vector2 canvasSize;
    public static Vector2 canvasScale;
    public static string iapInitializeFailReason;

    public static float tileSize;
    public static float tileDistance;
    public static Vector2 screenSizeWorld;

    #region LIVES
    public static LivesData livesData;
    #endregion

    #region SETTING
    public static bool IsVibrate;
    #endregion
}
