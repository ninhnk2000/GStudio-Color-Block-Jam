using System;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class LivesShopPopup : BasePopup
{
    [SerializeField] private Button refillByAdsButton;
    [SerializeField] private Button refillByCoinButton;

    private LivesData _livesData;

    public static event Action<int> changeLivesNumberEvent;
    public static event Action<ScreenRoute> switchRouteEvent;

    protected override void MoreActionInAwake()
    {
        refillByCoinButton.onClick.AddListener(RefillByCoin);
    }

    private void RefillByAds()
    {
        AdmobAdsMax.Instance.ShowVideoReward(
            OnRewaredAdCompletedToUnlockBox, actionNotLoadedVideo: ShowAdsNotLoadedPopup, actionClose: null, actionType: ActionWatchVideo.UnlockScrewBox);

    }

    private void OnRewaredAdCompletedToUnlockBox()
    {
        Hide(onCompletedAction: () =>
        {
            changeLivesNumberEvent?.Invoke(1);
        });
    }

    private void ShowAdsNotLoadedPopup()
    {
        switchRouteEvent?.Invoke(ScreenRoute.NoInternet);
    }

    private void RefillByCoin()
    {
        changeLivesNumberEvent?.Invoke(1);
    }
}
