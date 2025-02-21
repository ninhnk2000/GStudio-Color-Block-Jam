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

    protected override void MoreActionInAwake()
    {
        refillByCoinButton.onClick.AddListener(RefillByCoin);
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
            changeLivesNumberEvent?.Invoke(1);

            Hide();
        });
    }

    private void ShowAdsNotLoadedPopup()
    {
        switchRouteEvent?.Invoke(ScreenRoute.NoInternet);
    }

    private void RefillByCoin()
    {
        changeLivesNumberEvent?.Invoke(1);

        Hide();
    }
}
