using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class LivesShopPopup : BasePopup
{
    [SerializeField] private Button refillByAdsButton;
    [SerializeField] private Button refillByCoinButton;
    [SerializeField] private TMP_Text currentLivesText;

    private LivesData _livesData;

    public static event Action<int> changeLivesNumberEvent;
    public static event Action<ScreenRoute> switchRouteEvent;
    public static event Action runPendingReplayCommandEvent;

    protected override void MoreActionInAwake()
    {
        refillByCoinButton.onClick.AddListener(RefillByCoin);
        refillByAdsButton.onClick.AddListener(RefillByAds);
    }

    protected override void Show(Action onCompletedAction = null)
    {
        base.Show(onCompletedAction);

        LivesData livesData = DataUtility.Load(GameConstants.USER_LIVES_DATA, new LivesData());

        currentLivesText.text = $"{livesData.CurrentLives}";
    }

    private void RefillByAds()
    {
        AdmobAdsMax.Instance.ShowVideoReward(
            OnRewaredAdCompletedToRefillLives, actionNotLoadedVideo: ShowAdsNotLoadedPopup, actionClose: null, actionType: ActionWatchVideo.UnlockScrewBox);
    }

    private void OnRewaredAdCompletedToRefillLives()
    {
        Hide(onCompletedAction: () =>
        {
            ActualRefill();
        });
    }

    private void ShowAdsNotLoadedPopup()
    {
        switchRouteEvent?.Invoke(ScreenRoute.NoInternet);
    }

    private void RefillByCoin()
    {
        ActualRefill();
    }

    private void ActualRefill() {
        changeLivesNumberEvent?.Invoke(1);

        Hide();

        if(GamePersistentVariable.isPendingReplay) {
            runPendingReplayCommandEvent?.Invoke();

            GamePersistentVariable.isPendingReplay = false;
        }
    }
}
