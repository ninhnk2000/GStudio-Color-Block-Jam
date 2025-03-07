using UnityEngine;
using static GameEnum;

public static class GameConstants
{
    public static string MENU_SCENE = "Menu";
    public static string GAMEPLAY_SCENE = "Gameplay";

    #region SCREEN ROUTE
    public static string LOBBY_DETAIL_ROUTE = "Lobby Detail";
    #endregion

    #region POOLING
    public static string BOOSTER = "Booster";
    public static string VEHICLE_ENGINE_SOUND = "Vehicle Engine Sound";
    public static string HIT_OBSTACLE_SOUND = "Hit Obstacle Sound";
    public static string GET_IN_VEHICLE_SOUND = "Get In Vehicle Sound";
    public static string VEHICLE_MOVE_OUT_SOUND = "Vehicle Move Out Sound";
    public static string DESTROY_OBJECT_PART_FX = "Destroy Object Part Fx";
    #endregion

    #region COMMON TEXT
    public static string START_GAME = "Start Game";
    public static string CONNECTED = "Connected!";
    public static string DISCONNECTED = "Disconnected!";
    #endregion

    #region COLOR
    public static Color PRIMARY_BACKGROUND = new Color(30f / 255, 60f / 255, 50f / 255, 1);
    public static Color PRIMARY_TEXT = new Color(130f / 255, 255f / 255, 130f / 255, 1);
    public static Color ERROR_BACKGROUND = new Color(90f / 255, 40f / 255, 40f / 255, 1);
    public static Color ERROR_TEXT = new Color(255f / 255, 140f / 255, 140f / 255, 1);


    public static Color SAFERIO_RED = new Color(245f / 255, 62f / 255, 46f / 255, 1);
    public static Color SAFERIO_GREEN = new Color(159f / 255, 231f / 255, 18f / 255, 1);
    public static Color SAFERIO_ORANGE = new Color(244f / 255, 101f / 255, 47f / 255, 1);
    public static Color SAFERIO_PURPLE = new Color(214f / 255, 86f / 255, 236f / 255, 1);
    public static Color SAFERIO_BLUE = new Color(24f / 255, 142f / 255, 233f / 255, 1);
    public static Color SAFERIO_YELLLOW = new Color(246f / 255, 204f / 255, 53f / 255, 1);
    public static Color SAFERIO_WHITE = new Color(223f / 255, 217f / 255, 216f / 255, 1);
    public static Color SAFERIO_PINK = new Color(253f / 255, 20f / 255, 146f / 255, 1);
    public static Color SAFERIO_CYAN = new Color(41f / 255, 202f / 255, 163f / 255, 1);
    public static Color SAFERIO_BROWN = new Color(139f / 255, 82f / 255, 44f / 255, 1);
    public static Color SAFERIO_DISABLED = new Color(70f / 255, 70f / 255, 70f / 255, 1);
    #endregion

    #region HDR COLOR
    public static Color SAFERIO_HDR_GREEN = new Color(159f * 1.7f / 255, 231f * 1.7f / 255, 18f * 1.7f / 255, 1);
    public static Color SAFERIO_HDR_CYAN = new Color(41f * 1.7f / 255, 202f * 1.7f / 255, 163f * 1.7f / 255, 1);
    public static Color SAFERIO_HDR_YELLLOW = new Color(246f * 1.8f / 255, 204f * 1.8f / 255, 53f * 1.8f / 255, 1);
    public static Color SAFERIO_HDR_BROWN = new Color(139f * 3 / 255, 82f * 3 / 255, 44f * 3 / 255, 1);
    #endregion

    #region OBJECT POOLING
    public static string TAG_SOUND = "Tag Sound";
    public static string SCREW_BOX = "Screw Box";
    public static string SCREW_PORT_SLOT = "Screw Port Slot";
    public static string FAKE_SCREW = "Fake Screw";
    public static string HAMMER = "Hammer";
    public static string VACUMN = "Vacumn";
    #endregion

    #region SOUND
    public static string LOOSEN_SCREW_SOUND = "Loosen Screw Sound";
    public static string TIGHTEN_SCREW_SOUND = "Tighten Screw Sound";
    public static string LOOSEN_SCREW_FAIL_SOUND = "Loosen Screw Fail Sound";
    public static string SCREW_BOX_DONE_SOUND = "Screw Box Done Sound";
    public static string BREAK_OBJECT_SOUND = "Break Object Sound";
    public static string WIN_SOUND = "Win Sound";
    public static string LOSE_SOUND = "Lose Sound";
    public static string CLICK_SOUND = "Click Sound";
    public static string CLOSE_POPUP_SOUND = "Close Popup Sound";
    public static string UNLOCK_ADS_SCREW_BOX_SOUND = "Unlock Ads Screw Box Sound";
    public static string CLEAR_SCREW_PORTS_SOUND = "Clear Screw Ports Sound";
    #endregion

    #region ANIMATION
    public static string ANIMATION_STATE = "AnimationState";
    #endregion

    #region LANGUAGE
    public static string DEFAULT_LANGUAGE = "English";
    public static string[] AvailableLanguages = { "English", "French", "German", "Italian", "Japanese" };
    #endregion

    #region TASK
    public static string TASK_ITEM_UI = "Task Item UI";
    public static string TASK_DESCRIPTION_PARAMETER = "task_requirement_value";
    public static string UNSCREW_TASK_TRANSLATION_NAME = "Unscrew Task Description";
    public static string COMPLETE_LEVEL_TRANSLATION_NAME = "Complete Level Task Description";
    public static string LEVEL_COMPLETED = "Level Completed";
    public static string LEVEL_PARAMETER = "level";
    #endregion

    #region IAP
    public static string REMOVE_AD_ID = "remove_ad";
    #endregion

    #region SAVE/LOAD
    public static string SAVE_FILE_NAME = "Game Local Data";
    public static string IS_FIRST_TIME_OPEN_APP = "IS_FIRST_TIME_OPEN_APP";
    public static string CURRENT_LEVEL = "Current Level";
    public static string CURRENT_LEVEL_PLAY = "Current Level Play";
    public static string IS_LOOP_LEVEL = "IS_LOOP_LEVEL";
    public static string LAST_LEVEL_BEFORE_LOOP = "LAST_LEVEL_BEFORE_LOOP";
    public static string NUM_LEVEL_LOOP = "NUM_LEVEL_LOOP";
    public static string WEEKLY_TASKS = "Weekly Tasks";
    public static string SCREWS_DATA = "Screws Data";
    public static string COIN_ = "User Coin";
    public static string USER_RESOURCES = "User Resources";
    public static string IS_REMOVE_ADS = "Is Remove Ads";
    public static string GAME_SETTING = "GAME_SETTING";
    public static string USER_LIVES_DATA = "USER_LIVES_DATA";
    #endregion

    #region TRACKING
    public static string RETENTION_D = "RETENTION_D";
    public static string PREVIOUS_REVENUE_D0_D1 = "PREVIOUS_REVENUE_D0_D1";
    public static string REVENUE_D0_D1 = "REVENUE_D0_D1";
    public static string NUMBER_OF_DISPLAYED_INTERSTITIAL_D0_D1 = "NUMBER_OF_DISPLAYED_INTERSTITIAL_D0_D1";
    public static string DAYS_PLAYED = "DAYS_PLAYED";
    public static string PAYING_TYPE = "PAYING_TYPE";
    public static string NUMBER_OF_ADS_IN_PLAY = "NUMBER_OF_ADS_IN_PLAY";
    public static string NUMBER_OF_ADS_IN_DAY = "NUMBER_OF_ADS_IN_DAY";
    public static string FLAG_LINK = "FLAG_LINK";
    public static string IS_TRACKED_PREMISSION = "IS_TRACKED_PREMISSION";
    #endregion

    #region SCREW AWAY
    public static int NUMBER_SLOT_PER_SCREW_BOX = 3;
    public static int DEFAULT_INITIAL_NUMBER_SCREW_BOXES = 2;
    public static int DEFAULT_NUMBER_SCREW_PORT = 5;
    public static GameFaction[] SCREW_FACTION = new GameFaction[8]
        {   GameFaction.Red, GameFaction.Green, GameFaction.Orange, GameFaction.Purple,
            GameFaction.Blue, GameFaction.Yellow, GameFaction.Cyan,GameFaction.White };
    #endregion

    #region BOOSTER
    public static int ADD_HOLE_BOOSTER_INDEX = 0;
    public static int BREAK_OBJECT_BOOSTER_INDEX = 1;
    public static int CLEAR_HOLES_BOOSTER_INDEX = 2;
    public static int UNLOCK_SCREW_BOX_BOOSTER_INDEX = 3;
    #endregion

    #region LEVEL
    public static string IS_FIRST_WIN_LEVEL = "is_first_win_level";
    #endregion

    #region LOCALIZE
    public static string TIPS = "Tips";
    public static string NO_SCREW_TO_BE_CLEARED = "No screw to be cleared";
    public static string SCREW = "Screw";
    public static string SCREWS = "Screws";
    public static string WARNING = "Warning";
    public static string TOO_FAST_ACTION = "Too fast action";
    public static string BOOSTER_IN_USE = "The booster is already in use";
    public static string NO_INTERNET = "No internet";
    public static string PLEASE_CHECK_YOUR_INTERNET_CONNECTION = "Please check your internet connection";
    #endregion

    #region SWIPE
    public static int TYPICAL_FRAME_MILISECOND = 13;
    #endregion

    #region COLOR BLOCK JAM
    public static float DEFAULT_CAMERA_FIELD_OF_VIEW = 70;
    public static float TINY_FLOAT_VALUE = 0.001f;
    public static int DEFAULT_LIVES = 5;
    #endregion

    #region ADDRESSABLES
    public static string SNAP_PREVIEW_SPRITE = "SNAP_PREVIEW_SPRITE";
    #endregion

    #region SHADER
    public static string ADDITION_POSITION_X = "_AdditionPositionX";
    #endregion
}
