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
        replayLevelButton.onClick.AddListener(Replay);
    }

    private void Replay()
    {
        if (GamePersistentVariable.livesData.CurrentLives <= 0)
        {
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
