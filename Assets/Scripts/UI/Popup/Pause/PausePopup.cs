using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static GameEnum;

public class PausePopup : BasePopup
{
    [SerializeField] private Button returnHomeButton;
    [SerializeField] private Button replayLevelButton;
    [SerializeField] private SaferioToggle turnMusicToggle;
    [SerializeField] private SaferioToggle turnSoundToggle;
    [SerializeField] private SaferioToggle vibrationToggle;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private LevelObserver levelObserver;
    [SerializeField] private LevelBoosterObserver levelBoosterObserver;

    public static event Action<bool> enableBackgroundMusicEvent;
    public static event Action<bool> enableGameSoundEvent;
    public static event Action replayLevelEvent;
    public static event Action<ScreenRoute> switchRouteEvent;

    protected override void MoreActionInAwake()
    {
        turnMusicToggle.SetState(gameSetting.IsTurnOnBackgroundMusic);
        turnSoundToggle.SetState(gameSetting.IsTurnOnSound);
        vibrationToggle.SetState(gameSetting.IsVibrate);
    }

    protected override void RegisterMoreEvent()
    {
        ScriptableObjectInitializer.gameSettingLoadedEvent += OnGameSettingLoaded;

        returnHomeButton.onClick.AddListener(ReturnHome);
        replayLevelButton.onClick.AddListener(Replay);
        turnMusicToggle.RegisterOnSwitchedEvent(EnableGameMusic);
        turnSoundToggle.RegisterOnSwitchedEvent(EnableGameSound);
        vibrationToggle.RegisterOnSwitchedEvent(EnableVibration);
    }

    protected override void UnregisterMoreEvent()
    {
        ScriptableObjectInitializer.gameSettingLoadedEvent -= OnGameSettingLoaded;
    }

    private void ReturnHome()
    {
        switchRouteEvent?.Invoke(ScreenRoute.ReturnHome);
        
        // // avoid spamming
        // returnHomeButton.interactable = false;
        // replayLevelButton.interactable = false;

        // SaferioTracking.TrackLevelLose(currentLevel.Value, levelObserver.Progress, levelBoosterObserver, EndLevelReason.ReturnHome.ToString());

        // Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }

    private void Replay()
    {
        // bool isShowInterReplayLevel = RemoteConfigController.GetBoolConfig(FirebaseConfig.IS_SHOW_INTER_REPLAY_LEVEL, false);

        // if (isShowInterReplayLevel)
        // {
        //     AdmobAdsMax.Instance.ShowInterstitial(actionIniterClose: OnInterstitialAdToReplayCompleted);
        // }
        // else
        // {
        //     ActualReplay();
        // }

        // Hide();
    }

    private void ActualReplay()
    {
        SaferioTracking.TrackLevelLose(currentLevel.Value, levelObserver.Progress, levelBoosterObserver, EndLevelReason.Replay.ToString());

        replayLevelEvent?.Invoke();

        // Hide();
    }

    private void OnInterstitialAdToReplayCompleted()
    {
        // SaferioTracking.TrackInterstitialAdCompleted(currentLevel.Value, "replay");

        ActualReplay();
    }

    private void EnableGameMusic(bool isTurnOn)
    {
        gameSetting.IsTurnOnBackgroundMusic = isTurnOn;

        enableBackgroundMusicEvent?.Invoke(isTurnOn);
    }

    private void EnableGameSound(bool isTurnOn)
    {
        gameSetting.IsTurnOnSound = isTurnOn;

        enableGameSoundEvent?.Invoke(isTurnOn);
    }

    private void EnableVibration(bool isTurnOn)
    {
        gameSetting.IsVibrate = isTurnOn;

        GamePersistentVariable.IsVibrate = isTurnOn;
    }

    private void OnGameSettingLoaded()
    {
        turnMusicToggle.SetState(gameSetting.IsTurnOnBackgroundMusic);
        turnSoundToggle.SetState(gameSetting.IsTurnOnSound);
        vibrationToggle.SetState(gameSetting.IsVibrate);
    }
}
