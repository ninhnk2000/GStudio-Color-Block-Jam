using UnityEngine;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    // public UseProfile useProfile;
    public AdmobAdsMax admobAds;
    public AnalyticsController AnalyticsController;
    public SegmentController segmentController;
    [HideInInspector] public SceneType currentScene;

    [HideInInspector] public bool openFeatureBox = false;

    public static bool isFirstLoadLevel = false;
    protected void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Init();
    }
    public void Init()
    {
        Application.targetFrameRate = 60;

        UserData.CurrentLevelPlay = UserData.CurrentLevel;

        //notificationController.Initialize();

        //MMVibrationManager.SetHapticsActive(useProfile.OnVibration);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
//         ResetDay();
//         UseProfile.NumPlayedInDay++;

//         GplayUMP.ShowConsentForm((consert) =>
//         {
//             UseProfile.GDPRContentValue = consert;
// #if UNITY_IOS
//  if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
//             {
//                 ATTrackingStatusBinding.RequestAuthorizationTracking();
//             }
// #endif
//             InitLoadScene();
//         });
//         //notificationController.Initialize();

    }
    public void InitLoadScene()
    {
        segmentController.Init();
        admobAds.Init();
    }

    // public static void SetUserProperties()
    // {
    //     if (UseProfile.IsFirstTimeInstall)
    //     {
    //         UseProfile.FirstTimeOpenGame = UnbiasedTime.Instance.Now();
    //         UseProfile.LastTimeOpenGame = UseProfile.FirstTimeOpenGame;
    //         UseProfile.IsFirstTimeInstall = false;

    //         //AnalyticsController.LogFirstOpen(0);
    //     }

    //     var lastTimeOpen = UseProfile.LastTimeOpenGame;
    //     UseProfile.RetentionD = (UnbiasedTime.Instance.Now() - UseProfile.FirstTimeOpenGame).Days;

    //     var dayPlayerd = (TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()) - TimeManager.ParseTimeStartDay(UseProfile.LastTimeOpenGame)).Days;
    //     if (dayPlayerd >= 1)
    //     {
    //         UseProfile.LastTimeOpenGame = UnbiasedTime.Instance.Now();
    //         UseProfile.DaysPlayed++;

    //         // AnalyticsController.LogFirstOpen(UseProfile.DaysPlayed);
    //     }

    //     AnalyticsController.SetUserProperties();
    // }

    // public void ResetDay()
    // {
    //     ///Các loại ResetDay ở đây
    //     long time = TimeManager.CaculateTime(TimeManager.ParseTimeStartDay(UseProfile.LastTimeOnline), TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()));
    //     if (time >= 86400)
    //     {
    //         UseProfile.LastTimeOnline = UnbiasedTime.Instance.Now();
    //         ShopItemVideoCoin.NumTurnVideoCoinInDay = 0;

    //         UseProfile.NumberLoseLevelInDay = 0;

    //         UseProfile.NumPlayedInDay = 0;

    //         UseProfile.ResetDayMiniGame();

    //     }
    // }

    private void OnApplicationQuit()
    {
        //AnalyticsController.SessionLog(Time.time);
    }
}
public enum SceneType
{
    StartLoading = 0,
    MainHome = 1,
    GamePlay = 2
}