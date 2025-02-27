using System;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class ReplayPopup : BasePopup
{
    [SerializeField] private Button replayLevelButton;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private GameSetting gameSetting;
    [SerializeField] private LevelObserver levelObserver;
    [SerializeField] private LevelBoosterObserver levelBoosterObserver;

    public static event Action replayLevelEvent;
    public static event Action<int> changeLivesNumberEvent;

    public static event Action<ScreenRoute> switchRouteEvent;

    protected override void MoreActionInAwake()
    {
        LivesShopPopup.runPendingReplayCommandEvent += ActualReplay;

        replayLevelButton.onClick.AddListener(Replay);
    }

    protected override void MoreActionOnDestroy()
    {
        LivesShopPopup.runPendingReplayCommandEvent -= ActualReplay;
    }

    private void Replay()
    {
        if (GamePersistentVariable.livesData.CurrentLives <= 0)
        {
            GamePersistentVariable.isPendingReplay = true;

            switchRouteEvent?.Invoke(ScreenRoute.LivesShop);

            return;
        }

        bool isShowInterReplayLevel = RemoteConfigController.GetBoolConfig(FirebaseConfig.IS_SHOW_INTER_REPLAY_LEVEL, false);

        if (isShowInterReplayLevel)
        {
            AdmobAdsMax.Instance.ShowInterstitial(actionIniterClose: ActualReplay);
        }
        else
        {
            ActualReplay();
        }

        Hide();
    }

    private void ActualReplay()
    {
        SaferioTracking.TrackLevelLose(currentLevel.Value, levelObserver.Progress, levelBoosterObserver, EndLevelReason.Replay.ToString());

        replayLevelEvent?.Invoke();
        changeLivesNumberEvent?.Invoke(-1);
    }
}
