using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper
{
    public const string ONOFF_SOUND = "ONOFF_SOUND";
    public const string ONOFF_MUSIC = "ONOFF_MUSIC";
    public const string ONOFF_VIBRATION = "ONOFF_VIBRATION";
    public const string ONOFF_NOTIFICATION = "ONOFF_NOTIFICATION";
    public const string FIRST_TIME_INSTALL = "FIRST_TIME_INSTALL";
    public const string IS_REINSTALL_SYNC_DATA = "IS_REINSTALL_SYNC_DATA";
    public const string LastTimeSyncName = "lasttimesyncname";

    public const string VERSION_FIRST_INSTALL = "VERSION_FIRST_INSTALL";
    public const string REMOVE_ADS = "REMOVE_ADS";
    public const string CURRENT_LEVEL = "CURRENT_LEVEL";
    public const string CURRENT_LEVEL_PLAY = "CURRENT_LEVEL_PLAY";
    public const string PATH_CONFIG_LEVEL = "Levels/Level_{0}";
    public const string PATH_CONFIG_LEVEL_BIRD1 = "LevelsBird1_LongTH/Level_{0}";
    public const string PATH_CONFIG_LEVEL_EASY = "Levels/Level_{0}";
    public const string PATH_CONFIG_LEVEL_HARD = "NewLevel/Level{0}";
    public const string PATH_CONFIG_LEVEL_TEST = "LevelsBird1/Level_{0}";
    public const string PATH_CONFIG_LEVEL_MKT = "LevelsMKT/Level_{0}";
    public const string PATH_CONFIG_MAP = "Maps/Map_{0}";


    public const string SALE_IAP = "_sale";

    public const string LOGGED_REVENUE_CENTS = "logged_revenue_cents";

    public const string PREVIOUS_REVENUE_D0_D1 = "previous_revenue_d0";
    public const string REVENUE_D0_D1 = "revenue_d0";
    public const string NUMBER_OF_DISPLAYED_INTERSTITIAL_D0_D1 = "number_of_displayed_interstitial_d0";

    public const string PREVIOUS_REVENUE_D1 = "previous_revenue_d1";
    public const string REVENUE_D1 = "revenue_d1";
    public const string NUMBER_OF_DISPLAYED_INTERSTITIAL_D1 = "number_of_displayed_interstitial_d1";

    public const string RETENTION_D = "retent_type";
    public const string DAYS_PLAYED = "days_played";
    public const string PAYING_TYPE = "retent_type";
    public const string LEVEL = "level";

    public const string LAST_TIME_OPEN_GAME = "LAST_TIME_OPEN_GAME";
    public const string FIRST_TIME_OPEN_GAME = "FIRST_TIME_OPEN_GAME";

    public const string CAN_SHOW_RATE = "CAN_SHOW_RATE";
    public const string LEVEL_SHOWED_RATE = "LEVEL_SHOWED_RATE";


    public const string IS_TUTED_RETURN = "IS_TUTED_RETURN";
    public const string CURRENT_NUM_RETURN = "CURRENT_NUM_RETURN";
    public const string CURRENT_NUM_ADD_STAND = "CURRENT_NUM_ADD_STAND";
    public const string CURRENT_NUM_REMOVE_BOMB = "CURRENT_NUM_REMOVE_BOMB";
    public const string CURRENT_NUM_REMOVE_CAGE = "CURRENT_NUM_REMOVE_CAGE";
    public const string CURRENT_NUM_REMOVE_EGG = "CURRENT_NUM_REMOVE_EGG";
    public const string CURRENT_NUM_REMOVE_SLEEP = "CURRENT_NUM_REMOVE_SLEEP";
    public const string CURRENT_NUM_REMOVE_JAIL = "CURRENT_NUM_REMOVE_JAIL";


    public const string IS_TUTED_BUY_STAND = "IS_TUTED_BUY_STAND";
    public const string ACCUMULATION_REWARD = "ACCUMULATION_REWARD";
    public const string CURRENT_BIRD_SKIN = "CURRENT_BIRD_SKIN";
    public const string CURRENT_BRANCH_SKIN = "CURRENT_BRANCH_SKIN";
    public const string CURRENT_THEME = "CURRENT_THEME";
    public const string OWNED_BIRD_SKIN = "OWNED_BIRD_SKIN";
    public const string OWNED_BRANCH_SKIN = "OWNED_BRANCH_SKIN";
    public const string OWNED_THEME = "OWNED_THEME";
    public const string RANDOM_BIRD_SKIN_IN_SHOP = "RANDOM_BIRD_SKIN_IN_SHOP";
    public const string RANDOM_BRANCH_IN_SHOP = "RANDOM_BRANCH_IN_SHOP";
    public const string RANDOM_THEME_IN_SHOP = "RANDOM_THEME_IN_SHOP";

    public const string RANDOM_BIRD_SKIN_SALE_WEEKEND_1 = "RANDOM_BIRD_SKIN_SALE_WEEKEND_1";
    public const string RANDOM_BRANCH_SALE_WEEKEND_2 = "RANDOM_BRANCH_SALE_WEEKEND_2";
    public const string RANDOM_THEME_SALE_WEEKEND_2 = "RANDOM_THEME_SALE_WEEKEND_2";

    public const string CURRENT_RANDOM_BIRD_SKIN = "CURRENT_RANDOM_BIRD_SKIN";
    public const string CURRENT_RANDOM_BRANCH_SKIN = "CURRENT_RANDOM_BRANCH_SKIN";
    public const string CURRENT_RANDOM_THEME = "CURRENT_RANDOM_THEME";


    public const string NUM_SHOWED_ACCUMULATION_REWARD_RANDOM = "NUM_SHOWED_ACCUMULATION_REWARD_RANDOM";

    public const string NUMBER_OF_ADS_IN_DAY = "NUMBER_OF_ADS_IN_DAY";
    public const string NUMBER_OF_ADS_IN_PLAY = "NUMBER_OF_ADS_IN_PLAY";
    public const string NUMBER_INTER_SHOWED = "NUMBER_INTER_SHOWED";
    public const string NUMBER_REWARD_SHOWED = "NUMBER_REWARD_SHOWED";

    public const string IS_PACK_PURCHASED_ = "IS_PACK_PURCHASED_";
    public const string NUM_OF_PURCHASED_ = "NUM_OF_PURCHASED_";

    public const string IS_PACK_ACTIVATED_ = "IS_PACK_ACTIVATED_";
    public const string NUM_PACK_ACTIVATED_ = "NUM_PACK_ACTIVATED_";
    public const string LAST_TIME_PACK_ACTIVE_ = "LAST_TIME_PACK_ACTIVE_";
    public const string LAST_TIME_PACK_SHOW_HOME_ = "LAST_TIME_PACK_SHOW_HOME_";
    public const string STARTER_PACK_IS_COMPLETED = "STARTER_PACK_IS_COMPLETED";

    public const string HAS_PACK_IN_WEEK_TODAY = "HAS_PACK_IN_WEEK_TODAY";
    public const string HAS_PACK_WEEKEND_TODAY = "HAS_PACK_WEEKEND_TODAY";

    public const string CURRENT_PACK_IN_WEEK = "CURRENT_PACK_IN_WEEK";
    public const string CURRENT_PACK_WEEKEND = "CURRENT_PACK_WEEKEND";
    public const string CURRENT_RANDOM_SET_SKIN = "CURRENT_RANDOM_SET_SKIN";


    public const string LAST_TIME_RESET_SALE_PACK_SHOP = "LAST_TIME_RESET_SALE_PACK_SHOP";

    public const string LAST_TIME_ONLINE = "LAST_TIME_ONLINE";
    public const string LAST_TIME_ONLINE_HOME_DECOR = "last_time_online_home_decor";
    public const string CURRENT_ID_MINI_GAME = "current_id_mini_game";

    public const string LAST_TIME_PLAYFAB = "Last_Time_PlayFab";


    public const string DECOR_USER_DATA = "Decor_User_Data";
    public const string DECOR_ITEM_DATA = "Decor_Item_Data";
    public const string DECOR_ROOM_DATA = "Decor_Room_Data";
    public const string COIN = "Coin";
    public const string HEALTH = "Health";
    public const string PIECE_DATA = "piece_data";

    public const string BIRD_OF_PLAYER = "bird_of_player";
    public const string JSON_DATA_PLAY = "json_data_play";
    public const string VERSION_DATA = "version_data";
    public const string AVATAR_LINK = "avatar_link";
    public const string FLAG_LINK = "flag_link";
    public const string IS_LOGGED_IN = "is_logged_in";
    public static string SENCE_NAME;
    public static string OPEN_POPUP_PVP = "open_popup_pvp";
    public static string TIME_COOLDOWN_FREE_TICKIT = "time_cooldown_free_tickit";
    public static string WAS_CLAIM_REWARD_PVP = "was_claim_reward_pvp";

    public static string FREE_TICKIT_PVP = "free_tickit_pvp";
    public static string OPEN_POPUP_SEASON_ENDED = "open_popup_season_ended";
    public static string CONFIG_ON_OFF_HOME_DECOR = "config_on_off_home_decor";

    public const string AUTH_CODE_GG = "auth_code_gg";
    public static string USED_DECOR_ITEM = "USED_DECOR_ITEM";
    public static string OWNED_DECOR_ITEM = "OWNED_DECOR_ITEM";


    public const string PVP_CURRENT_ROOM = "PVP_CURRENT_ROOM";
    public const string LAST_TIME_SYNC = "lastTimeSync";
    public const string MAX_RANK = "MAX_RANK";

    public static string PURCHASED_PACK = "PURCHASED_PACK";

    #region MiniGame
    public const string VERSION_MINI_GAME_MINIGAME = "version_mini_game_minigame";
    public const string POINT_MINIGAME_CONNECT_BIRD = "point_minigame_connect_bird";
    public const string LIST_REWARD_MINI_GAME_CONNECT_BIRD = "list_reward_mini_game_connect_bird";
    public const string WAS_COMMPLETE_MINI_GAME_CONNECT_BIRD = "was_commplete_mini_game_connect_bird";
    public const string CONNECT_BIRD_TURN_FREE = "connect_bird_turn_free";
    public const string CONNECT_BIRD_TURN_BUY = "connect_bird_turn_buy";
    public const string LIMIT_NUMB_WATCH_VIDEO_IN_DAY = "limit_numb_watch_video_in_day";
    public const string FIRST_OPEN_CONNECT_BIRD_MINI_GAME = "first_open_connect_bird_mini_game";
    public const string FIRST_SHOW_HAND = "first_show_hand";
    public const string CURRENT_NUM_WATCH_VIDEO_IN_DAY_MINI_GAME_CONNECT_BIRD = "current_num_watch_video_in_day_mini_game_connect_bird";
    public const string DATA_TO_RESET_MINI_GAME_CONNECT_BIRD = "data_to_reset_mini_game_connect_bird";
    public const string DATA_TO_RESET_MINI_GAME_SHOP_CONNECT_BIRD = "data_to_reset_mini_game_shop_connect_bird";
    public const string DATA_TO_RESET_MINI_GAME_SOCK_CONNECT_BIRD = "data_to_reset_mini_game_sock_connect_bird";
    public const string FIRST_WEEK = "first_week";

    #endregion


    public const string TIME_WATCHED_SPECIAL_VIDEO_ = "time_watched_special_video_";


    public const string LINKED_ID = "linked_id";

    public const string IS_TRACKED_PREMISSION = "is_tracked_premission";
    public const string IS_ACCEPT_TRACKED_PREMISSION = "is_accept_tracked_premission";


    public const string NUM_ITEM = "num_item_";
    public const string USE_ITEM = "use_item_";
    public const string TIME_END_UNLIMITED_ITEM = "time_end_unlimited_item_";

    public const string TIME_END_UNLIMITED_HEALTH = "time_end_unlimited_health";
    public const string TIME_LAST_OVER_HEALTH = "time_last_over_health";

    public const string CURRENT_SEGMENT = "current_segment";

    public const string IS_REFILL_HEALTH = "is_refill_health";

    //Revive
    public const string CURRENT_TURN_SUGGET_VIDEO_REVIVE = "current_turn_sugget_video_revive";
    public const string LAST_TIME_OVER_SUGGET_VIDEO_REVIVE = "last_time_over_sugget_video_revive";

    public const string NUM_TURN_VIDEO_COIN_IN_DAY = "num_turn_video_coin_in_day";

    //Daily Login
    public const string LAST_TIME_CLAIM_REWARD_DAILY_LOGIN = "last_time_claim_reward_daily_login";
    public const string CURRENT_ID_DAILY_REWARD = "current_id_daily_reward";
    public const string CURRENT_WEEK_DAILY_REWARD = "current_week_daily_reward";
    public const string IS_CLAIMED_REWARD_DAILY_LOGIN = "is_claimed_reward_daily_login";

    //Star Chest
    public const string CURRENT_START_CHEST = "current_start_chest";
    public const string REMEMBER_CURRENT_START_CHEST = "remember_current_start_chest";

    //Level Chest
    public const string CURRENT_LEVEL_CHEST = "current_level_chest";
    public const string REMEMBER_CURRENT_LEVEL_CHEST = "remember_current_level_chest";

    public static string IQ_SCORE = "IQ_SCORE";

    public const string GDPRContentValue = "gdprcontent_value";

    //Tut
    public const string IS_DONE_TUT = "IS_DONE_TUT";

    //Buy Item Video
    public const string CURRENT_TURN_SUGGET_VIDEO_BUY_ITEM = "current_turn_sugget_video_buy_item_";
    public const string LAST_TIME_OVER_SUGGET_VIDEO_BUY_ITEM = "last_time_over_sugget_video_buy_item_";

    //Skill User
    public const string NUM_LOSE_LEVEL_TO_DAY = "num_lose_level_to_day";


    //Challenge Weekly
    public const string NUM_PLAYED_IN_DAY = "num_played_in_day";
    public const string NUM_PLAYED_LEVEL_IN_DAY = "num_played_level_in_day";
    public const string DAILY_TIME_END = "daily_time_end";
    public const string DAILY_BUY_TIME = "daily_buy_time";
    public const string COIN_SALE_TIME_END = "coin_sale_time_end";
    public const string COIN_BUY_TIME = "coin_buy_time";
    public const string ITEM_SALE_TIME_END = "item_sale_time_end";
    public const string ITEM_BUY_TIME = "item_buy_time";
    public const string WEEKEND_SALE_TIME_END = "weekend_sale_time_end";
    public const string WEEKEND_BUY_TIME = "weekend_buy_time";
    public const string CURRENT_SKILL_USER = "current_skill_user";
    //Royal Pass
    public const string IS_ACTIVE_ROYAL_PACK = "is_active_royal_pack";
    public const string IS_CLAIMED_ROYAL_PACK_INDEX_REWARD = "is_claimed_royal_pack_index_reward_";
    public const string IS_OPENED_TUT_ROYAL_PACK = "is_opened_tut_royal_pack";
    //Coin Challenge
    public const string IS_OPENED_TUT_COIN_CHALLENGE = "is_opened_tut_coin_challenge";
    //Diamod Challenge
    public const string IS_OPENED_TUT_DIAMOD_CHALLENGE = "is_opened_tut_diamod_challenge";

    //Race Challenge
    public const string CURRENT_SCORE_ELO_RACE_CHALLENGE = "current_score_elo_race_challenge";
    public const string TIME_START_RACE_RACE_CHALLENGE = "time_start_race_race_challenge";
    public const string IS_RACCING_RACE_CHALLENGE = "is_raccing_race_challenge";
    public const string IS_FINISHED_RACE = "is_finished_race";
    public const string IS_OPENED_TUT_RACE_CHALLENGE = "is_opened_tut_race_challenge";
    //Boss
    public const string PROGESS_BOSS_RACE_CHALLENGE = "progess_boss_race_challenge_";
    public const string REMEMBER_CURRENT_LEVEL_PASS_BOSS = "remember_current_level_pass_boss";
    public const string CURRENT_LEVEL_PASS_BOSS = "current_level_pass_boss";
    public const string NAME_BOSS = "name_boss_";
    public const string LINK_FLAG_BOSS = "link_flag_boss_";
    //Me
    public const string REMEMBER_CURRENT_LEVEL_PASS_ME = "remember_current_level_pass_me";
    public const string CURRENT_LEVEL_PASS_ME = "current_level_pass_me";
    public const string TIME_ME_FINISH_RACE_RACE_CHALLENGE = "time_me_finish_race_race_challenge";
    public const string LAST_TIME_MINIGAME_SHOW_HOME = "LAST_TIME_MINIGAME_SHOW_HOME";
}

public class PathPrefabs
{
    public const string POPUP_REWARD_BASE = "UI/Popups/PopupRewardBase";
    public const string CONFIRM_POPUP = "UI/Popups/ConfirmBox";
    public const string WAITING_BOX = "UI/Popups/WaitingBox";
    public const string WAITING_BOX_SMALL = "UI/Popups/WaitingBoxSmall";
    public const string AD_BREAK_BOX = "UI/Popups/AdBreakBox";

    public const string REWARD_IAP_BOX = "UI/Popups/RewardIAPBox";
    public const string SHOP_BOX = "UI/ShopBox";
    public const string RATE_GAME_BOX = "UI/Popups/RateGameBox";


    public const string REWARD_CONGRATULATION_BOX = "UI/Popups/RewardCongratulationBox";
    public const string SHOP_GAME_BOX = "UI/Popups/ShopBox";
    public const string CONGRATULATION_BOX = "UI/Popups/CongratulationBox";

    public const string STARTER_PACK_BOX = "UI/Popups/PackBoxes/StarterPackBox";

    public const string CREDIT_BOX = "UI/Popups/CreditBox";
    public const string TRACKING_BOX = "UI/Popups/TrackingBox";

    public const string PLAY_CAMPAIN_BOX = "UI/Popups/PlayCampainBox";
    public const string REVIVE_BOX = "UI/Popups/ReviveBox";
    public const string WIN_BOX = "UI/Popups/WinBox";
    public const string LOSE_BOX = "UI/Popups/LoseBox";
    public const string PAUSE_BOX = "UI/Popups/PasueBox";
    public const string SETTING_BOX = "UI/Popups/SettingBox";
    public const string QUIT_LEVEL_BOX = "UI/Popups/QuitLevelBox";
    public const string LANGUAGE_BOX = "UI/Popups/LanguageBox";
    public const string OUT_OF_HEALTH_BOX = "UI/Popups/OutOfHealthBox";
    public const string BUY_ITEM_BOX = "UI/Popups/BuyItemBox";
    public const string DAILY_LOGIN_BOX = "UI/Popups/DailyLoginBox";
    public const string OPEN_CHEST_BOX = "UI/Popups/OpenChestBox";
    public const string SUGGET_LOGIN_BOX = "UI/Popups/SuggetLoginBox";
    public const string REPLAY_BOX = "UI/Popups/ReplayBox";


    //Pack Sale
    public const string BIG_REMOVE_ADS_BOX = "UI/Popups/SalePacks/BigRemoveAdsBox";
    public const string DAILY_OFFER_BOX = "UI/Popups/SalePacks/DailyOfferBox";
    public const string SALE_COIN_BOX = "UI/Popups/SalePacks/SaleCoinBox";
    public const string SALE_ITEM_BOX = "UI/Popups/SalePacks/SaleItemBox";
    public const string WEEKLY_SALE_BOX = "UI/Popups/SalePacks/WeekendSaleBox";

    //Star Chest
    public const string CHEST_STAR_INFO_BOX = "UI/Popups/ChestStarInfoBox";

    //Level Chest
    public const string CHEST_LEVEL_INFO_BOX = "UI/Popups/ChestLevelInfoBox";

    //Video Sale
    public const string VIDEO_GIRL_BOX = "UI/Popups/VideoGirlBox";

    //Challegene
    public const string WEEKLY_CHALLEGEN_BOX = "UI/Popups/WeeklyChallegenBox";
    public const string WEEKLY_CHALLEGEN_TUT_BOX = "UI/Popups/WeeklyChallegenTutBox";
    public const string COIN_CHALLENGE_BOX = "UI/Popups/CoinChallengeBox";
    public const string ROYAL_PASS_BOX = "UI/Popups/RoyalPassBox";
    public const string ACTIVE_ROYAL_PASS_BOX = "UI/Popups/ActiveRoyalPassBox";
    public const string ROYAL_PASS_TUT_BOX = "UI/Popups/RoyalPassTutBox";
    public const string DIAMOD_CHALLENGE_BOX = "UI/Popups/DiamodChallengeBox";
    public const string SPACE_RACE_CHALLENGE_BOX = "UI/Popups/SpaceRaceChallengeBox";
    public const string JOIN_SPACE_RACE_BOX = "UI/Popups/JoinSpaceRaceBox";
    public const string SPACE_RACE_TUT_BOX = "UI/Popups/SpaceRaceTutBox";


    public const string MINI_GAME_CONNECT_BIRD_BOX = "UI/Popups/ConnectBirdMGBox";
    public const string MINI_GAME_TRIPLE_BIRD_BOX = "UI/Popups/TripleBirdMGBox";
    public const string CONNECT_BIRD_MINI_GAME_SHOP_BOX = "UI/Popups/ConnectBirdMiniGameShop";
    public const string TRIPLE_BIRD_MINI_GAME_SHOP_BOX = "UI/Popups/TripleBirdMiniGameShop";
    public const string REWARD_CONNECT_BIRD_MN_BOX = "UI/Popups/RewardConnectBirdMNBox";
    public const string REWARD_TRIPLE_BIRD_MN_BOX = "UI/Popups/RewardTripleBirdMNBox";
}


public class SceneName
{
    public const string LOADING_SCENE = "LoadingScene";
    public const string HOME_SCENE = "HomeScene";
    public const string GAME_PLAY = "GamePlay";
    public const string HOME_SCENE_PVP = "HomeScenePvP";
    public const string GAME_PLAY_PVP1 = "GamePlayPvP1";
    public const string HOME_SCENE_HOME_DECOR = "HomeSceneHomeDecor";

}


public class ActionOfBird
{
    public const string IDLE = "GAMEPLAY/idle";
    public const string EAT = "EATING/EATING";
    public const string SHOWER_SHAKING = "SHOWER/SHAKING";
    public const string SHOWER_ARGRY = "SHOWER/ANGRY";
}


public class AudioName
{
    public const string bgMainHome = "Music_BG_MainHome";
    public const string bgGamePlay = "Music_BG_GamePlay";

    //Ingame music
    public const string winMusic = "winMusic";
    public const string spawnerPlayerMusic = "spawnerPlayer";

    //Action Player music
    public const string jumpMusic = "jump";
    public const string jumpEndMusic = "jumpEnd";
    public const string swapMusic = "swap";
    public const string pushRockMusic = "pushRock";
    public const string dieMusic = "die";
    public const string reviveMusic = "revive";
    public const string flyMusic = "fly";

    //Collect music
    public const string collectCoinMusic = "collectCoin";
    public const string collectKeyMusic = "collectKey";
    public const string collectItemSound = "collectItem";

    //Level music
    public const string jumpOnWaterMusic = "jumpOnWater";
    public const string collisionDoorMusic = "collisionDoor";
    public const string doorOpenMusic = "doorOpen";
    public const string doorCloseMusic = "doorClose";
    public const string springMusic = "spring";
    public const string touchSwitchMusic = "touchSwitch";
    public const string bridgeMoveMusic = "bridgeMove";
    public const string bridgeMoveEndMusic = "bridgeMoveEnd";
    public const string iceDropFall = "rock1";
    public const string iceDropExplosion = "bigrock";
    public const string activeDiamond = "crystalactive";
    public const string releaseDiamond = "crystalrelease";
    //UI Music
    public const string buttonClick = "buttonClick";
}

public class KeyPref
{
    public const string SERVER_INDEX = "SERVER_INDEX";
}

public class FirebaseConfig
{
    public const string DIFFICULTY_MULTIPLIER = "difficulty_multiplier";
    public const string IS_SHOW_INTER_REPLAY_LEVEL = "is_show_inter_replay_level";

    public const string DELAY_SHOW_INITSTIALL = "delay_show_initi_ads";//Thời gian giữa 2 lần show inital 30
    public const string LEVEL_START_SHOW_INITSTIALL = "level_start_show_initstiall";//Level bắt đầu show initial//3


    public const string LEVEL_START_SHOW_RATE = "level_start_show_rate";//Level bắt đầu show popuprate



    public const string LEVEL_START_TUT_RETURN = "level_start_tut_return";//4
    public const string LEVEL_START_TUT_BUY_STAND = "level_start_tut_buy_stand";//5

    public const string ON_OFF_REMOVE_ADS = "on_off_remove_ads_2";//5
    public const string MAX_LEVEL_SHOW_RATE = "max_level_show_rate";//30

    public const string TEST_LEVEL_CAGE_BOOM = "test_level_cage_boom_ver2";//30

    public const string ON_OFF_ACCUMULATION_REWARD_LEVEL_START = "on_off_accumulation_reward_level_start";//true
    public const string ACCUMULATION_REWARD_LEVEL_START = "accumulation_reward_level_start";//6
    public const string ACCUMULATION_REWARD_END_LEVEL = "accumulation_reward_end_level_{0}";//
    public const string ACCUMULATION_REWARD_TIME_SHOW_NEXT_BUTTON = "accumulation_reward_time_show_next_button";//1.5
    public const string ACCUMULATION_REWARD_END_LEVEL_RANDOM = "accumulation_reward_end_level_random";//10
    public const string MAX_TURN_ACCUMULATION_REWARD_END_LEVEL_RANDOM = "max_turn_accumulation_reward_end_level_random";//150

    public const string ON_OFF_SALE_INAPP = "on_off_sale_inapp";//true

    public const string LEVEL_UNLOCK_SALE_PACK = "level_unlock_sale_pack"; //11
    public const string LEVEL_UNLOCK_PREMIUM_PACK = "level_unlock_premium_pack"; //25
    public const string TIME_LIFE_STARTER_PACK = "time_life_starter_pack"; // 3DAY
    public const string TIME_LIFE_PREMIUM_PACK = "time_life_premium_pack"; // 2DAY
    public const string TIME_LIFE_SALE_PACK = "time_life_premium_pack"; // 1DAY
    public const string TIME_LIFE_BIG_REMOVE_ADS_PACK = "time_life_big_remove_ads_pack"; // 3h

    public const string NUMBER_OF_ADS_IN_DAY_TO_SHOW_PACK = "number_of_ads_in_day_to_show_pack"; //5ADS
    public const string NUMBER_OF_ADS_IN_PLAY_TO_SHOW_PACK = "number_of_ads_in_play_to_show_pack"; //3ADS
    public const string TIME_DELAY_SHOW_POPUP_SALE_PACK_ = "time_delay_show_popup_sale_pack_"; // 6H
    public const string TIME_DELAY_ACTIVE_SALE_PACK = "time_delay_active_sale_pack_"; // 6H

    public const string CONFIG_SALE_PACK_HALLOWEEN = "config_sale_pack_halloween";
    public const string CONFIG_SALE_PACK_BLACK_FRIDAY = "config_sale_pack_black_friday";
    public const string CONFIG_SALE_PACK_CHRISTMAS = "config_sale_pack_christmas";

    public const string CONFIG_EVENT_GAME = "config_event_game";

    public const string GAME_THEME_CONFIG = "game_theme_config";
    public const string TIME_COOL_DOWN_RETURN_PVP1 = "time_cool_down_return_pvp1";
    public const string TIME_COOL_DOWN_REPLAY_PVP1 = "time_cool_down_replay_pvp1";
    public const string SENCE_GAMEPLAY_OLD = "sence_gameplay_old";
    public const string ON_OFF_PVP1 = "on_off_pvp1";
    public const string DEFAULT_NUM_ADD_STAND = "default_num_add_stand";
    public const string TIME_COOL_DOWN_PVP1_TICKET = "time_cool_down_pvp1_ticket";

    //Banner Collap
    public const string ENABLE_ADMOB_BANNER = "enable_banner_admob";
    public const string ENABLE_ADMOB_COLLAPSE_BANNER = "enable_collapse_banner_admob";
    public const string COOLDOWN_ADMOB_REFRESH = "cooldown_admob_refresh";
    public const string MINIMUM_TIME_SHOW_COLLAPSE = "minimum_time_show_collapse";

    public const string RELOAD_BANNER_COLLAPSE_TIME = "reload_banner_collapse_time";
    public const string ENABLE_ADMOB_BANNER_COLLAP = "enable_admob_banner_collap";
    public const string COOL_DOWN_SHOW_COLLAP_BANNER = "cool_down_show_collap_banner";

    // Interstitial
    public const string ENABLE_INTERSTITIAL = "enable_interstitial";
    public const string ENABLE_APP_OPEN_ADS = "enable_app_open_ads";

    //Controll level
    public const string ENABLE_HARD_LEVEL_SET = "enable_hard_level_set";
}


public static class DataPlayFab
{
    public const string PLAYFAB_ID = "PLAY_FAB_ID";
    public const string PLAYER_DATA = "DATA_NORMAL_PLAYER";
}


public class LayerGame
{
    public const string BIRD = "Bird";
    public const string BIRD_FLY = "BirdFly";
    public const string BIRD_SLOT = "BirdSlot";
}

public class AdsPlacement
{
    public class Interstitials
    {
        public const string LEVEL_CAMPAIN_FAIL = "level_campain_fail";
        public const string RESUME_CAMPAIN = "resume_campain";
        public const string STAY_ON_QUIT_POPUP_CAMPAIN = "stay_on_quit_popup_campain";
        public const string QUIT_LEVEL_CAMPAIN = "quit_level_campain";
        public const string NEXT_LEVEL_CAMPAIN = "next_level_campain";
        public const string REPLAY_LEVEL_CAMPAIN = "replay_level_campain";
    }

    public class VideoReward
    {
        public const string REVIVE_VIDEO = "revive_video";
        public const string VIDEO_IN_SHOP = "video_in_shop_";
        public const string VIDEO_COIN_IN_SHOP = "video_coin_in_shop";
        public const string ADD_HEALTH = "add_health";
        public const string VIDEO_GIRL = "video_girl";
        public const string X_REWARD = "x_reward";
        public const string X_COIN_WIN_LEVEL = "x_coin_win_level";
        public const string BUY_ITEM_VIDEO_NORMAL = "buy_item_video_normal";
        public const string BUY_ITEM_IN_POPUP_SUGGET = "buy_item_in_popup_sugget";
    }
}

public static class LocalizationKey
{
    public const string LOCAL_NOTI_COMEBACK_TITLE_1 = "s_local_noti_comeback_game_title_1";
    public const string LOCAL_NOTI_COMEBACK_TITLE_2 = "s_local_noti_comeback_game_title_2";
    public const string LOCAL_NOTI_COMEBACK_TITLE_3 = "s_local_noti_comeback_game_title_3";
    public const string LOCAL_NOTI_COMEBACK_1 = "s_local_noti_comeback_game_1";
    public const string LOCAL_NOTI_COMEBACK_2 = "s_local_noti_comeback_game_2";
    public const string LOCAL_NOTI_COMEBACK_3 = "s_local_noti_comeback_game_3";
    public const string LOCAL_NOTI_COMEBACK_TITLE_ = "s_local_noti_comeback_game_title_";
    public const string LOCAL_NOTI_COMEBACK_ = "s_local_noti_comeback_game_";
    public const string FREE_SOCK = "s_free_sock_in";
    public const string CLAIM_NOW = "s_claimnow";
    public const string LOADING = "s_loading";
    public const string LOWER_NOTIFICATION = "s_notification_lower";
    public const string CANT_CONNECT_TO_SERVER = "s_cant_connect";
    public const string CANT_CONNECT_CHECK_INTERNET = "s_cant_connect_check_internet";
    public const string NOT_ENOUGH_BIRD_CARD = "s_not_enough_card";
    public const string TIME_LEFT = "s_timeleft";
    public const string LEVEL = "s_level";
    public const string RESET_IN = "s_resetin";
    public const string DEALS = "s_deals";
    public const string WISH = "s_wish";
    public const string VERSION = "s_version";
    public const string USER_ID = "s_userid";
    public const string FREE_TICKET_IN = "s_free_ticket_in";
    public const string WAVE = "s_wave";
    public const string UNLOCK_AT_LEVEL = "s_unlock_at_level";
    public const string BUY_TO_UNLOCK = "s_buy_to_unlock";
    public const string COMPLETE = "s_complete";
    public const string FAILED = "s_failed";
    public const string NOADS_MOMENT = "s_noads_moment";
    public const string PLEASE_CONNECT_INTERNET = "s_please_connect_internet";
    public const string NEED_MORE_COIN = "s_need_more_coin";
    public const string NEED_MORE_GEM = "s_need_more_gem";
    public const string DONT_HAVE_ITEM = "s_dont_have_item";
    public const string DOWNLOAD_FAILED = "s_msg_download_failed";
    public const string CONNECT_FAILED = "s_msg_connect_failed";
    public const string PASS_LEVEL = "s_pass_level";
    public const string THANK_FOR_REVIEW = "s_thank_for_review";
    public const string TRY_AGAIN = "s_tryagain";
    public const string PLEASE_ENTER_NAME = "s_please_enter_name";
    public const string NAME_TAKEN = "s_name_taken";
    public const string COMING_SOON = "s_comingsoon";
    public const string END_IN = "s_end_in";
    public const string DAYS = "s_days";
    public const string DAY_HOUR = "s_day_hour";
    public const string FREE = "s_free";
    public const string SALE_OFF = "s_sale_off";
    public const string TURNS_REMAIN = "s_turnsremain";
    public const string LOCAL = "s_local";
    public const string SERVER = "s_server";
    public const string DO_YOU_GET_DATA = "s_do_you_get_data";
    public const string DO_YOU_SURRENDER = "s_do_you_surrender";
    public const string LOGIN = "s_login";
    public const string YES = "s_yes";
    public const string NO = "s_no";
    public const string SURRENDER = "s_surrender";
    public const string RESTORE_FAIL = "s_restore_fail";
    public const string RESTORE_SUCCESS = "s_restore_success";
    public const string SYNC_FAILED = "s_sync_failed";
    public const string WARNING = "s_warning";
    public const string CREATE_MATCH_FAILED = "s_create_match_failed";
    public const string TAP_AGAIN_EXIT = "s_tap_again_exit";

}